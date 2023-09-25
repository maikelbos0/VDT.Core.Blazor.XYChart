using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace VDT.Core.Blazor.XYChart.Tests;

public static class Path {
    private static Regex pathDecimalTrimmer = new("(\\.0+|(?<=\\.[1-9]+)(0+))(?= )", RegexOptions.Compiled);

    public static string TrimDecimals(string path) => pathDecimalTrimmer.Replace(path, "");

    public static string Create(params FormattableString[] commands) => TrimDecimals(string.Join(" ", commands.Select(FormattableString.Invariant)));
}
