namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// SVG shape to display a grid line in the plot area
/// </summary>
public class GridLineShape : ShapeBase {
    /// <inheritdoc/>
    public override string CssClass => "grid-line";

    /// <inheritdoc/>
    public override string ElementName => "line";

    /// <summary>
    /// Gets the X-coordinate of the left edge of the grid line
    /// </summary>
    public decimal X { get; }

    /// <summary>
    /// Gets the Y-coordinate of the grid line
    /// </summary>
    public decimal Y { get; }

    /// <summary>
    /// Gets the width of the grid line
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the data value of the grid line
    /// </summary>
    public decimal Value { get; }

    /// <summary>
    /// Creates an SVG grid line shape
    /// </summary>
    /// <param name="x">X-coordinate of the left edge of the grid line</param>
    /// <param name="y">Y-coordinate of the grid line</param>
    /// <param name="width">Width of the grid line</param>
    /// <param name="value">Data value of the grid line</param>
    /// <param name="index">Index of the grid line in the plot area</param>
    public GridLineShape(decimal x, decimal y, int width, decimal value, int index) : base(index) {
        X = x;
        Y = y;
        Width = width;
        Value = value;
    }

    /// <inheritdoc/>
    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x1", X },
        { "y1", Y },
        { "x2", X + Width },
        { "y2", Y },
        { "value", Value },
        { "stroke", "grey" }
    };
}