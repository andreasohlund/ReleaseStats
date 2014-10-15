namespace ReleaseStats.Tests.Reports
{
    using System;
    using System.Linq;
    using NUnit.Framework;
    using ReleaseStats.ReleaseProperties;

    [TestFixture, Explicit("Long running")]
    public class NServiceBus_and_components_ReleasesLastMonth : ReportFixture
    {

        [Test]
        public void DisplayReport()
        {

            var project = "NServiceBus";

            var result = releaseStatsRunner.GenerateMultiple(project+"*");


            var reportData = result.First().AllReleasesForHierarchy.Where(r=>r.HasProperty<ReleaseDate>() && r.Property<ReleaseDate>().ReleasedAt >= DateTime.UtcNow.AddMonths(-1)).ToList();


            Console.Out.WriteLine("-------------- NSB + Subcomponent releases last month -------------------------------");
            foreach (var release in reportData)
            {
                Console.Out.WriteLine(release);
            }

        }

    }
}