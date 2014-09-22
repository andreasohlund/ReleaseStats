namespace ReleaseStats
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ReleaseStats.ReleaseProperties;

    public class Release : IEquatable<Release>
    {
        public bool Equals(Release other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Equals(Version, other.Version);
        }

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
            return Equals((Release) obj);
        }

        public override int GetHashCode()
        {
            return (Version != null ? Version.GetHashCode() : 0);
        }

        public static bool operator ==(Release left, Release right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Release left, Release right)
        {
            return !Equals(left, right);
        }

        public SemVer Version { get; private set; }
        public List<ReleaseProperty> Properties { get; private set; }

        public Release(string versionString):this(new SemVer(versionString))
        {
            
        }
        public Release(SemVer version)
        {
            Version = version;
            Properties = new List<ReleaseProperty>();
        }

        public T Property<T>() where T:ReleaseProperty
        {
            return (T)Properties.Single(p => p.GetType() == typeof(T));
        }

        public override string ToString()
        {
            return Version.ToString();
        }

        public bool HasProperty<T>()
        {
            return Properties.Any(p => p.GetType() == typeof(T));   
        }
    }
}