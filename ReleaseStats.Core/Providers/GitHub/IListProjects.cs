namespace ReleaseStats
{
    using System.Collections.Generic;
    using System.Linq;

    public interface IListProjects
    {
        IEnumerable<Project> FindMatching(string filter);
    }

    public class Project
    {
        public Project(string project)
        {
            Name = project;
        }

        public ReleaseStatistics ReleaseStatistics { get; set; }

        public string Name { get; set; }
        public IEnumerable<Project> Subprojects { get { return subprojects; }}

        public static implicit operator Project(string project)
        {
            return new Project(project);
        }

        public IEnumerable<Release> AllReleasesForHierarchy
        {
            get
            {
                var result = ReleaseStatistics.Releases.ToList();
                
                result.AddRange(Subprojects.SelectMany(sub=>sub.ReleaseStatistics.Releases));

                return result;
            }
        }

        List<Project> subprojects = new List<Project>();

        public void AddSubprojects(IEnumerable<Project> projects)
        {
            subprojects.AddRange(projects);
        }
    }
}