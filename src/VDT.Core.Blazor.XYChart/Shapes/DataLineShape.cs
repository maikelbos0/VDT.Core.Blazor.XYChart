using System.Collections.Generic;

namespace VDT.Core.Blazor.XYChart.Shapes;

public class DataLineShape : ShapeBase {
    public const string DefaultCssClass = "data line-data";

    public override string ElementName => "path";
    public string Path { get; }
    public decimal Width { get; }
    public string Color { get; }
    public override string CssClass { get; }

    public DataLineShape(IEnumerable<string> commands, decimal width, string color, string? cssClass, int dataSeriesIndex) : base(dataSeriesIndex) {
        Path = string.Join(' ', commands);
        Width = width;
        Color = color;
        CssClass = $"{DefaultCssClass} {cssClass}";
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "d", Path },
        { "stroke-width", Width },
        { "stroke", Color }
    };
}
