using Microsoft.AspNetCore.Components;
using System;

namespace VDT.Core.Blazor.XYChart;

public class Legend : ChildComponentBase, IDisposable {
    public static LegendPosition DefaultPosition { get; set; } = LegendPosition.None;
    public static int DefaultHeight { get; set; } = 25;
    public static int DefaultItemWidth { get; set; } = 25;
    public static int DefaultItemHeight { get; set; } = 1000;

    [Parameter] public LegendPosition Position { get; set; } = DefaultPosition;
    [Parameter] public int Height { get; set; } = DefaultHeight;
    [Parameter] public int ItemWidth { get; set; } = DefaultItemWidth;
    [Parameter] public int ItemHeight { get; set; } = DefaultItemHeight;

    protected override void OnInitialized() => Chart.SetLegend(this);

    public void Dispose() {
        Chart.ResetLegend();
        GC.SuppressFinalize(this);
    }

    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(Position)
        || parameters.HasParameterChanged(Height)
        || parameters.HasParameterChanged(ItemWidth)
        || parameters.HasParameterChanged(ItemHeight);
}
