namespace ReleaseStats.Providers.GitHub
{
    using System.Collections.Generic;
    using Octokit;

    public class GitHubProjectProvider:IListProjects
    {
        readonly string organization;
        GitHubClient client;
        public GitHubProjectProvider(string organization)
        {
            this.organization = organization;
            client = GitHubClientBuilder.Build();
        }


        public IEnumerable<Project> FindMatching(string filter)
        {
            yield return new Project("NServiceBus");
        }
    }
}