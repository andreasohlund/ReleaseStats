using System;
using NUnit.Framework;
using ReleaseStats;
using ReleaseStats.Providers;

[TestFixture, Explicit("Long running")]
public class GitHubProviderTests
{
    [Test]
    public void ReturnAllNServiceBusReleases()
    {
        var config = new RunnerConfiguration();

        config.AddProvider(new GitHubStatsProvider("Particular"));

        using (var releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config))
        {
            var result = releaseStatsRunner.GenerateStatistics();

            Assert.Contains(new Release(new SemVer("4.6.3")), result.Releases);

            result.Releases.ForEach(r=>Console.Out.WriteLine(r.Version));
        }
    }
}

