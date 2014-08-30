using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ReleaseStats;
using ReleaseStats.Cleaners;
using ReleaseStats.ReleaseProperties;

[TestFixture]
public class ConsolidateDuplicateReleasesCleanerTests
{
    [Test]
    public void ShouldConsolidateDuplicatesBySelectingTheFirstOne()
    {
        var releases = new List<Release>
        {
            CreateRelease("1.0.0","2014-01-01"),
            CreateRelease("1.0.1","2014-02-01"),
            CreateRelease("1.0.1","2014-03-01"),
            CreateRelease("1.0.2","2014-04-01"),
            CreateRelease("1.1.1","2014-05-01"),
            CreateRelease("1.1.1","2014-06-01")
        };


        var validator = new ConsolidateDuplicateReleasesCleaner();

        var result = validator.Clean(releases);


        Assert.NotNull(result.Single(r => r.Version == "1.0.0"));
        Assert.AreEqual(result.Single(r => r.Version == "1.0.1").Property<ReleaseDate>().ReleasedAt,DateTimeOffset.Parse("2014-02-01"));
        Assert.NotNull(result.Single(r => r.Version == "1.0.2"));
        Assert.NotNull(result.Single(r => r.Version == "1.1.1"));

    }

    Release CreateRelease(string version, string releasedAt)
    {
        var release = new Release(version);

        release.Properties.Add(new ReleaseDate(DateTimeOffset.Parse(releasedAt)));

        return release;
    }
}