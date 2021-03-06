﻿namespace ReleaseStats.Providers.GitHub
{
    using Octokit;
    using Octokit.Internal;

    static class GitHubClientBuilder
    {
        public static GitHubClient Build()
        {
            var credentialStore = new InMemoryCredentialStore(GitHubHelper.Credentials);

            var httpClient = new HttpClientAdapter(null);

            var connection = new Connection(
                new ProductHeaderValue("ReleaseStats"),
                GitHubClient.GitHubApiUrl,
                credentialStore,
                httpClient,
                new SimpleJsonSerializer());

            return new GitHubClient(connection);
        }
    }
}