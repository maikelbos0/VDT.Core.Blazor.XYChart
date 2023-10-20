using Microsoft.AspNetCore.Components;
using System;

namespace VDT.Core.Blazor.XYChart;

public class Legend : ChildComponentBase, IDisposable {
    public static bool DefaultIsEnabled { get; set; } = true;
    public static LegendPosition DefaultPosition { get; set; } = LegendPosition.Top;
    public static LegendAlignment DefaultAlignment { get; set; } = LegendAlignment.Center;
    public static int DefaultHeight { get; set; } = 25;
    public static int DefaultItemWidth { get; set; } = 100;
    public static int DefaultItemHeight { get; set; } = 25;
    public static int DefaultKeySize { get; set; } = 16;

    [Parameter] public bool IsEnabled { get; set; } = DefaultIsEnabled;
    [Parameter] public LegendPosition Position { get; set; } = DefaultPosition;
    [Parameter] public LegendAlignment Alignment { get; set; } = DefaultAlignment;
    [Parameter] public int Height { get; set; } = DefaultHeight;
    [Parameter] public int ItemWidth { get; set; } = DefaultItemWidth;
    [Parameter] public int ItemHeight { get; set; } = DefaultItemHeight;
    [Parameter] public int KeySize { get; set; } = DefaultKeySize;

    protected override void OnInitialized() => Chart.SetLegend(this);

    public void Dispose() {
        Chart.ResetLegend();
        GC.SuppressFinalize(this);
    }

    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(IsEnabled)
        || parameters.HasParameterChanged(Position)
        || parameters.HasParameterChanged(Alignment)
        || parameters.HasParameterChanged(Height)
        || parameters.HasParameterChanged(ItemWidth)
        || parameters.HasParameterChanged(ItemHeight)
        || parameters.HasParameterChanged(KeySize);
}
