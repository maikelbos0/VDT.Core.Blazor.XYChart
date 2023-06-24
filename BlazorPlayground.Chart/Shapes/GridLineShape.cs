﻿namespace BlazorPlayground.Chart.Shapes;

public class GridLineShape : ShapeBase {
    public override string CssClass => "grid-line";
    public override string ElementName => "line";
    public double X { get; }
    public double Y { get; }
    public int Width { get; }

    public GridLineShape(double x, double y, int width) {
        X = x;
        Y = y;
        Width = width;
    }

    public override ShapeAttributeCollection GetAttributes() => new() {
        { "x1", X },
        { "y1", Y },
        { "x2", X + Width },
        { "y2", Y }
    };           
}