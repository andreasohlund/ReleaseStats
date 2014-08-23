namespace ReleaseStats.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Octokit;
    using Octokit.Internal;
    using Release = ReleaseStats.Release;

    public class GitHubStatsProvider : IStatsProvider
    {
        readonly string organization;
        GitHubClient client;
        public GitHubStatsProvider(string organization)
        {
            this.organization = organization;
            client = ClientBuilder.Build();
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

                var versionString = ghRelease.TagName;

                release = new Release(new SemVer(versionString));

                return true;

            }
            catch (Exception)
            {

                Console.Out.WriteLine("Bad release: " + ghRelease.TagName);
                return false;
            }
        }

        static class ClientBuilder
        {
            public static GitHubClient Build()
            {
                var credentialStore = new InMemoryCredentialStore(Helper.Credentials);

                var httpClient = new HttpClientAdapter(Helper.Proxy);

                var connection = new Connection(
                    new ProductHeaderValue("ReleaseStats"),
                    GitHubClient.GitHubApiUrl,
                    credentialStore,
                    httpClient,
                    new SimpleJsonSerializer());

                return new GitHubClient(connection);
            }
        }

        public static class Helper
        {
            // From https://github.com/octokit/octokit.net/blob/master/Octokit.Tests.Integration/Helper.cs

            static readonly Lazy<Credentials> _credentialsThunk = new Lazy<Credentials>(() =>
            {
                var githubUsername = Environment.GetEnvironmentVariable("OCTOKIT_GITHUBUSERNAME");

                var githubToken = Environment.GetEnvironmentVariable("OCTOKIT_OAUTHTOKEN");

                if (githubToken != null)
                    return new Credentials(githubToken);

                var githubPassword = Environment.GetEnvironmentVariable("OCTOKIT_GITHUBPASSWORD");

                if (githubUsername == null || githubPassword == null)
                    return Credentials.Anonymous;

                return new Credentials(githubUsername, githubPassword);
            });

            public static Credentials Credentials
            {
                get { return _credentialsThunk.Value; }
            }

            public static IWebProxy Proxy
            {
                get
                {
                    return null;
                    /*
                                    return new WebProxy(
                                        new System.Uri("http://myproxy:42"),
                                        true,
                                        new string[] {},
                                        new NetworkCredential(@"domain\login", "password"));
                    */
                }
            }
        }
    }
}