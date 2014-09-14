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

        public string Name { get; set; }
    }
}