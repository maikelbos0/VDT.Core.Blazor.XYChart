using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Ploy area/scaling settings for an <see cref="XYChart"/>
/// </summary>
public class PlotArea : ChildComponentBase, IDisposable {
    /// <summary>
    /// Gets or sets the default value for the lowest data point value that is visible in the chart
    /// </summary>
    public static decimal DefaultMin { get; set; } = 0M;

    /// <summary>
    /// Gets or sets the default value for the highest data point value that is visible in the chart
    /// </summary>
    public static decimal DefaultMax { get; set; } = 50M;

    /// <summary>
    /// Gets or sets the default value for the interval with which grid lines are shown; the starting point is zero
    /// </summary>
    public static decimal DefaultGridLineInterval { get; set; } = 5M;

    /// <summary>
    /// Gets or sets the default value for the unit multiplier when showing y-axis labels and data labels; all values get divided by this value before
    /// being displayed
    /// </summary>
    public static decimal DefaultMultiplier { get; set; } = 1M;

    /// <summary>
    /// Gets or sets the default value for whether or not automatic scaling of the plot area minimum, maximum and grid line interval is enabled; if enabled
    /// the values for <see cref="Min"/>, <see cref="Max"/> and <see cref="GridLineInterval"/> will be ignored
    /// </summary>
    public static bool DefaultAutoScaleIsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the default value for the preferred number of grid lines that should be visible if automatic scaling is enabled; please note that the end
    /// result can differ from the requested count
    /// </summary>
    public static int DefaultAutoScaleRequestedGridLineCount { get; set; } = 11;

    /// <summary>
    /// Gets or sets the default value for the forced inclusion of the zero line in the plot area when automatically scaling
    /// </summary>
    public static bool DefaultAutoScaleIncludesZero { get; set; } = true;

    /// <summary>
    /// Gets or sets the default value for the minimum clearance between the highest/lowest data point and the edge of the plot area when automatically 
    /// scaling, expressed as a percentage of the total plot area range
    /// </summary>
    public static decimal DefaultAutoScaleClearancePercentage { get; set; } = 5M;

    /// <summary>
    /// Gets or sets the lowest data point value that is visible in the chart
    /// </summary>
    [Parameter] public decimal Min { get; set; } = DefaultMin;

    /// <summary>
    /// Gets or sets the highest data point value that is visible in the chart
    /// </summary>
    [Parameter] public decimal Max { get; set; } = DefaultMax;

    /// <summary>
    /// Gets or sets the interval with which grid lines are shown; the starting point is zero
    /// </summary>
    [Parameter] public decimal GridLineInterval { get; set; } = DefaultGridLineInterval;

    /// <summary>
    /// Gets or sets the unit multiplier when showing y-axis labels and data labels; all values get divided by this value before being displayed
    /// </summary>
    [Parameter] public decimal Multiplier { get; set; } = DefaultMultiplier;

    /// <summary>
    /// Gets or sets whether or not automatic scaling of the plot area minimum, maximum and grid line interval is enabled; if enabled the values for 
    /// <see cref="Min"/>, <see cref="Max"/> and <see cref="GridLineInterval"/> will be ignored
    /// </summary>
    [Parameter] public bool AutoScaleIsEnabled { get; set; } = DefaultAutoScaleIsEnabled;

    /// <summary>
    /// Gets or sets the preferred number of grid lines that should be visible if automatic scaling is enabled; please note that the end result can differ 
    /// from the requested count
    /// </summary>
    [Parameter] public int AutoScaleRequestedGridLineCount { get; set; } = DefaultAutoScaleRequestedGridLineCount;

    /// <summary>
    /// Gets or sets the forced inclusion of the zero line in the plot area when automatically scaling
    /// </summary>
    [Parameter] public bool AutoScaleIncludesZero { get; set; } = DefaultAutoScaleIncludesZero;

    /// <summary>
    /// Gets or sets the minimum clearance between the highest/lowest data point and the edge of the plot area when automatically scaling, expressed as a
    /// percentage of the total plot area range
    /// </summary>
    [Parameter] public decimal AutoScaleClearancePercentage { get; set; } = DefaultAutoScaleClearancePercentage;

    private decimal? AutoScaleMin { get; set; }
    private decimal? AutoScaleMax { get; set; }
    private decimal? AutoScaleGridLineInterval { get; set; }

    /// <summary>
    /// Gets the lowest value that is visible in the chart, taking automatic scaling into account if enabled
    /// </summary>
    public decimal ActualMin => AutoScaleMin ?? Min;

    /// <summary>
    /// Gets the highest value that is visible in the chart, taking automatic scaling into account if enabled
    /// </summary>
    public decimal ActualMax => AutoScaleMax ?? Max;

    /// <summary>
    /// Gets the interval with which grid lines are shown, taking automatic scaling into account if enabled
    /// </summary>
    public decimal ActualGridLineInterval => AutoScaleGridLineInterval ?? GridLineInterval;

    /// <inheritdoc/>
    protected override void OnInitialized() => Chart.SetPlotArea(this);

    /// <inheritdoc/>
    public void Dispose() {
        Chart.ResetPlotArea();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(Min)
        || parameters.HasParameterChanged(Max)
        || parameters.HasParameterChanged(GridLineInterval)
        || parameters.HasParameterChanged(Multiplier)
        || parameters.HasParameterChanged(AutoScaleIsEnabled)
        || parameters.HasParameterChanged(AutoScaleRequestedGridLineCount)
        || parameters.HasParameterChanged(AutoScaleIncludesZero)
        || parameters.HasParameterChanged(AutoScaleClearancePercentage);

    /// <summary>
    /// Applies automatic scaling to the plot area if <see cref="AutoScaleIsEnabled"/> is <see langword="true"/>
    /// </summary>
    /// <param name="dataPoints">All data points that should be displayed in the plot area, taking stacking into account</param>
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

    /// <summary>
    /// Gets the data point values on which grid lines and their value labels should be placed
    /// </summary>
    /// <returns>The data point values</returns>
    public IEnumerable<decimal> GetGridLineDataPoints() {
        var dataPoint = DecimalMath.CeilingToScale(ActualMin, ActualGridLineInterval);

        while (dataPoint <= ActualMax) {
            yield return dataPoint;

            dataPoint += ActualGridLineInterval;
        }
    }
}
