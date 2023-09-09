using System.Collections.Generic;

namespace VDT.Core.Blazor.XYChart.Shapes;

public class LineDataShape : ShapeBase {
    public const string DefaultCssClass = "data line-data";

    public override string ElementName => "path";
    public string Path { get; }
    public string Color { get; }
    public override string CssClass { get; }

    public LineDataShape(IEnumerable<string> commands, string color, string? cssClass, int dataSeriesIndex) : base(dataSeriesIndex) {
        Path = string.Join(' ', commands);
        Color = color;
        CssClass = $"{DefaultCssClass} {cssClass}";
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "d", Path },
        { "stroke", Color }
    };
}
