namespace ReleaseStats
{
    using System;
    using System.Linq;

    public class ReleaseStatsRunner : IDisposable
    {
        readonly RunnerConfiguration runnerConfiguration;

        internal ReleaseStatsRunner(RunnerConfiguration runnerConfiguration)
        {
            this.runnerConfiguration = runnerConfiguration;
        }

        public void Dispose()
        {

        }

        public ReleaseStatistics GenerateStatistics()
        {
            var result = new ReleaseStatistics();
            
            foreach (var provider in runnerConfiguration.providers)
            {
                var providerResult = provider.FetchStats("NServiceBus");

                var validationErrors = runnerConfiguration.providerValidators.SelectMany(v=>v.Validate(providerResult)).ToList();

                if (validationErrors.Any())
                {
                    throw new Exception("Validation errors found for provider: " + provider.GetType().Name + Environment.NewLine + string.Join(Environment.NewLine,validationErrors));
                }
                result.Releases.AddRange(providerResult);
            }
          
            runnerConfiguration.PropertyEnrichers.ForEach(e => e.Process(result));

            return result;
        }
    }
}