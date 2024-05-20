namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Control points for a data point on a canvas
/// </summary>
/// <param name="LeftX">X-coordinate of the left control point</param>
/// <param name="LeftY">Y-coordinate of the left control point</param>
/// <param name="RightX">X-coordinate of the right control point</param>
/// <param name="RightY">Y-coordinate of the right control point</param>
public record class ControlPoints(decimal LeftX, decimal LeftY, decimal RightX, decimal RightY);
