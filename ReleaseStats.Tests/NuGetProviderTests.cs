using System.Linq;
using NUnit.Framework;
using ReleaseStats;
using ReleaseStats.Providers.NuGet;
using ReleaseStats.ReleaseProperties;

[TestFixture, Explicit("Long running")]
public class NuGetProviderTests
{
    [Test]
    public void ReturnAllNServiceBusReleases()
    {
        var config = RunnerConfiguration.Default;

        config.AddProvider(new NuGetStatsProvider());
     
        using (var releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config))
        {
            var result = releaseStatsRunner.GenerateStatistics("NServiceBus");

            Assert.Contains(new Release(new SemVer("4.6.3")), result.Releases);

            var groupedByOriginalRelease = result.Releases.GroupBy(r => r.Property<ReleaseHierarchy>().OriginalRelease)
                .OrderByDescending(g=>g.Key.Property<ReleaseDate>().ReleasedAt)
                .ToList();

            groupedByOriginalRelease.ForEach(ConsoleFormatter.PrintRelease);
        }
    }
}