using ReleaseStats.ReleaseProperties;

public class LinkToReleaseNotes : ReleaseProperty
{
    public string Url { get; private set; }

    public LinkToReleaseNotes(string url)
    {
        Url = url;
    }
}