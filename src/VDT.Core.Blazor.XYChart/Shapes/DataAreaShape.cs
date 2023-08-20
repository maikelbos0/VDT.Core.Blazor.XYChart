using System.Collections.Generic;

namespace VDT.Core.Blazor.XYChart.Shapes;

public class DataAreaShape : ShapeBase {
    public override string CssClass => "data area-data";
    public override string ElementName => "path";
    public string Path { get; }
    public string Color { get; }

    public DataAreaShape(IEnumerable<string> commands, string color, int dataSeriesIndex) : base(dataSeriesIndex) {
        Path = string.Join(' ', commands);
        Color = color;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "d", Path },
        { "fill", Color }
    };
}
