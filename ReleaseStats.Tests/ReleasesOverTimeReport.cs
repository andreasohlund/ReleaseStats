using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ReleaseStats;
using ReleaseStats.Cleaners;
using ReleaseStats.Providers.NuGet;
using ReleaseStats.ReleaseProperties;
using ReleaseStats.Tests;

[TestFixture, Explicit("Long running")]
public class ReleasesOverTimeReport
{
    [Test]
    public void DisplayReport()
    {

        var project = "NServiceBus";


        var config = RunnerConfiguration.Default;

        config.AddProvider(new NuGetStatsProvider());
        config.AddCleaner(new ConsolidateDuplicateReleasesCleaner());

        var releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config);

        var result = releaseStatsRunner.GenerateStatistics(project);


        var reportData = result.Releases.Select(r => new
        {
            Period = r.Property<ReleaseDate>().ReleasedAt.ToString("yyyy-M"),
            NumberOfReleases = 1
        }).GroupBy(r => r.Period)
        .OrderBy(g=>g.Key)
            .Select(g => new
            {
                Period = g.Key,
                NumberOfReleases = g.Count()
            }).ToArray();

        var data = SimpleJson.SerializeObject(new
        {
            title = "NServiceBus releases",
            data = reportData
        });

        var target = RenderTemplate(project, data);

        Process.Start(target);

    }

    static string RenderTemplate(string project, string data)
    {
        var target = string.Format(@".\reports\{0}-releases-over-time.html", project);
        var template = @"..\..\ReportTemplates\releases-over-time.html";

        var text = File.ReadAllText(template);
        text = text.Replace("{{data}}", data);
        File.WriteAllText(target, text);
        return target;
    }
}