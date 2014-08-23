namespace ReleaseStats
{
    using System.Collections.Generic;

    public class ReleaseStatistics
    {
        public ReleaseStatistics()
        {
            Releases=new List<Release>();
        }

        public List<Release> Releases { get; private set; }
    }
}