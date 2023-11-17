using System.Collections.Generic;

namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// SVG shape to display an area data series
/// </summary>
public class AreaDataShape : ShapeBase {
    /// <summary>
    /// Default CSS classes that are always applied to this SVG shape
    /// </summary>
    public const string DefaultCssClass = "data area-data";

    /// <inheritdoc/>
    public override string ElementName => "path";

    /// <summary>
    /// Gets the series of commands that will draw the path for this area data series
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Gets the data series color
    /// </summary>
    public string Color { get; }

    /// <inheritdoc/>
    public override string CssClass { get; }

    /// <summary>
    /// Creates an SVG area data shape
    /// </summary>
    /// <param name="commands">Series of commands that will draw the path for this area data series</param>
    /// <param name="color">Data series color</param>
    /// <param name="cssClass">Data series CSS class</param>
    /// <param name="layerIndex">Index of the layer that contains the data series</param>
    /// <param name="dataSeriesIndex">Index of the data series in the containing layer</param>
    public AreaDataShape(IEnumerable<string> commands, string color, string? cssClass, int layerIndex, int dataSeriesIndex) : base(layerIndex, dataSeriesIndex) {
        Path = string.Join(' ', commands);
        Color = color;
        CssClass = $"{DefaultCssClass} {cssClass}";
    }

    /// <inheritdoc/>
    public override ShapeAttributeCollection GetAttributes() => new() {
        { "d", Path },
        { "fill", Color }
    };
}
