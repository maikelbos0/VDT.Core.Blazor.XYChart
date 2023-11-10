namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// SVG shape to display a marker in line data as a square
/// </summary>
public class SquareDataMarkerShape : ShapeBase {
    /// <summary>
    /// Default CSS classes that are always applied to this SVG shape
    /// </summary>
    public const string DefaultCssClass = "data-marker data-marker-square";

    /// <inheritdoc/>
    public override string ElementName => "rect";

    /// <summary>
    /// Gets the X coordinate of the center point of the marker
    /// </summary>
    public decimal X { get; }

    /// <summary>
    /// Gets the Y coordinate of the center point of the marker
    /// </summary>
    public decimal Y { get; }

    /// <summary>
    /// Gets the width/height of the marker
    /// </summary>
    public decimal Size { get; }

    /// <summary>
    /// Gets the data series color
    /// </summary>
    public string Color { get; }

    /// <inheritdoc/>
    public override string CssClass { get; }

    /// <summary>
    /// Creates an SVG square data marker shape
    /// </summary>
    /// <param name="x">X coordinate of the center point of the marker</param>
    /// <param name="y">Y coordinate of the center point of the marker</param>
    /// <param name="size">Width/height of the marker</param>
    /// <param name="color">Data series color</param>
    /// <param name="cssClass">Data series CSS class</param>
    /// <param name="layerIndex">Index of the layer that contains the data series</param>
    /// <param name="dataSeriesIndex">Index of the data series in the containing layer</param>
    /// <param name="dataPointIndex">Index of the data point in the data series</param>
    public SquareDataMarkerShape(decimal x, decimal y, decimal size, string color, string? cssClass, int layerIndex, int dataSeriesIndex, int dataPointIndex) : base(layerIndex, dataSeriesIndex, dataPointIndex) {
        X = x;
        Y = y;
        Size = size;
        Color = color;
        CssClass = $"{DefaultCssClass} {cssClass}";
    }

    /// <inheritdoc/>
    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X - Size / 2M },
        { "y", Y - Size / 2M },
        { "width", Size },
        { "height", Size },
        { "fill", Color }
    };
}
