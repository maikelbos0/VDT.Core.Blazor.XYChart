using System.Collections.Generic;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Information for displaying data points in a data series on a canvas
/// </summary>
/// <param name="Color">Data series color</param>
/// <param name="CssClass">Data series CSS class</param>
/// <param name="Index">Index of the data series in its containing layer</param>
/// <param name="DataPoints">Data points in the data series</param>
public record class CanvasDataSeries(string Color, string? CssClass, int Index, IList<CanvasDataPoint> DataPoints);
