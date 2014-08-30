namespace ReleaseStats.PropertyEnrichers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ReleaseHierarchyEnricher:PropertyEnricher
    {
        public override void Process(ReleaseStatistics stats)
        {
            foreach (var release in stats.Releases)
            {
                var hierarchy = CreateReleaseHierarchy(release, stats.Releases);

                release.Properties.Add(hierarchy);
            }   
        }

        ReleaseHierarchy CreateReleaseHierarchy(Release release, IEnumerable<Release> releases)
        {
            if (release.Version.IsMajorRelease || release.Version.IsMinorRelease)
            {
                var patches = releases.Where(r => r.Version.IsPatchFor(release.Version))
                    .OrderBy(r=>r.Version)                    
                    .ToList();

                return new ReleaseHierarchy(release,patches);
            }

            var potentialOriginalRelease = releases.SingleOrDefault(original =>
                original.Version.Major == release.Version.Major &&
                original.Version.Minor == release.Version.Minor &&
                !original.Version.IsPatchRelease);

            if (potentialOriginalRelease != null)
            {
                return new ReleaseHierarchy(potentialOriginalRelease);
            }
            
            throw new Exception("No original release could be found for: " + release);
        }
    }
}