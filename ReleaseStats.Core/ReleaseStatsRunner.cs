namespace ReleaseStats
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ReleaseStatsRunner : IDisposable
    {
        readonly RunnerConfiguration runnerConfiguration;

        internal ReleaseStatsRunner(RunnerConfiguration runnerConfiguration)
        {
            this.runnerConfiguration = runnerConfiguration;
        }

        public void Dispose()
        {

        }

        public ReleaseStatistics GenerateStatistics(string project)
        {
            return GenerateStatistics(new Project(project));
        }
        ReleaseStatistics GenerateStatistics(Project project)
        {
            var releases = new List<Release>();
            foreach (var provider in runnerConfiguration.statsProviders)
            {
                var providerResult = provider.FetchStats(project).ToList();

                providerResult.ForEach(r => r.Properties.Add(new BelongsToProject(project)));


           
                releases.AddRange(providerResult);
            }

            foreach (var cleaner in runnerConfiguration.releaseCleaners)
            {
                releases = cleaner.Clean(releases).ToList();
            }

            var validationErrors = runnerConfiguration.providerValidators.SelectMany(v => v.Validate(releases)).ToList();

            if (validationErrors.Any())
            {
                throw new Exception("Validation errors: " + Environment.NewLine + string.Join(Environment.NewLine, validationErrors));
            }
             

            var result = new ReleaseStatistics(project);
            
            result.Releases.AddRange(releases.OrderByDescending(r => r.Version));
          
            runnerConfiguration.PropertyEnrichers.ForEach(e => e.Process(result));



            return result;
        }

        public IEnumerable<Project> GenerateMultiple(params string[] filters)
        {
            var projects = new List<Project>();

            foreach (var filter in filters)
            {
                foreach (var provider in runnerConfiguration.projectProviders)
                {
                    projects.AddRange(provider.FindMatching(filter));
                }                
            }

            foreach (var project in projects)
            {
                Console.Out.WriteLine("Processing stats for: " + project.Name);
                project.ReleaseStatistics = GenerateStatistics(project);
            }

            //arrange the projects into sub projects
            return ProjectHierarchyClassifier.Classify(projects);
        }
    }
}