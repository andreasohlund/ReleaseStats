namespace ReleaseStats
{
    using ReleaseStats.ReleaseProperties;

    public class BelongsToProject : ReleaseProperty
    {
        public Project Project { get; private set; }

        public BelongsToProject(Project project)
        {
            Project = project;
        }
    }
}