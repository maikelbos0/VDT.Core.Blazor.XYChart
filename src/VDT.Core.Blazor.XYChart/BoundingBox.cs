using System;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Represents the smallest rectangle in which an SVG object fits
/// </summary>
/// <param name="X">X-coordinate of the bounding box</param>
/// <param name="Y">Y-coordinate of the bounding box</param>
/// <param name="Width">Width of the bounding box</param>
/// <param name="Height">Height of the bounding box</param>
public record BoundingBox(decimal X, decimal Y, decimal Width, decimal Height) {
    /// <summary>
    /// Required width to make the element fit inside an area, taking the offset of the X-coordinate into account
    /// </summary>
    public int RequiredWidth => (int)Math.Ceiling(Math.Abs(X) > Width ? Math.Abs(X) : Width + Math.Abs(X));

    /// <summary>
    /// Required height to make the element fit inside an area, taking the offset of the Y-coordinate into account
    /// </summary>
    public int RequiredHeight => (int)Math.Ceiling(Math.Abs(Y) > Height ? Math.Abs(Y) : Height + Math.Abs(Y));
}
