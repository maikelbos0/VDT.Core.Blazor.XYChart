namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// SVG shape to display the color key of a legend item
/// </summary>
public class LegendKeyShape : ShapeBase {
    /// <summary>
    /// Default CSS classes that are always applied to this SVG shape
    /// </summary>
    public const string DefaultCssClass = "legend-key";

    /// <inheritdoc/>
    public override string ElementName => "rect";

    /// <summary>
    /// Gets the X coordinate of the legend key
    /// </summary>
    public decimal X { get; }

    /// <summary>
    /// Gets the Y coordinate of the legend key
    /// </summary>
    public decimal Y { get; }

    /// <summary>
    /// Gets the width/height of the legend key
    /// </summary>
    public decimal Size { get; }

    /// <summary>
    /// Gets the data series color
    /// </summary>
    public string Color { get; }

    /// <inheritdoc/>
    public override string CssClass { get; }

    /// <summary>
    /// Create an SVG legend key shape
    /// </summary>
    /// <param name="x">X coordinate of the legend key</param>
    /// <param name="y">Y coordinate of the legend key</param>
    /// <param name="size">Width/height of the legend key</param>
    /// <param name="color">Data series color</param>
    /// <param name="cssClass">Data series CSS class</param>
    /// <param name="layerIndex">Index of the layer that contains the data series</param>
    /// <param name="dataSeriesIndex">Index of the data series in the containing layer</param>
    public LegendKeyShape(decimal x, decimal y, decimal size, string color, string? cssClass, int layerIndex, int dataSeriesIndex) : base(layerIndex, dataSeriesIndex) {
        Y = y;
        X = x;
        Size = size;
        Color = color;
        CssClass = $"{DefaultCssClass} {cssClass}";
    }

    /// <inheritdoc/>
    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y },
        { "width", Size },
        { "height", Size },
        { "fill", Color }
    };
}
