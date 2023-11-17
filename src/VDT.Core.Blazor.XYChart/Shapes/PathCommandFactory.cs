using System;

namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// Methods to create commands for SVG path elements
/// </summary>
public static class PathCommandFactory {
    /// <summary>
    /// Close the current path
    /// </summary>
    public const string ClosePath = "Z";

    /// <summary>
    /// Create a command to move to the provided absolute coordinates without drawing
    /// </summary>
    /// <param name="x">Absolute X-coordinate</param>
    /// <param name="y">Absolute Y-coordinate</param>
    /// <returns>The created move command</returns>
    public static string MoveTo(decimal x, decimal y) => FormattableString.Invariant($"M {x} {y}");

    /// <summary>
    /// Create a command to draw a line to the provided absolute coordinates
    /// </summary>
    /// <param name="x">Absolute X-coordinate</param>
    /// <param name="y">Absolute Y-coordinate</param>
    /// <returns>The created line command</returns>
    public static string LineTo(decimal x, decimal y) => FormattableString.Invariant($"L {x} {y}");
}
