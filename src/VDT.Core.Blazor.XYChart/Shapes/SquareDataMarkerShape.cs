namespace VDT.Core.Blazor.XYChart.Shapes;

public class SquareDataMarkerShape : ShapeBase {
    public const string DefaultCssClass = "data-marker data-marker-square";

    public override string ElementName => "rect";
    public decimal X { get; }
    public decimal Y { get; }
    public decimal Size { get; }
    public string Color { get; }
    public override string CssClass { get; }

    public SquareDataMarkerShape(decimal x, decimal y, decimal size, string color, string? cssClass, int layerIndex, int dataSeriesIndex, int dataPointIndex) : base(layerIndex, dataSeriesIndex, dataPointIndex) {
        X = x;
        Y = y;
        Size = size;
        Color = color;
        CssClass = $"{DefaultCssClass} {cssClass}";
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X - Size / 2M },
        { "y", Y - Size / 2M },
        { "width", Size },
        { "height", Size },
        { "fill", Color }
    };
}
