namespace ReleaseStats
{
    public class ValidationError
    {
        public SemVer Version { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Version, Message);
        }
    }
}