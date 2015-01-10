namespace ReleaseStats.PropertyEnrichers
{
    using ReleaseStats.ReleaseProperties;

    public class SupportReleaseEnricher : PropertyEnricher
    {
        public override void Process(ReleaseStatistics stats)
        {
            foreach (var release in stats.Releases)
            {
                release.Properties.Add(new SupportRelease());
            }
        }

    }
}