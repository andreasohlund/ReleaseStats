namespace ReleaseStats.ReleaseProperties
{
    using System;

    public class ReleaseDate : ReleaseProperty
    {
        public DateTimeOffset ReleasedAt { get; private set; }

        public ReleaseDate(DateTimeOffset releasedAt)
        {
            ReleasedAt = releasedAt;
        }


        public override string ToString()
        {
            return "Release date: " + ReleasedAt;
        }
    }
}