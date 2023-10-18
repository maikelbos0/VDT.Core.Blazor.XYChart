using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart.Shapes;

public class LegendKeyShape : ShapeBase {
    public const string DefaultCssClass = "legend-key";

    public override string ElementName => "rect";
    public decimal X { get; }
    public decimal Y { get; }
    public decimal Width { get; }
    public decimal Height { get; }
    public string Color { get; }
    public override string CssClass { get; }

    public LegendKeyShape(decimal x, decimal y, decimal width, decimal height, string color, string? cssClass, int layerIndex, int dataSeriesIndex) : base(layerIndex, dataSeriesIndex) {
        Y = y;
        X = x;
        Height = height;
        Width = width;
        Color = color;
        CssClass = $"{DefaultCssClass} {cssClass}";
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y },
        { "width", Width },
        { "height", Height },
        { "fill", Color }
    };
}
