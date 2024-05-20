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

    /// <summary>
    /// Create a command to draw a quadratic bezier curve via a control point to the provided absolute coordinates
    /// </summary>
    /// <param name="x1">Absolute X-coordinate of the control point</param>
    /// <param name="y1">Absolute Y-coordinate of the control point</param>
    /// <param name="x">Absolute X-coordinate</param>
    /// <param name="y">Absolute Y-coordinate</param>
    /// <returns>The created line command</returns>
    public static string QuadraticBezierTo(decimal x1, decimal y1, decimal x, decimal y) => FormattableString.Invariant($"Q {x1} {y1}, {x} {y}");

    /// <summary>
    /// Create a command to draw a cubic bezier curve via two control points to the provided absolute coordinates
    /// </summary>
    /// <param name="x1">Absolute X-coordinate of the first control point</param>
    /// <param name="y1">Absolute Y-coordinate of the first control point</param>
    /// <param name="x2">Absolute X-coordinate of the second control point</param>
    /// <param name="y2">Absolute Y-coordinate of the second control point</param>
    /// <param name="x">Absolute X-coordinate</param>
    /// <param name="y">Absolute Y-coordinate</param>
    /// <returns>The created line command</returns>
    public static string CubicBezierTo(decimal x1, decimal y1, decimal x2, decimal y2, decimal x, decimal y) => FormattableString.Invariant($"C {x1} {y1}, {x2} {y2}, {x} {y}");
}
