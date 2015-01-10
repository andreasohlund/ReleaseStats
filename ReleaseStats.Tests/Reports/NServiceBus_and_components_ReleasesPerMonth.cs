namespace ReleaseStats.Tests.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using NUnit.Framework;
    using ReleaseStats.ReleaseProperties;

    [TestFixture, Explicit("Long running")]
    public class NServiceBus_and_components_ReleasesPerMonth : ReportFixture
    {

        [Test]
        public void DisplayReport()
        {
            var result = releaseStatsRunner.GenerateMultiple("NServiceBus*", "Service*")
                .ToList();

            var raw = result.SelectMany(r => r.AllReleasesForHierarchy)
                .Where(r => r.HasProperty<ReleaseDate>())
                .ToList();

            var start = raw.Min(r => r.Property<ReleaseDate>().ReleasedAt.Year);
            var end = raw.Max(r => r.Property<ReleaseDate>().ReleasedAt.Year);

            for (var year = start; year < end; year++)
            {
                GenerateForYear(raw, year);
            }
        }

        void GenerateForYear(IEnumerable<Release> releases, int year)
        {
            var reportData = releases.Where(r =>
                r.HasProperty<ReleaseDate>() &&
                r.Property<ReleaseDate>().ReleasedAt.Year == year
                && r.HasProperty<LinkToReleaseNotes>()).Select(r => new Datapoint
                {
                    Period = r.Property<ReleaseDate>().ReleasedAt.ToString("yyyy-MM"),
                    NumberOfReleases = 1
                }).ToList();

            Console.Out.WriteLine("Total for {0}: {1}",year,reportData.Count);

            reportData.AddRange(GenerateAllMonths(reportData.Min(r => r.Period), reportData.Max(r => r.Period)));


            reportData = reportData.GroupBy(r => r.Period)
                .OrderBy(g => g.Key)
                .Select(g => new Datapoint
                {
                    Period = g.Key,
                    NumberOfReleases = g.Sum(d => d.NumberOfReleases)
                }).ToList();


            var data = SimpleJson.SerializeObject(new
            {
                title = "Releases - "+year,
                data = reportData.ToArray()
            });

            var target = RenderTemplate(year.ToString(), data);

            Process.Start(target);
        }
    }
}