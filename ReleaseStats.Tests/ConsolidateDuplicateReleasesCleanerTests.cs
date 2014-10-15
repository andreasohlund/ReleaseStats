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
    public void ShouldConsolidateDuplicatesBySelectingTheOldestOne()
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
        Assert.AreEqual(result.Single(r => r.Version == "1.0.1").Property<ReleaseDate>().ReleasedAt, DateTimeOffset.Parse("2014-02-01"));
        Assert.NotNull(result.Single(r => r.Version == "1.0.2"));
        Assert.NotNull(result.Single(r => r.Version == "1.1.1"));

    }

    [Test]
    public void ShouldHandleMissingReleaseDateForDuplicate()
    {
        var releases = new List<Release>
        {
            CreateRelease("1.0.0","2014-01-01"),
                new Release("1.0.0")
        };


        var validator = new ConsolidateDuplicateReleasesCleaner();

        var result = validator.Clean(releases);


        Assert.AreEqual(result.Single(r => r.Version == "1.0.0").Property<ReleaseDate>().ReleasedAt, DateTimeOffset.Parse("2014-01-01"));

    }
    [Test]
    public void ShouldHandleMissingReleaseDateForBoth()
    {
        var releases = new List<Release>
        {
                new Release("1.0.0"),new Release("1.0.0")
        };


        var validator = new ConsolidateDuplicateReleasesCleaner();

        var result = validator.Clean(releases);


        Assert.NotNull(result.Single(r => r.Version == "1.0.0"));

    }

    [Test]
    public void ShouldMergeProperties()
    {
        var duplicate1 = new Release("1.0.0");

        duplicate1.Properties.Add(new LinkToReleaseNotes("fdsfsd"));
        duplicate1.Properties.Add(new ReleaseDate(DateTimeOffset.Now));

        var duplicate2 = new Release("1.0.0");

        duplicate2.Properties.Add(new BelongsToProject(new Project("test")));
        
        var releases = new List<Release>
        {
               duplicate1,duplicate2
        };


        var validator = new ConsolidateDuplicateReleasesCleaner();

        var result = validator.Clean(releases).Single(r => r.Version == "1.0.0");


        Assert.True(result.HasProperty<LinkToReleaseNotes>());
        Assert.True(result.HasProperty<BelongsToProject>());

    }

    Release CreateRelease(string version, string releasedAt)
    {
        var release = new Release(version);

        release.Properties.Add(new ReleaseDate(DateTimeOffset.Parse(releasedAt)));

        return release;
    }
}