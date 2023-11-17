namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// SVG shape to display the value of a data point
/// </summary>
public class DataLabelShape : ShapeBase {
    /// <summary>
    /// Default CSS classes that are always applied to this SVG shape
    /// </summary>
    public const string DefaultCssClass = "data-label";

    /// <inheritdoc/>
    public override string ElementName => "text";

    /// <summary>
    /// Gets the X-coordinate of the anchor for this label
    /// </summary>
    public decimal X { get; }

    /// <summary>
    /// Gets the Y-coordinate of the anchor for this label
    /// </summary>
    public decimal Y { get; }

    /// <summary>
    /// Gets the data label value
    /// </summary>
    public string Value { get; }

    /// <inheritdoc/>
    public override string CssClass { get; }

    /// <summary>
    /// <see langword="true"/> if the data point value is positive; otherwise <see langword="false"/>
    /// </summary>
    public bool IsPositive { get; }

    /// <summary>
    /// Creates an SVG data label shape
    /// </summary>
    /// <param name="x">X-coordinate of the anchor for this label</param>
    /// <param name="y">Y-coordinate of the anchor for this label</param>
    /// <param name="value">Data label value</param>
    /// <param name="cssClass">Data series CSS class</param>
    /// <param name="isPositive"><see langword="true"/> if the data point value is positive; otherwise <see langword="false"/></param>
    /// <param name="layerIndex">Index of the layer that contains the data series</param>
    /// <param name="dataSeriesIndex">Index of the data series in the containing layer</param>
    /// <param name="dataPointIndex">Index of the data point in the data series</param>
    public DataLabelShape(decimal x, decimal y, string value, string? cssClass, bool isPositive, int layerIndex, int dataSeriesIndex, int dataPointIndex) : base(layerIndex, dataSeriesIndex, dataPointIndex) {
        Y = y;
        X = x;
        Value = value;
        CssClass = $"{DefaultCssClass} {cssClass}";
        IsPositive = isPositive;
    }

    /// <inheritdoc/>
    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y },
        { IsPositive ? "data-positive" : "data-negative", true }
    };

    /// <inheritdoc/>
    public override string? GetContent() => Value;
}
