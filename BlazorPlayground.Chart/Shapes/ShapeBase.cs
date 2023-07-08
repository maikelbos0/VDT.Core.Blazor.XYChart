﻿namespace BlazorPlayground.Chart.Shapes;

public abstract class ShapeBase {
    public abstract string CssClass { get; }
    public abstract string ElementName { get; }

    public abstract string GetKey();
    public abstract ShapeAttributeCollection GetAttributes();
    public virtual string? GetContent() => null;
}
