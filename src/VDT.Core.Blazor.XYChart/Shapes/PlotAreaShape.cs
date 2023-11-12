namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// SVG shape to display the plot area
/// </summary>
public class PlotAreaShape : ShapeBase {
    /// <summary>
    /// Since the plot area shape is drawn in a negative to cover data shapes that cross its border, we want to go over the borders of the canvas; this bleed
    /// is how much we cross those borders
    /// </summary>
    public const int Bleed = 100;

    /// <inheritdoc/>
    public override string CssClass => "plot-area";

    /// <inheritdoc/>
    public override string ElementName => "path";
    
    /// <summary>
    /// Gets the total width of the canvas
    /// </summary>
    public int CanvasWidth { get; }

    /// <summary>
    /// Gets the total height of the canvas
    /// </summary>
    public int CanvasHeight { get; }

    /// <summary>
    /// Gets the X-coordinate of the top left corner of the plot area
    /// </summary>
    public int X { get; }

    /// <summary>
    /// Gets the Y-coordinate of the top left corner of the plot area
    /// </summary>
    public int Y { get; }

    /// <summary>
    /// Gets the width of the plot area
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the height of the plot area
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Creates an SVG plot area shape
    /// </summary>
    /// <param name="canvasWidth">Total width of the canvas</param>
    /// <param name="canvasHeight">Total height of the canvas</param>
    /// <param name="x">X-coordinate of the top left corner of the plot area</param>
    /// <param name="y">Y-coordinate of the top left corner of the plot area</param>
    /// <param name="width">Width of the plot area</param>
    /// <param name="height">Height of the plot area</param>
    public PlotAreaShape(int canvasWidth, int canvasHeight, int x, int y, int width, int height) {
        CanvasWidth = canvasWidth;
        CanvasHeight = canvasHeight;
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <inheritdoc/>
    public override ShapeAttributeCollection GetAttributes() => new() {
        { "d", $"M{-Bleed} {-Bleed} l{CanvasWidth + Bleed * 2} 0 l0 {CanvasHeight + Bleed * 2} l{-CanvasWidth - Bleed * 2} 0 Z M{X} {Y} l{Width} 0 l0 {Height} l{-Width} 0 Z" },
        { "fill-rule", "evenodd" }
    };
}