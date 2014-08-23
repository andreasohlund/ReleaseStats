namespace ReleaseStats
{
    public class ReleaseStatsFactory
    {
        public static ReleaseStatsRunner CreateRunner(RunnerConfiguration runnerConfiguration)
        {
            return new ReleaseStatsRunner(runnerConfiguration);
        }
    }
}