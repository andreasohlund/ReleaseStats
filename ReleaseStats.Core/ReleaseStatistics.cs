namespace ReleaseStats
{
    using System.Collections.Generic;

    public class ReleaseStatistics
    {
        public ReleaseStatistics(Project project)
        {
            Project = project;
            Releases=new List<Release>();
        }

        public List<Release> Releases { get; private set; }
        public Project Project { get; private set; }
    }
}