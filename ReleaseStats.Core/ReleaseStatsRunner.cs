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
            var stats = runnerConfiguration.providers.SelectMany(provider => provider.FetchStats("NServiceBus"))
                .ToList();
            var result = new ReleaseStatistics();

            result.Releases.AddRange(stats);

            runnerConfiguration.PropertyEnrichers.ForEach(e => e.Process(result));

            return result;
        }
    }
}