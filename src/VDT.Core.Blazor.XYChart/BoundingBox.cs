using System;

namespace VDT.Core.Blazor.XYChart;

internal record BoundingBox(decimal X, decimal Y, decimal Width, decimal Height) {
    public int RequiredWidth => (int)Math.Ceiling(Math.Abs(X) > Width ? Math.Abs(X) : Width + Math.Abs(X));
    public int RequiredHeight => (int)Math.Ceiling(Math.Abs(Y) > Height ? Math.Abs(Y) : Height + Math.Abs(Y));
}
