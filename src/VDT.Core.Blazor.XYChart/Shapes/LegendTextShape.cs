using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart.Shapes;

public class LegendTextShape : ShapeBase {
    public const string DefaultCssClass = "legend-text";

    public override string ElementName => "text";
    public decimal X { get; }
    public decimal Y { get; }
    public string DataSeriesName { get; }
    public override string CssClass { get; }

    public LegendTextShape(decimal x, decimal y, string dataSeriesName, string? cssClass, int layerIndex, int dataSeriesIndex) : base(layerIndex, dataSeriesIndex) {

        CssClass = $"{DefaultCssClass} {cssClass}";
        X = x;
        Y = y;
        DataSeriesName = dataSeriesName;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x", X },
        { "y", Y }
    };

    public override string? GetContent() => DataSeriesName;
}
