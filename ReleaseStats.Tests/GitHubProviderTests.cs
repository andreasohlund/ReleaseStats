using System.Linq;
using NUnit.Framework;
using ReleaseStats;
using ReleaseStats.Providers.GitHub;

[TestFixture, Explicit("Long running")]
public class GitHubProviderTests
{
    [Test]
    public void ReturnAllNServiceBusReleases()
    {
        var config = RunnerConfiguration.Default;

        config.AddProvider(new GitHubStatsProvider("Particular"));
     
        using (var releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config))
        {
            var result = releaseStatsRunner.GenerateStatistics("NServiceBus");

            Assert.Contains(new Release("4.6.3"), result.Releases);

            result.Releases.Where(r=>!r.Version.IsPatchRelease)
                .ToList()
                .ForEach(ConsoleFormatter.PrintRelease);
        }
    }
}

