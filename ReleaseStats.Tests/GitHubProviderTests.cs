using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ReleaseStats;
using ReleaseStats.PropertyEnrichers;
using ReleaseStats.Providers.GitHub;
using ReleaseStats.ReleaseProperties;

[TestFixture, Explicit("Long running")]
public class GitHubProviderTests
{
    [Test]
    public void ReturnAllNServiceBusReleases()
    {
        var config = new RunnerConfiguration();

        config.AddProvider(new GitHubStatsProvider("Particular"));

        config.AddEnricher(new ReleaseHierarchyEnricher());

        using (var releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config))
        {
            var result = releaseStatsRunner.GenerateStatistics();

            Assert.Contains(new Release(new SemVer("4.6.3")), result.Releases);

            var groupedByOriginalRelease = result.Releases.GroupBy(r => r.Property<ReleaseHierarchy>().OriginalRelease)
                .OrderByDescending(g=>g.Key.Property<ReleaseDate>().ReleasedAt)
                .ToList();

            groupedByOriginalRelease.ForEach(PrintRelease);
        }
    }

    static void PrintRelease(IGrouping<Release, Release> grouping)
    {
        var sb = new StringBuilder();

        sb.Append(grouping.Key.Version);

        if (grouping.Count()>1)
        {
            sb.AppendFormat("({0})", string.Join(",", grouping.Where(r=>r!=grouping.Key).Select(patch => patch.Version)));
        }

        sb.AppendFormat(" - {0}", grouping.Key.Property<ReleaseDate>());

        Console.Out.WriteLine(sb);
    }
}

