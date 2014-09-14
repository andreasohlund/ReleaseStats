namespace ReleaseStats
{
    using System.Collections.Generic;

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
        
        
        List<Project> subprojects = new List<Project>();

        public void AddSubprojects(IEnumerable<Project> projects)
        {
            subprojects.AddRange(projects);
        }
    }
}