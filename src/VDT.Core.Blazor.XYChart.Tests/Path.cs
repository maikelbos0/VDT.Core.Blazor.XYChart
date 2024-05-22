using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace VDT.Core.Blazor.XYChart.Tests;

public static partial class Path {
    private readonly static Regex pathDecimalTrimmer = CreatePathDecimalTrimmer();

    public static string TrimDecimals(string path) => pathDecimalTrimmer.Replace(path, "");

    public static string Create(params FormattableString[] commands) => TrimDecimals(string.Join(" ", commands.Select(FormattableString.Invariant)));

    [GeneratedRegex("(\\.0+|(?<=\\.[1-9]+)(0+))(?=[ ,]|$)")]
    private static partial Regex CreatePathDecimalTrimmer();
}
