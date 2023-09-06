namespace VDT.Core.Blazor.XYChart.Shapes;

public class RoundDataMarkerShape : ShapeBase {
    public const string DefaultCssClass = "data-marker data-marker-round";
    
    public override string ElementName => "circle";
    public decimal X { get; }
    public decimal Y { get; }
    public decimal Size { get; }
    public string Color { get; }
    public override string CssClass { get; }

    public RoundDataMarkerShape(decimal x, decimal y, decimal size, string color, string? cssClass, int dataSeriesIndex, int dataPointIndex) : base(dataSeriesIndex, dataPointIndex) {
        X = x;
        Y = y;
        Size = size;
        Color = color;
        CssClass = $"{DefaultCssClass} {cssClass}";
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "cx", X },
        { "cy", Y },
        { "r", Size / 2M },
        { "fill", Color }
    };
}
