using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Signature for methods that return SVG data marker shapes for use in a <see cref="LineLayer"/>
/// </summary>
/// <param name="x">X-coordinate of the center point of the marker</param>
/// <param name="y">Y-coordinate of the center point of the marker</param>
/// <param name="size">Width/height of the marker</param>
/// <param name="color">Data series color</param>
/// <param name="cssClass">Data series CSS class</param>
/// <param name="layerIndex">Index of the layer that contains the data series</param>
/// <param name="dataSeriesIndex">Index of the data series in the containing layer</param>
/// <param name="dataPointIndex">Index of the data point in the data series</param>
public delegate ShapeBase DataMarkerDelegate(decimal x, decimal y, decimal size, string color, string? cssClass, int layerIndex, int dataSeriesIndex, int dataPointIndex);
