namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// SVG shape to display a label on the Y-axis
/// </summary>
public class YAxisLabelShape : ShapeBase {
    /// <summary>
    /// Default CSS classes that are always applied to this SVG shape
    /// </summary>
    public const string DefaultCssClass = "y-axis-label";

    /// <inheritdoc/>
    public override string CssClass => DefaultCssClass;

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
    /// Gets the label value
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Creates an SVG Y-axis label
    /// </summary>
    /// <param name="x">X-coordinate of the anchor for this label</param>
    /// <param name="y">Y-coordinate of the anchor for this label</param>
    /// <param name="value">Label value</param>
    /// <param name="index">Label index</param>
    public YAxisLabelShape(decimal x, decimal y, string value, int index) : base(index) {
        X = x;
        Y = y;
        Value = value;
    }

    /// <inheritdoc/>
    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y },
        { "text-anchor", "end" },
        { "dominant-baseline", "middle" }
    };

    /// <inheritdoc/>
    public override string? GetContent() => Value;
}
