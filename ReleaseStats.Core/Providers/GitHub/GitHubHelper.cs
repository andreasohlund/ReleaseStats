namespace ReleaseStats.Providers.GitHub
{
    using System;
    using Octokit;

    public static class GitHubHelper
    {
        static readonly Lazy<Credentials> _credentialsThunk = new Lazy<Credentials>(() =>
        {
            var githubUsername = Environment.GetEnvironmentVariable("RELEASESTATS_GITHUBUSERNAME");

            var githubToken = Environment.GetEnvironmentVariable("RELEASESTATS_OAUTHTOKEN");

            if (githubToken != null)
                return new Credentials(githubToken);

            var githubPassword = Environment.GetEnvironmentVariable("RELEASESTATS_GITHUBPASSWORD");

            if (githubUsername == null || githubPassword == null)
                return Credentials.Anonymous;

            return new Credentials(githubUsername, githubPassword);
        });

        public static Credentials Credentials
        {
            get { return _credentialsThunk.Value; }
        }
    }
}