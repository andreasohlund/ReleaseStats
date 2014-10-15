namespace ReleaseStats.Tests.Reports
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using NUnit.Framework;
    using ReleaseStats.Cleaners;
    using ReleaseStats.Providers.GitHub;
    using ReleaseStats.Providers.NuGet;
    using ReleaseStats.Validators;

    public class ReportFixture
    {
        protected ReleaseStatsRunner releaseStatsRunner;

        [SetUp]
        public void SetUp()
        {
            var config = new RunnerConfiguration();

            config.AddProviderValidator(new DuplicateVersionsValidator());

            config.AddCleaner(new ConsolidateDuplicateReleasesCleaner());

            config.AddStatsProvider(new NuGetStatsProvider());
            config.AddStatsProvider(new GitHubStatsProvider("Particular"));

            config.AddProjectProvider(new GitHubProjectProvider("Particular"));
            
            releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config);
        }


        public class Datapoint
        {
            public string Period;
            public int NumberOfReleases;
        }

        protected IEnumerable<Datapoint> GenerateAllMonths(string min, string max)
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

        protected static string RenderTemplate(string project, string data)
        {
            var target = String.Format(@".\reports\{0}-releases-over-time.html", project);
            var template = @"..\..\ReportTemplates\releases-over-time.html";

            var text = File.ReadAllText(template);
            text = text.Replace("{{data}}", data);
            File.WriteAllText(target, text);
            return target;
        }
    }
}