namespace ReleaseStats
{
    using System.Collections.Generic;
    using System.Linq;

    public class ProjectHierarchyClassifier
    {
        public static IEnumerable<Project> Classify(IEnumerable<Project> projects)
        {
            var rootProjects = projects.Where(p => !p.Name.Contains("."))
                .OrderBy(p=>p.Name)
                .ToList();

            foreach (var project in rootProjects)
            {
                project.AddSubprojects(projects.Where(p => p.Name.StartsWith(project.Name + ".")).OrderBy(p=>p.Name));
            }

            return rootProjects;
        }
    }
}