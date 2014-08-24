namespace ReleaseStats
{
    using ReleaseStats.ReleaseProperties;

    public class ReleaseHierarchy:ReleaseProperty
    {
        public Release OriginalRelease { get; set; }
    }
}