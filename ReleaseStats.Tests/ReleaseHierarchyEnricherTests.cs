namespace ReleaseStats.Tests
{
    using System.Linq;
    using NUnit.Framework;
    using ReleaseStats.PropertyEnrichers;

    [TestFixture]
    public class ReleaseHierarchyEnricherTests
    {
        [Test]
        public void MajorWithNoPatches()
        {
            var release = new Release("1.0.0");

            Enrich(release);

            Assert.AreEqual(release, release.Property<ReleaseHierarchy>().OriginalRelease);
        }

        [Test]
        public void MinorWithNoPatches()
        {
            var release = new Release("1.1.0");

            Enrich(release,"1.0.0");

            Assert.AreEqual(release, release.Property<ReleaseHierarchy>().OriginalRelease);
        }

        [Test]
        public void PatchToMajor()
        {
            var release = new Release("1.0.1");

            Enrich(release, "1.0.0");

            Assert.AreEqual(new Release("1.0.0"),release.Property<ReleaseHierarchy>().OriginalRelease);
        }

        [Test]
        public void PatchToMinor()
        {
            var release = new Release("1.1.1");

            Enrich(release, "1.1.0");

            Assert.AreEqual(new Release("1.1.0"), release.Property<ReleaseHierarchy>().OriginalRelease);
        }

        void Enrich(Release release, params string[] otherVersions)
        {
            var enricher = new ReleaseHierarchyEnricher();

            var stats = new ReleaseStatistics();
          
            stats.Releases.Add(release);
            stats.Releases.AddRange(otherVersions.Select(v=>new Release(v)));

            enricher.Process(stats);

        }
    }
}