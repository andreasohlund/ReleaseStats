namespace ReleaseStats.Tests
{
    using NUnit.Framework;
    using ReleaseStats.PropertyEnrichers;
    using ReleaseStats.ReleaseProperties;

    [TestFixture]
    public class SupportReleaseEnricherTests
    {
        [Test]
        public void OrderPatchReleasesBySemver()
        {
            var first = new Release("1.0.0");
            var patchToFirst = new Release("1.0.1");
            var minor = new Release("1.1.0");
            var major = new Release("2.0.0");

            var enricher = new ReleaseHierarchyEnricher();

            var stats = new ReleaseStatistics(new Project("test"));

            stats.Releases.Add(release);
            stats.Releases.Add(release);
            stats.Releases.AddRange(otherVersions.Select(v => new Release(v)));

            enricher.Process(stats);

            Enrich(release, "1.0.2", "2.0.1", "2.1.4", "3.0.3");

            Assert.True(release.HasProperty<SupportRelease>());
        }

    }
}