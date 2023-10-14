namespace VDT.Core.Blazor.XYChart.Shapes;

public class DataLabelShape : ShapeBase {
    public const string DefaultCssClass = "data-label";

    public override string ElementName => "text";
    public decimal X { get; }
    public decimal Y { get; }
    public string Value { get; }
    public override string CssClass { get; }
    public bool IsPositive { get; }

    public DataLabelShape(decimal x, decimal y, string value, string? cssClass, bool isPositive, int layerIndex, int dataSeriesIndex, int dataPointIndex) : base(layerIndex, dataSeriesIndex, dataPointIndex) {
        Y = y;
        X = x;
        Value = value;
        CssClass = $"{DefaultCssClass} {cssClass}";
        IsPositive = isPositive;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y },
        { IsPositive ? "data-positive" : "data-negative", true }
    };

    public override string? GetContent() => Value;
}
