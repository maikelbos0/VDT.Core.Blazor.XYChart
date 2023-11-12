using System;

namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// SVG shape to display a data point in a bar data series
/// </summary>
public class BarDataShape : ShapeBase {
    /// <summary>
    /// Default CSS classes that are always applied to this SVG shape
    /// </summary>
    public const string DefaultCssClass = "data bar-data";
    
    /// <inheritdoc/>
    public override string ElementName => "rect";

    /// <summary>
    /// Gets the X-coordinate of the top left corner of the bar
    /// </summary>
    public decimal X { get; }

    /// <summary>
    /// Gets the Y-coordinate of the top left corner of the bar; if <see cref="Height"/> is negative it will be added to the resulting attribute
    /// </summary>
    public decimal Y { get; }

    /// <summary>
    /// Gets the width of the bar
    /// </summary>
    public decimal Width { get; }

    /// <summary>
    /// Gets the height of the bar; if negative it will be negated
    /// </summary>
    public decimal Height { get; }

    /// <summary>
    /// Gets the data series color
    /// </summary>
    public string Color { get; }

    /// <inheritdoc/>
    public override string CssClass { get; }

    /// <summary>
    /// Creates an SVG bar data shape
    /// </summary>
    /// <param name="x">X-coordinate of the top left corner of the bar</param>
    /// <param name="y">Y-coordinate of the top left corner of the bar</param>
    /// <param name="width">Width of the bar</param>
    /// <param name="height">Height of the bar</param>
    /// <param name="color">Data series color</param>
    /// <param name="cssClass">Data series CSS class</param>
    /// <param name="layerIndex">Index of the layer that contains the data series</param>
    /// <param name="dataSeriesIndex">Index of the data series in the containing layer</param>
    /// <param name="dataPointIndex">Index of the data point in the data series</param>
    public BarDataShape(decimal x, decimal y, decimal width, decimal height, string color, string? cssClass, int layerIndex, int dataSeriesIndex, int dataPointIndex) : base(layerIndex, dataSeriesIndex, dataPointIndex) {
        Y = y;
        X = x;
        Height = height;
        Width = width;
        Color = color;
        CssClass = $"{DefaultCssClass} {cssClass}";
    }

    /// <inheritdoc/>
    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Height < 0 ? Y + Height : Y },
        { "width", Width },
        { "height", Math.Abs(Height) },
        { "fill", Color }
    };
}