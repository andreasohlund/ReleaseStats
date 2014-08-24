namespace ReleaseStats
{
    using System.Collections.Generic;
    using ReleaseStats.PropertyEnrichers;

    public class RunnerConfiguration
    {
        internal List<IStatsProvider> providers = new List<IStatsProvider>();
        internal List<PropertyEnricher> PropertyEnrichers = new List<PropertyEnricher>();
        public void AddProvider(IStatsProvider statsProvider)
        {
            providers.Add(statsProvider);
        }

        public void AddEnricher(PropertyEnricher enricher)
        {
            PropertyEnrichers.Add(enricher);
        }
    }
}