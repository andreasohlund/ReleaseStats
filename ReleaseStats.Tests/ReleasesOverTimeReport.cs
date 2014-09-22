using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;
using ReleaseStats;
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

        config.AddStatsProvider(new NuGetStatsProvider());
    
        var releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config);

        var result = releaseStatsRunner.GenerateStatistics(project);


        var reportData = result.Releases.Select(r => new Datapoint
        {
            Period = r.Property<ReleaseDate>().ReleasedAt.ToString("yyyy-MM"),
            NumberOfReleases = 1
        }).ToList();

        reportData.AddRange(GenerateAllMonths(reportData.Min(r=>r.Period),reportData.Max(r=>r.Period)));
        
        reportData = reportData.GroupBy(r => r.Period)
        .OrderBy(g=>g.Key)
            .Select(g => new Datapoint
            {
                Period = g.Key,
                NumberOfReleases = g.Sum(d=>d.NumberOfReleases)
            }).ToList();

     
        var data = SimpleJson.SerializeObject(new 
        {
            title = "NServiceBus releases",
            data = reportData.ToArray()
        });

        var target = RenderTemplate(project, data);

        Process.Start(target);

    }

    class Datapoint
    {
        public string Period;
        public int NumberOfReleases;
    }

    IEnumerable<Datapoint> GenerateAllMonths(string min, string max)
    {
        var start = DateTimeOffset.Parse(min);
        var end = DateTimeOffset.Parse(max);

        var current = start;
        do
        {
            yield return new Datapoint
            {
                Period = current.ToString("yyyy-MM"),
                NumberOfReleases = 0
            };

            current = current.AddMonths(1);

        } while (current <= end);
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