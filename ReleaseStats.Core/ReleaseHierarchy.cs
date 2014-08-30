namespace ReleaseStats
{
    using System.Collections.Generic;
    using ReleaseStats.ReleaseProperties;

    public class ReleaseHierarchy:ReleaseProperty
    {
        public ReleaseHierarchy(Release originalRelease):this(originalRelease,new List<Release>())
        {
        }

        public ReleaseHierarchy(Release originalRelease,IEnumerable<Release> patches)
        {
            OriginalRelease = originalRelease;
            Patches = patches;
        }
        public Release OriginalRelease { get; private set; }
        public IEnumerable<Release> Patches { get; private set; }
    }
}