namespace ReleaseStats.Tests.Reports
{
    using System.Diagnostics;
    using System.Linq;
    using NUnit.Framework;
    using ReleaseStats.ReleaseProperties;

    [TestFixture, Explicit("Long running")]
    public class NServiceBus_ReleasesPerMonth:ReportFixture
    {

        [Test]
        public void DisplayReport()
        {

            var project = "NServiceBus";

            var result = releaseStatsRunner.GenerateMultiple(project+"*");


            var reportData = result.SelectMany(p=>p.ReleaseStatistics.Releases).Select(r => new Datapoint
            {
                Period = r.Property<ReleaseDate>().ReleasedAt.ToString("yyyy-MM"),
                NumberOfReleases = 1
            }).ToList();

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
                title = "NServiceBus releases",
                data = reportData.ToArray()
            });

            var target = RenderTemplate(project, data);

            Process.Start(target);

        }

    }
}