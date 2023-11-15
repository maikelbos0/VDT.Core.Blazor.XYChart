namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Information for displaying a legend item on a canvas
/// </summary>
/// <param name="Color">Data series color</param>
/// <param name="Text">Data series name</param>
/// <param name="CssClass">Data series CSS class</param>
/// <param name="LayerIndex">Index of the layer that contains the data series</param>
/// <param name="DataSeriesIndex">Index of the data series in the containing layer</param>
public record class LegendItem(string Color, string Text, string? CssClass, int LayerIndex, int DataSeriesIndex);
