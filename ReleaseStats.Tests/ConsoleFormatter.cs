using System;
using System.Linq;
using System.Text;
using ReleaseStats;
using ReleaseStats.ReleaseProperties;

class ConsoleFormatter
{
    public static void PrintRelease(IGrouping<Release, Release> grouping)
    {
        var sb = new StringBuilder();

        sb.Append(grouping.Key.Version);

        if (grouping.Count() > 1)
        {
            sb.AppendFormat("({0})", String.Join("|", grouping.Where(r => r != grouping.Key).Select(patch => patch.Version)));
        }

        sb.AppendFormat(" - {0}", grouping.Key.Property<ReleaseDate>());

        Console.Out.WriteLine(sb);
    }
}