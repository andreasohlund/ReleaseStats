namespace ReleaseStats
{
    using System.Collections.Generic;

    public class GitHubStatsProvider : IStatsProvider
    {
        public IEnumerable<Release> FetchStats(string project)
        {
            var fake = new Release(new SemVer("4.6.3"));

            return new List<Release>
            {
                fake
            };
        }
    }
}