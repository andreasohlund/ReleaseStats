namespace ReleaseStats
{
    using System.Collections.Generic;

    public class RunnerConfiguration
    {
        internal List<IStatsProvider> providers = new List<IStatsProvider>();
        public void AddProvider(IStatsProvider statsProvider)
        {
            providers.Add(statsProvider);
        }
    }
}