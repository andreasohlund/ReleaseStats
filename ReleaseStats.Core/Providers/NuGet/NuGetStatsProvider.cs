namespace ReleaseStats.Providers.NuGet
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Xml;
    using ReleaseStats.ReleaseProperties;

    public class NuGetStatsProvider : IStatsProvider
    {
        public IEnumerable<Release> FetchStats(Project project)
        {
            var releases = new List<RemoteNuget>();

            var page = 1;
            var results = loadLatestFeed(project.Name, page);

            releases.AddRange(results);
            while (results.Count() == 40)
            {
                page++;
                results = loadLatestFeed(project.Name, page);
                releases.AddRange(results);
            }

            foreach (var nuGetRelease in releases)
            {
                Release release;
                if (TryParseRelease(nuGetRelease, out release))
                    yield return release;
            }
        }

        bool TryParseRelease(RemoteNuget nuGetRelease, out Release release)
        {
            release = new Release(new SemVer(nuGetRelease.Version));

         
            if(nuGetRelease.PublishedAt > DateTimeOffset.Parse("1901-01-01"))
            {
                release.Properties.Add(new ReleaseDate(nuGetRelease.PublishedAt));

            }
            else
            {
                Console.Out.WriteLine(release + " has an invalid release date (probably due to it being unlisted from nuget.org)");
            }
     
           return true;
        }

        static IEnumerable<RemoteNuget> loadLatestFeed(string packageId, int page)
        {
            var toSkip = (page - 1) * 40;

            return new NugetXmlFeed(GetNuGetData(string.Format("http://packages.nuget.org/v1/FeedService.svc/Packages()?$filter=Id eq '{0}'&$orderby=Published desc&$skip={1}&$top=40", packageId, toSkip))).ReadAll().ToArray();
        }

        static XmlDocument GetNuGetData(string url)
        {
            var client = new WebClient();
            var text =
                client.DownloadString(url);

            var document = new XmlDocument();
            document.LoadXml(text);

            return document;
        }

    }
}
