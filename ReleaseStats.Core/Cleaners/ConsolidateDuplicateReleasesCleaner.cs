using System.Collections.Generic;

namespace ReleaseStats.Cleaners
{
    using System.Linq;
    using ReleaseStats.ReleaseProperties;

    public class ConsolidateDuplicateReleasesCleaner : IReleaseCleaner
    {
        public IEnumerable<Release> Clean(IEnumerable<Release> releases)
        {
            var gropedByVersion = releases.GroupBy(r => r.Version);

            var alreadyOkOnes = gropedByVersion.Where(g => g.Count() == 1).Select(g => g.First())
                .ToList();


            foreach (var toBeConsolidated in gropedByVersion.Where(g => g.Count() > 1))
            {
                var findLastReleaseDate = toBeConsolidated.Where(r => r.HasProperty<ReleaseDate>())
                    .OrderBy(r => r.Property<ReleaseDate>().ReleasedAt)
                    .FirstOrDefault();

                if (findLastReleaseDate!=null)
                {
                    alreadyOkOnes.Add(findLastReleaseDate);
                    continue;
                }

                alreadyOkOnes.Add(toBeConsolidated.Last());
            }
            

            return alreadyOkOnes;
        }
    } 
}