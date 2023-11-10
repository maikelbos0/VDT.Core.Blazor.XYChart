namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// SVG shape to display the Y-axis multiplier
/// </summary>
public class YAxisMultiplierShape : ShapeBase {
    /// <inheritdoc/>
    public override string CssClass => "y-axis-multiplier";

    /// <inheritdoc/>
    public override string ElementName => "text";

    /// <summary>
    /// Gets the X coordinate of the anchor for the multiplier
    /// </summary>
    public decimal X { get; }

    /// <summary>
    /// Gets the Y coordinate of the anchor for the multiplier
    /// </summary>
    public decimal Y { get; }

    /// <summary>
    /// Gets the multiplier value
    /// </summary>
    public string Multiplier { get; }

    /// <summary>
    /// Creates an SVG Y-axis multiplier
    /// </summary>
    /// <param name="x">X coordinate of the anchor for the multiplier</param>
    /// <param name="y">Y coordinate of the anchor for the multiplier</param>
    /// <param name="multiplier">Multiplier value</param>
    public YAxisMultiplierShape(decimal x, decimal y, string multiplier) {
        X = x;
        Y = y;
        Multiplier = multiplier;
    }

    /// <inheritdoc/>
    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y }
    };

    /// <inheritdoc/>
    public override string? GetContent() => Multiplier;
}
