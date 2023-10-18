using Microsoft.AspNetCore.Components;
using System;

namespace VDT.Core.Blazor.XYChart;

public class Legend : ChildComponentBase, IDisposable {
    public static LegendPosition DefaultLegendPosition { get; set; } = LegendPosition.None;
    public static int DefaultLegendHeight { get; set; } = 25;
    public static int DefaultLegendItemWidth { get; set; } = 25;
    public static int DefaultLegendItemHeight { get; set; } = 1000;

    [Parameter] public LegendPosition LegendPosition { get; set; } = DefaultLegendPosition;
    [Parameter] public int LegendHeight { get; set; } = DefaultLegendHeight;
    [Parameter] public int LegendItemWidth { get; set; } = DefaultLegendItemWidth;
    [Parameter] public int LegendItemHeight { get; set; } = DefaultLegendItemHeight;

    protected override void OnInitialized() => Chart.SetLegend(this);

    public void Dispose() {
        Chart.ResetLegend();
        GC.SuppressFinalize(this);
    }

    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(LegendPosition)
        || parameters.HasParameterChanged(LegendHeight)
        || parameters.HasParameterChanged(LegendItemWidth)
        || parameters.HasParameterChanged(LegendItemHeight);
}
