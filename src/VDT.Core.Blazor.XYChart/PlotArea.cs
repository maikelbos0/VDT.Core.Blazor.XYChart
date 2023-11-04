using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VDT.Core.Blazor.XYChart;

public class PlotArea : ChildComponentBase, IDisposable {
    public static decimal DefaultMin { get; set; } = 0M;
    public static decimal DefaultMax { get; set; } = 50M;
    public static decimal DefaultGridLineInterval { get; set; } = 5M;
    public static decimal DefaultMultiplier { get; set; } = 1M;
    public static bool DefaultAutoScaleIsEnabled { get; set; } = true;
    public static int DefaultAutoScaleRequestedGridLineCount { get; set; } = 11;
    public static bool DefaultAutoScaleIncludesZero { get; set; } = true;
    public static decimal DefaultAutoScaleClearancePercentage { get; set; } = 5M;

    [Parameter] public decimal Min { get; set; } = DefaultMin;
    [Parameter] public decimal Max { get; set; } = DefaultMax;
    [Parameter] public decimal GridLineInterval { get; set; } = DefaultGridLineInterval;
    [Parameter] public decimal Multiplier { get; set; } = DefaultMultiplier;
    [Parameter] public bool AutoScaleIsEnabled { get; set; } = DefaultAutoScaleIsEnabled;
    [Parameter] public int AutoScaleRequestedGridLineCount { get; set; } = DefaultAutoScaleRequestedGridLineCount;
    [Parameter] public bool AutoScaleIncludesZero { get; set; } = DefaultAutoScaleIncludesZero;
    [Parameter] public decimal AutoScaleClearancePercentage { get; set; } = DefaultAutoScaleClearancePercentage;

    private decimal? AutoScaleMin { get; set; }
    private decimal? AutoScaleMax { get; set; }
    private decimal? AutoScaleGridLineInterval { get; set; }

    internal decimal ActualMin => AutoScaleMin ?? Min;
    internal decimal ActualMax => AutoScaleMax ?? Max;
    internal decimal ActualGridLineInterval => AutoScaleGridLineInterval ?? GridLineInterval;


    protected override void OnInitialized() => Chart.SetPlotArea(this);

    public void Dispose() {
        Chart.ResetPlotArea();
        GC.SuppressFinalize(this);
    }

    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(Min)
        || parameters.HasParameterChanged(Max)
        || parameters.HasParameterChanged(GridLineInterval)
        || parameters.HasParameterChanged(Multiplier)
        || parameters.HasParameterChanged(AutoScaleIsEnabled)
        || parameters.HasParameterChanged(AutoScaleRequestedGridLineCount)
        || parameters.HasParameterChanged(AutoScaleIncludesZero)
        || parameters.HasParameterChanged(AutoScaleClearancePercentage);

    public void AutoScale(IEnumerable<decimal> dataPoints) {
        if (!AutoScaleIsEnabled) {
            return;
        }

        var min = DefaultMin;
        var max = DefaultMax;

        if (dataPoints.Any()) {
            min = dataPoints.Min();
            max = dataPoints.Max();

            if (min == max) {
                min -= (DefaultMax - DefaultMin) / 2M;
                max += (DefaultMax - DefaultMin) / 2M;
            }
            else {
                var clearance = (max - min) / (100M - AutoScaleClearancePercentage * 2) * AutoScaleClearancePercentage;

                min -= clearance;
                max += clearance;
            }

            if (min > 0M && AutoScaleIncludesZero) {
                min = 0M;
            }

            if (max < 0M && AutoScaleIncludesZero) {
                max = 0M;
            }
        }

        var rawGridLineInterval = (max - min) / Math.Max(1, AutoScaleRequestedGridLineCount - 1);
        var baseMultiplier = DecimalMath.Pow(10M, (int)Math.Floor((decimal)Math.Log10((double)rawGridLineInterval)));
        var scale = new[] { 1M, 2M, 5M, 10M }
            .Select(baseGridLineInterval => baseGridLineInterval * baseMultiplier)
            .Select(gridLineInterval => new {
                GridLineInterval = gridLineInterval,
                Min = DecimalMath.FloorToScale(min, gridLineInterval),
                Max = DecimalMath.CeilingToScale(max, gridLineInterval)
            })
            .OrderBy(candidate => Math.Abs((candidate.Max - candidate.Min) / candidate.GridLineInterval - AutoScaleRequestedGridLineCount))
            .First();

        AutoScaleMin = DecimalMath.Trim(scale.Min);
        AutoScaleMax = DecimalMath.Trim(scale.Max);
        AutoScaleGridLineInterval = DecimalMath.Trim(scale.GridLineInterval);
    }

    public IEnumerable<decimal> GetGridLineDataPoints() {
        var dataPoint = DecimalMath.CeilingToScale(ActualMin, ActualGridLineInterval);

        while (dataPoint <= ActualMax) {
            yield return dataPoint;

            dataPoint += ActualGridLineInterval;
        }
    }
}
