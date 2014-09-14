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
            var releases = new List<Release>();
            foreach (var provider in runnerConfiguration.statsProviders)
            {
                var providerResult = provider.FetchStats(project).ToList();

                foreach (var cleaner in runnerConfiguration.releaseCleaners)
                {
                    providerResult = cleaner.Clean(providerResult).ToList();
                }

                var validationErrors = runnerConfiguration.providerValidators.SelectMany(v=>v.Validate(providerResult)).ToList();

                if (validationErrors.Any())
                {
                    throw new Exception("Validation errors found for provider: " + provider.GetType().Name + Environment.NewLine + string.Join(Environment.NewLine,validationErrors));
                }
                releases.AddRange(providerResult);
            }

            var result = new ReleaseStatistics(new Project(project));
            
            result.Releases.AddRange(releases.OrderByDescending(r => r.Version));
          
            runnerConfiguration.PropertyEnrichers.ForEach(e => e.Process(result));

            return result;
        }

        public IEnumerable<Project> GenerateMultiple(string filter)
        {
            var projects = new List<Project>();

            foreach (var provider in runnerConfiguration.projectProviders)
            {
                projects.AddRange(provider.FindMatching(filter));    
            }


            //arrange the projects into sub projects
            return ProjectHierarchyClassifier.Classify(projects);
        }
    }
}