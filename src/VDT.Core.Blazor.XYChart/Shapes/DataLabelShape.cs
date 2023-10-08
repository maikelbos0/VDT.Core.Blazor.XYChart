namespace VDT.Core.Blazor.XYChart.Shapes;

public class DataLabelShape : ShapeBase {
    public const string DefaultCssClass = "data-label";

    public override string ElementName => "text";
    public decimal X { get; }
    public decimal Y { get; }
    public string Value { get; }
    public override string CssClass { get; }

    public DataLabelShape(decimal x, decimal y, string value, string? cssClass, int layerIndex, int dataSeriesIndex, int dataPointIndex) : base(layerIndex, dataSeriesIndex, dataPointIndex) {
        Y = y;
        X = x;
        Value = value;
        CssClass = $"{DefaultCssClass} {cssClass}";
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y }
    };

    public override string? GetContent() => Value;
}
