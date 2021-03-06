﻿namespace ReleaseStats
{
    using System;

    public class SemVer : IEquatable<SemVer>,IComparable<SemVer>
    {
        public SemVer(int major,int minor,int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }
        public SemVer(string versionString)
        {
            var parts = versionString.Split('.');

            Major = int.Parse(parts[0]);
            Minor = int.Parse(parts[1]);
            Patch = int.Parse(parts[2]);
        }

        public int CompareTo(SemVer other)
        {
            if (Major < other.Major)
            {
                return -1;
            }
            if (Major > other.Major)
            {
                return 1;
            }

            if (Minor < other.Minor)
            {
                return -1;
            }
            if (Minor > other.Minor)
            {
                return 1;
            } 
            
            if (Patch < other.Patch)
            {
                return -1;
            }
            if (Patch > other.Patch)
            {
                return 1;
            }
            return 0;
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

        public static implicit operator SemVer(string version)
        {
            return new SemVer(version);
        }

        public bool IsMajorRelease
        {
            get { return Minor == 0 && Patch == 0; }
        }

        public bool IsMinorRelease
        {
            get { return Patch == 0 && Minor > 0; }
        }

        public bool IsPatchRelease
        {
            get { return Patch > 0; }
        }

        public bool IsPatchFor(SemVer version)
        {
            if (version.IsPatchRelease)
            {
                throw new Exception("Patches can't be patched: " + version);
            }

            if (!IsPatchRelease)
            {
                return false;
            }

            return Major == version.Major && Minor == version.Minor;
        }
    }
}