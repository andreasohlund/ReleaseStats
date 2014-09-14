namespace ReleaseStats.Providers.GitHub
{
    using System;
    using System.Collections.Generic;
    using Octokit;
    using ReleaseStats.ReleaseProperties;
    using Release = ReleaseStats.Release;

    public class GitHubStatsProvider : IStatsProvider
    {
        readonly string organization;
        GitHubClient client;
        public GitHubStatsProvider(string organization)
        {
            this.organization = organization;
            client = GitHubClientBuilder.Build();
        }

        public IEnumerable<Release> FetchStats(string project)
        {
            var releasesClient = client.Release;
            var releases = releasesClient.GetAll(organization, project).Result;

            foreach (var ghRelease in releases)
            {
                Release release;
                if (TryParseRelease(ghRelease, out release))
                    yield return release;
            }
        }

        bool TryParseRelease(Octokit.Release ghRelease, out Release release)
        {
            release = null;

            try
            {
                if (ghRelease.Draft)
                {
                    return false;
                }

                if (!ghRelease.PublishedAt.HasValue)
                {
                    return false;
                }

                var versionString = ghRelease.TagName;

                release = new Release(new SemVer(versionString));

                release.Properties.Add(new ReleaseDate(ghRelease.PublishedAt.Value));

                return true;

            }
            catch (Exception ex)
            {

                Console.Out.WriteLine("Bad release: " + ghRelease.TagName + " " + ex); 
                return false;
            }
        }

        
    }
}