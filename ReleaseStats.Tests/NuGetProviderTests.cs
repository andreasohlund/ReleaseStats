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

        config.AddProvider(new NuGetStatsProvider());
        config.AddCleaner(new ConsolidateDuplicateReleasesCleaner());

        using (var releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config))
        {
            var result = releaseStatsRunner.GenerateStatistics("NServiceBus");

            Assert.Contains(new Release(new SemVer("4.6.3")), result.Releases);

            result.Releases.Where(r => !r.Version.IsPatchRelease)
     .ToList()
     .ForEach(ConsoleFormatter.PrintRelease);

        }
    }
}