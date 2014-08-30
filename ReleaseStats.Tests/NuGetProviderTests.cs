using System.Linq;
using NUnit.Framework;
using ReleaseStats;
using ReleaseStats.PropertyEnrichers;
using ReleaseStats.Providers.NuGet;
using ReleaseStats.ReleaseProperties;
using ReleaseStats.Validators;

[TestFixture, Explicit("Long running")]
public class NuGetProviderTests
{
    [Test]
    public void ReturnAllNServiceBusReleases()
    {
        var config = new RunnerConfiguration();

        config.AddProvider(new NuGetStatsProvider());
        config.AddProviderValidator(new DuplicateVersionsValidator());

        config.AddEnricher(new ReleaseHierarchyEnricher());

        using (var releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config))
        {
            var result = releaseStatsRunner.GenerateStatistics();

            Assert.Contains(new Release(new SemVer("4.6.3")), result.Releases);

            var groupedByOriginalRelease = result.Releases.GroupBy(r => r.Property<ReleaseHierarchy>().OriginalRelease)
                .OrderByDescending(g=>g.Key.Property<ReleaseDate>().ReleasedAt)
                .ToList();

            groupedByOriginalRelease.ForEach(ConsoleFormatter.PrintRelease);
        }
    }
}