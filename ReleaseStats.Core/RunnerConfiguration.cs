namespace ReleaseStats
{
    using System.Collections.Generic;
    using ReleaseStats.PropertyEnrichers;
    using ReleaseStats.Validators;

    public class RunnerConfiguration
    {
        internal List<IStatsProvider> providers = new List<IStatsProvider>();
        internal List<PropertyEnricher> PropertyEnrichers = new List<PropertyEnricher>();
        internal List<IProviderValidator> providerValidators = new List<IProviderValidator>(); 
        public void AddProvider(IStatsProvider statsProvider)
        {
            providers.Add(statsProvider);
        }
        public void AddProviderValidator(IProviderValidator validator)
        {
            providerValidators.Add(validator);
        }

        public void AddEnricher(PropertyEnricher enricher)
        {
            PropertyEnrichers.Add(enricher);
        }
    }
}