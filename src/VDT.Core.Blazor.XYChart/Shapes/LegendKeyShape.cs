using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart.Shapes;

public class LegendKeyShape : ShapeBase {
    public const string DefaultCssClass = "legend-key";

    public override string ElementName => "rect";
    public decimal X { get; }
    public decimal Y { get; }
    public decimal Size { get; }
    public string Color { get; }
    public override string CssClass { get; }

    public LegendKeyShape(decimal x, decimal y, decimal size, string color, string? cssClass, int layerIndex, int dataSeriesIndex) : base(layerIndex, dataSeriesIndex) {
        Y = y;
        X = x;
        Size = size;
        Color = color;
        CssClass = $"{DefaultCssClass} {cssClass}";
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y },
        { "width", Size },
        { "height", Size },
        { "fill", Color }
    };
}
