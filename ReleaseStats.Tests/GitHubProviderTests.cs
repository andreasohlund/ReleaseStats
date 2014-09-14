using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ReleaseStats;
using ReleaseStats.Providers.GitHub;

[TestFixture, Explicit("Long running")]
public class GitHubProviderTests
{
    [Test]
    public void ReturnAllNServiceBusReleases()
    {
        var config = RunnerConfiguration.Default;

        config.AddStatsProvider(new GitHubStatsProvider("Particular"));
     
        using (var releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config))
        {
            var result = releaseStatsRunner.GenerateStatistics("NServiceBus");

            Assert.Contains(new Release("4.6.3"), result.Releases);

            result.Releases.Where(r=>!r.Version.IsPatchRelease)
                .ToList()
                .ForEach(ConsoleFormatter.PrintRelease);
        }
    }

    [Test]
    public void ListProjectsAndSubProjects()
    {
        var config = RunnerConfiguration.Default;

        config.AddStatsProvider(new GitHubStatsProvider("Particular"));
        config.AddProjectProvider(new GitHubProjectProvider("Particular"));

        using (var releaseStatsRunner = ReleaseStatsFactory.CreateRunner(config))
        {
            var result = releaseStatsRunner.GenerateMultiple("NServiceBus*");

            var project = result.Single(p=>p.Name == "NServiceBus");

            Assert.AreEqual("NServiceBus", project.Name);

            Assert.Contains("NServiceBus.RabbitMQ", project.Subprojects.Select(p=>p.Name).ToList());

            PrintProjects(result);
        }
    }


    void PrintProjects(IEnumerable<Project> projects)
    {
        foreach (var project in projects)
        {
            Console.Out.WriteLine("{0} - ({1} subprojects)",project.Name,project.Subprojects.Count());

            foreach (var subproject in project.Subprojects)
            {
                Console.Out.WriteLine("\t{0}", subproject.Name);
            }
            Console.Out.WriteLine(Environment.NewLine);
        }
    }
}

