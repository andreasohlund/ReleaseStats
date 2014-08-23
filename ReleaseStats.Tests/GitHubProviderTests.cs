using NUnit.Framework;
using ReleaseStats;

[TestFixture, Explicit("Long running")]
public class GitHubProviderTests
{
    [Test]
    public void ReturnAllNServiceBusReleases()
    {
        var config = new RunnerConfiguration();

        config.AddProvider(new GitHubStatsProvider());

        using (var releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config))
        {
            var result = releaseStatsRunner.GenerateStatistics();

            Assert.Contains(new Release(new SemVer("4.6.3")), result.Releases);
        }
    }
}

