namespace ReleaseStats
{
    using System;

    public class SemVer : IEquatable<SemVer>
    {
        public SemVer(string versionString)
        {
            var parts = versionString.Split('.');

            Major = int.Parse(parts[0]);
            Minor = int.Parse(parts[1]);
            Patch = int.Parse(parts[2]);
        }

        public override string ToString()
        {
            return string.Format("{0}.{1}.{2}", Major, Minor, Patch);
        }

        public bool Equals(SemVer other)
        {
            if (other == null)
            {
                return false;
            }

            return other.Major == Major && other.Minor == Minor && other.Patch == Patch;
        }

        public int Major { get; private set; }
        public int Minor { get; private set; }

        public int Patch { get; private set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((SemVer) obj);
        }

        public override int GetHashCode()
        {
            return int.Parse(string.Format("{0}{1}{2}", Major, Minor, Patch));
        }

        public static bool operator ==(SemVer left, SemVer right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SemVer left, SemVer right)
        {
            return !Equals(left, right);
        }


    }
}