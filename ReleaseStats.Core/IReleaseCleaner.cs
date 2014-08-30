namespace ReleaseStats
{
    using System.Collections.Generic;

    public interface IReleaseCleaner
    {
        IEnumerable<Release> Clean(IEnumerable<Release> releases);
    }
}