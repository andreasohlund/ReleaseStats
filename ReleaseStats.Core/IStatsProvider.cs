namespace ReleaseStats
{
    using System.Collections.Generic;

    public interface IStatsProvider
    {
        IEnumerable<Release> FetchStats(string project);
    }
}