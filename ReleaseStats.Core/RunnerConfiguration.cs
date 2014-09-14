namespace ReleaseStats
{
    using System.Collections.Generic;
    using ReleaseStats.PropertyEnrichers;
    using ReleaseStats.Validators;

    public class RunnerConfiguration
    {
     
        public void AddStatsProvider(IStatsProvider statsProvider)
        {
            statsProviders.Add(statsProvider);
        }
        public void AddProviderValidator(IProviderValidator validator)
        {
            providerValidators.Add(validator);
        }

        public void AddEnricher(PropertyEnricher enricher)
        {
            PropertyEnrichers.Add(enricher);
        }

        public static RunnerConfiguration Default
        {
            get
            {
                var config = new RunnerConfiguration();

                config.AddProviderValidator(new DuplicateVersionsValidator());

                config.AddEnricher(new ReleaseHierarchyEnricher());

                return config;
            }
        }

        public void AddCleaner(IReleaseCleaner releaseCleaner)
        {
            releaseCleaners.Add(releaseCleaner);
        }

        public void AddProjectProvider(IListProjects listProjectsProvider)
        {
            projectProviders.Add(listProjectsProvider);
        }

        internal List<IStatsProvider> statsProviders = new List<IStatsProvider>();
        internal List<IListProjects> projectProviders = new List<IListProjects>();

        internal List<PropertyEnricher> PropertyEnrichers = new List<PropertyEnricher>();
        internal List<IProviderValidator> providerValidators = new List<IProviderValidator>();


        internal List<IReleaseCleaner> releaseCleaners = new List<IReleaseCleaner>(); 
    }
}