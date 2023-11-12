namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// SVG shape to display the data series name in a legend item
/// </summary>
public class LegendTextShape : ShapeBase {
    /// <summary>
    /// Default CSS classes that are always applied to this SVG shape
    /// </summary>
    public const string DefaultCssClass = "legend-text";

    /// <inheritdoc/>
    public override string ElementName => "text";

    /// <summary>
    /// Gets the X-coordinate of the legend text
    /// </summary>
    public decimal X { get; }

    /// <summary>
    /// Gets the Y-coordinate of the legend text
    /// </summary>
    public decimal Y { get; }

    /// <summary>
    /// Gets the data series name
    /// </summary>
    public string DataSeriesName { get; }

    /// <inheritdoc/>
    public override string CssClass { get; }

    /// <summary>
    /// Create an SVG legend text shape
    /// </summary>
    /// <param name="x">X-coordinate of the legend text</param>
    /// <param name="y">Y-coordinate of the legend text</param>
    /// <param name="dataSeriesName">Data series name</param>
    /// <param name="cssClass">Data series CSS class</param>
    /// <param name="layerIndex">Index of the layer that contains the data series</param>
    /// <param name="dataSeriesIndex">Index of the data series in the containing layer</param>
    public LegendTextShape(decimal x, decimal y, string dataSeriesName, string? cssClass, int layerIndex, int dataSeriesIndex) : base(layerIndex, dataSeriesIndex) {
        CssClass = $"{DefaultCssClass} {cssClass}";
        X = x;
        Y = y;
        DataSeriesName = dataSeriesName;
    }

    /// <inheritdoc/>
    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y }
    };

    /// <inheritdoc/>
    public override string? GetContent() => DataSeriesName;
}
