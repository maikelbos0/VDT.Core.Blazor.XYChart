using System;

namespace VDT.Core.Blazor.XYChart.Shapes;

public static class PathCommandFactory {
    public static string MoveTo(decimal x, decimal y) => FormattableString.Invariant($"M {x} {y}");

    public static string LineTo(decimal x, decimal y) => FormattableString.Invariant($"L {x} {y}");
}
