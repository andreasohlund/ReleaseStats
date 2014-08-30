using System;
using System.Linq;
using System.Text;
using ReleaseStats;
using ReleaseStats.ReleaseProperties;

class ConsoleFormatter
{
    public static void PrintRelease(Release release)
    {
        var sb = new StringBuilder();

        sb.Append(release.Version);

        var hierarchy = release.Property<ReleaseHierarchy>();

        if (hierarchy.Patches.Any())
        {
            sb.AppendFormat("({0})", String.Join("|", hierarchy.Patches.Select(r=>r.Version)));
        }

        sb.AppendFormat(" - {0}", release.Property<ReleaseDate>());

        Console.Out.WriteLine(sb);
    }
}