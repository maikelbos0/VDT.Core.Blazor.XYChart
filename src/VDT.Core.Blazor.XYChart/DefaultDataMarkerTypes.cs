using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Default implementations of <see cref="DataMarkerDelegate"/>
/// </summary>
public static class DefaultDataMarkerTypes {
    /// <summary>
    /// Creates an SVG shape to display a marker in line data as a circle
    /// </summary>
    /// <param name="x">X-coordinate of the center point of the marker</param>
    /// <param name="y">Y-coordinate of the center point of the marker</param>
    /// <param name="size">Width/height of the marker</param>
    /// <param name="color">Data series color</param>
    /// <param name="cssClass">Data series CSS class</param>
    /// <param name="layerIndex">Index of the layer that contains the data series</param>
    /// <param name="dataSeriesIndex">Index of the data series in the containing layer</param>
    /// <param name="dataPointIndex">Index of the data point in the data series</param>
    /// <returns>An SVG round data marker shape</returns>
    public static ShapeBase Round(decimal x, decimal y, decimal size, string color, string? cssClass, int layerIndex, int dataSeriesIndex, int dataPointIndex)
        => new RoundDataMarkerShape(x, y, size, color, cssClass, layerIndex, dataSeriesIndex, dataPointIndex);

    /// <summary>
    /// Creates an SVG square data marker shape
    /// </summary>
    /// <param name="x">X-coordinate of the center point of the marker</param>
    /// <param name="y">Y-coordinate of the center point of the marker</param>
    /// <param name="size">Width/height of the marker</param>
    /// <param name="color">Data series color</param>
    /// <param name="cssClass">Data series CSS class</param>
    /// <param name="layerIndex">Index of the layer that contains the data series</param>
    /// <param name="dataSeriesIndex">Index of the data series in the containing layer</param>
    /// <param name="dataPointIndex">Index of the data point in the data series</param>
    /// <returns>An SVG square data marker shape</returns>
    public static ShapeBase Square(decimal x, decimal y, decimal size, string color, string? cssClass, int layerIndex, int dataSeriesIndex, int dataPointIndex)
        => new SquareDataMarkerShape(x, y, size, color, cssClass, layerIndex, dataSeriesIndex, dataPointIndex);
}
