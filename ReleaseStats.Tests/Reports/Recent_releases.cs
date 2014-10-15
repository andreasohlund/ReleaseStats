namespace ReleaseStats.Tests.Reports
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using ReleaseStats.ReleaseProperties;

    [TestFixture, Explicit("Long running")]
    public class Recent_releases : ReportFixture
    {

        [Test]
        public void DisplayReport()
        {

            var result = releaseStatsRunner.GenerateMultiple("NServiceBus*","Service*");

            var reportData = result.SelectMany(p=>p.AllReleasesForHierarchy).ToList();


            Console.Out.WriteLine("---------------------- Releases last 7 days --------------------------------");    
            
            foreach (var release in reportData.Where(r=>r.HasProperty<ReleaseDate>()).OrderByDescending(r=>r.Property<ReleaseDate>().ReleasedAt))
            {
                var releaseDate = release.Property<ReleaseDate>().ReleasedAt;

                if ((DateTime.Now - releaseDate) < TimeSpan.FromDays(7))
                {

                    var releaseNotes = "not found";

                    if (release.HasProperty<LinkToReleaseNotes>())
                    {
                        releaseNotes = release.Property<LinkToReleaseNotes>().Url;
                    }

                    Console.Out.WriteLine("{0} - {1} ({2})",release.Property<BelongsToProject>().Project.Name,release.Version,releaseNotes);    
                }
                
            }

          
        }

    }
}