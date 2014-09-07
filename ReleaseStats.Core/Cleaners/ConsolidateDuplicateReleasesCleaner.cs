using System.Collections.Generic;

namespace ReleaseStats.Cleaners
{
    using System.Linq;
    using ReleaseStats.ReleaseProperties;

    public class ConsolidateDuplicateReleasesCleaner : IReleaseCleaner
    {
        public IEnumerable<Release> Clean(IEnumerable<Release> releases)
        {
            return releases.GroupBy(r => r.Version)
                .Select(g => g.Where(r => r.Property<ReleaseDate>().ReleasedAt.Year>1950)
                    .OrderBy(r => r.Property<ReleaseDate>().ReleasedAt).First())
                .ToList();
        }
    } 
}