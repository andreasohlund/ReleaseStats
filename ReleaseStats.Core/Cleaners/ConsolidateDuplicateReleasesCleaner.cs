using System.Collections.Generic;

namespace ReleaseStats.Cleaners
{
    using System;
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
                  var consolidated = toBeConsolidated
                      .OrderBy(r =>r.HasProperty<ReleaseDate>() ? r.Property<ReleaseDate>().ReleasedAt : DateTimeOffset.MinValue)
                      .First();

                foreach (var property in toBeConsolidated.SelectMany(r=>r.Properties))
                {
                    if (!consolidated.HasProperty(property.GetType()))
                    {
                        consolidated.Properties.Add(property);
                    }
                }
                alreadyOkOnes.Add(consolidated);
            }
            

            return alreadyOkOnes;
        }
    } 
}