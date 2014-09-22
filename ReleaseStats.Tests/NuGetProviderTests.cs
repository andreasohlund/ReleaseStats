using System.Linq;
using NUnit.Framework;
using ReleaseStats;
using ReleaseStats.Cleaners;
using ReleaseStats.Providers.NuGet;

[TestFixture, Explicit("Long running")]
public class NuGetProviderTests
{
    [Test]
    public void ReturnAllNServiceBusReleases()
    {
        var config = RunnerConfiguration.Default;

        config.AddStatsProvider(new NuGetStatsProvider());
        config.AddCleaner(new ConsolidateDuplicateReleasesCleaner());

        using (var releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config))
        {
            var result = releaseStatsRunner.GenerateStatistics("NServiceBus");

            Assert.Contains(new Release("4.6.3"), result.Releases);

            result.Releases.Where(r => !r.Version.IsPatchRelease)
     .ToList()
     .ForEach(ConsoleFormatter.PrintRelease);

        }
    }

    [Test]
    public void ReturnAllNServiceBusAzureReleases()
    {
        var config = RunnerConfiguration.Default;

        config.AddStatsProvider(new NuGetStatsProvider());
    
        using (var releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config))
        {
            var result = releaseStatsRunner.GenerateStatistics("NServiceBus.Azure");

            result.Releases.Where(r => !r.Version.IsPatchRelease)
     .ToList()
     .ForEach(ConsoleFormatter.PrintRelease);

        }
    }
}