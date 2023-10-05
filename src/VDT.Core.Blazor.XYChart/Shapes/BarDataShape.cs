using System;

namespace VDT.Core.Blazor.XYChart.Shapes;

public class BarDataShape : ShapeBase {
    public const string DefaultCssClass = "data bar-data";

    public override string ElementName => "rect";
    public decimal X { get; }
    public decimal Y { get; }
    public decimal Width { get; }
    public decimal Height { get; }
    public string Color { get; }
    public override string CssClass { get; }

    public BarDataShape(decimal x, decimal y, decimal width, decimal height, string color, string? cssClass, int layerIndex, int dataSeriesIndex, int dataPointIndex) : base(layerIndex, dataSeriesIndex, dataPointIndex) {
        Y = y;
        X = x;
        Height = height;
        Width = width;
        Color = color;
        CssClass = $"{DefaultCssClass} {cssClass}";
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Height < 0 ? Y + Height : Y },
        { "width", Width },
        { "height", Math.Abs(Height) },
        { "fill", Color }
    };
}