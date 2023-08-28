﻿using System.Collections.Generic;

namespace VDT.Core.Blazor.XYChart.Shapes;

public class DataLineShape : ShapeBase {
    public override string CssClass => "data line-data";
    public override string ElementName => "path";
    public string Path { get; }
    public decimal Width { get; }
    public string Color { get; }

    public DataLineShape(IEnumerable<string> commands, decimal width, string color, int dataSeriesIndex) : base(dataSeriesIndex) {
        Path = string.Join(' ', commands);
        Width = width;
        Color = color;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "d", Path },
        { "stroke-width", Width },
        { "stroke", Color }
    };
}
