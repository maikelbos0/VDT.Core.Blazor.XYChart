using Microsoft.AspNetCore.Components;
using System;

namespace VDT.Core.Blazor.XYChart;

public class AutoScaleSettings : ChildComponentBase, IDisposable {
    public static bool DefaultIsEnabled { get; set; } = true;
    public static int DefaultRequestedGridLineCount { get; set; } = 11;
    public static bool DefaultIncludeZero { get; set; } = false;
    public static decimal DefaultClearancePercentage { get; set; } = 5M;

    [Parameter] public bool IsEnabled { get; set; } = DefaultIsEnabled;
    [Parameter] public int RequestedGridLineCount { get; set; } = DefaultRequestedGridLineCount;
    [Parameter] public bool IncludeZero { get; set; } = DefaultIncludeZero;
    [Parameter] public decimal ClearancePercentage { get; set; } = DefaultClearancePercentage;

    protected override void OnInitialized() => Chart.SetAutoScaleSettings(this);

    public void Dispose() {
        Chart.ResetAutoScaleSettings();
        GC.SuppressFinalize(this);
    }

    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(IsEnabled)
        || parameters.HasParameterChanged(RequestedGridLineCount)
        || parameters.HasParameterChanged(IncludeZero)
        || parameters.HasParameterChanged(ClearancePercentage);
}
