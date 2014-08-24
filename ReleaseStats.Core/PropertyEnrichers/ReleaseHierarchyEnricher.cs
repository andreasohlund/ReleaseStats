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
                var hierarchy = new ReleaseHierarchy();
                Release originalRelease;

                if (TryFindOriginalRelease(release, stats.Releases, out originalRelease))
                {
                    hierarchy.OriginalRelease = originalRelease;
                }

                release.Properties.Add(hierarchy);
            }   
        }

        bool  TryFindOriginalRelease(Release release, IEnumerable<Release> releases,out Release originalRelease)
        {
            if (release.Version.IsMajorRelease || release.Version.IsMinorRelease)
            {
                originalRelease = release;
                return true;
            }

            var potentialOriginalRelease = releases.SingleOrDefault(original =>
                original.Version.Major == release.Version.Major &&
                original.Version.Minor == release.Version.Minor &&
                !original.Version.IsPatchRelease);

            if (potentialOriginalRelease != null)
            {
                originalRelease = potentialOriginalRelease;
                return true;            
            }
            else
            {
                throw new Exception("No original release could be found for: " + release);
            }
        }
    }
}