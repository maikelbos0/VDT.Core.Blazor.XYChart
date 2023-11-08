namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// SVG shape to display a label on the X-axis
/// </summary>
public class XAxisLabelShape : ShapeBase {
    /// <inheritdoc/>
    public override string CssClass => "x-axis-label";

    /// <inheritdoc/>
    public override string ElementName => "text";

    /// <summary>
    /// Gets the X coordinate of the anchor for this label
    /// </summary>
    public decimal X { get; }

    /// <summary>
    /// Gets the Y coordinate of the anchor for this label
    /// </summary>
    public decimal Y { get; }

    /// <summary>
    /// Gets the label value
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// Creates an SVG X-axis label
    /// </summary>
    /// <param name="x">X coordinate of the anchor for this label</param>
    /// <param name="y">Y coordinate of the anchor for this label</param>
    /// <param name="label">Label value</param>
    /// <param name="index">Label index</param>
    public XAxisLabelShape(decimal x, decimal y, string label, int index) : base(index) {
        X = x;
        Y = y;
        Label = label;
    }

    /// <inheritdoc/>
    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y }
    };

    /// <inheritdoc/>
    public override string? GetContent() => Label;
}