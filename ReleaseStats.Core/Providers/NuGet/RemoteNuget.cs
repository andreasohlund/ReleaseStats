namespace ReleaseStats.Providers.NuGet
{
    using System;

    class RemoteNuget
    {
        public readonly string Id;
        public readonly string Name;
        public readonly string Version;

        public readonly DateTime PublishedAt;

       
        public RemoteNuget(string id, string name, string version, DateTime publishedAt)
        {
            Id = id;
            Name = name;
            Version = version;
            PublishedAt = publishedAt;
        }
    }
}