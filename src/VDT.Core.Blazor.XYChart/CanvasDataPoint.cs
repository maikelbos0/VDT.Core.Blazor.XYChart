namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Information for displaying a data point on a canvas
/// </summary>
/// <param name="X">X-coordinate of the data point</param>
/// <param name="Y">Y-coordinate of the data point</param>
/// <param name="Height">Data point value converted to a canvas value</param>
/// <param name="Width">Width of the data point; normally zero</param>
/// <param name="Index">Index of the data point in the data series</param>
/// <param name="Value">Data point value</param>
public record class CanvasDataPoint(decimal X, decimal Y, decimal Height, decimal Width, int Index, decimal Value);
