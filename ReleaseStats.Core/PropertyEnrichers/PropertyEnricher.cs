namespace ReleaseStats.PropertyEnrichers
{
    public abstract class PropertyEnricher
    {
        public abstract void Process(ReleaseStatistics stats);
    }
}