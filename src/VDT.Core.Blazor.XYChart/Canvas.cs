﻿using Microsoft.AspNetCore.Components;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Canvas settings for an <see cref="XYChart"/>
/// </summary>
public class Canvas : ChildComponentBase, IAsyncDisposable {
    /// <summary>
    /// Gets or sets the default value for the total width of the chart, including axis labels and padding
    /// </summary>
    public static int DefaultWidth { get; set; } = 1200;

    /// <summary>
    /// Gets or sets the default value for the total height of the chart, including axis labels and padding
    /// </summary>
    public static int DefaultHeight { get; set; } = 500;

    /// <summary>
    /// Gets or sets the default value for the distance between the canvas edge and any chart elements
    /// </summary>
    public static int DefaultPadding { get; set; } = 25;

    /// <summary>
    /// Gets or sets the default value for whether or not axis labels should be automatically sized
    /// </summary>
    public static bool DefaultAutoSizeLabelsIsEnabled { get; set; } = false;

    /// <summary>
    /// Gets or sets the default value for the vertical room reserved for labels on the x-axis
    /// </summary>
    public static int DefaultXAxisLabelHeight { get; set; } = 50;

    /// <summary>
    /// Gets or sets the default value for the horizontal room reserved for labels on the y-axis, including the multiplier if applicable
    /// </summary>
    public static int DefaultYAxisLabelWidth { get; set; } = 75;

    /// <summary>
    /// Gets or sets the default value for the format string for the numeric labels on the y-axis
    /// </summary>
    public static string DefaultYAxisLabelFormat { get; set; } = "#,##0.######";

    /// <summary>
    /// Gets or sets the default value for the format string for the y-axis multiplier, if it's visible
    /// </summary>
    public static string DefaultYAxisMultiplierFormat { get; set; } = "x #,##0.######";

    /// <summary>
    /// Gets or sets the default value for the format string for the numeric data labels optionally displayed at each data point
    /// </summary>
    public static string DefaultDataLabelFormat { get; set; } = "#,##0.######";

    /// <summary>
    /// Gets or sets the total width of the chart, including axis labels and padding
    /// </summary>
    [Parameter] public int Width { get; set; } = DefaultWidth;

    /// <summary>
    /// Gets or sets the total height of the chart, including axis labels and padding
    /// </summary>
    [Parameter] public int Height { get; set; } = DefaultHeight;

    /// <summary>
    /// Gets or sets the distance between the canvas edge and any chart elements
    /// </summary>
    [Parameter] public int Padding { get; set; } = DefaultPadding;

    /// <summary>
    /// Gets or sets whether or not axis labels should be automatically sized
    /// </summary>
    [Parameter] public bool AutoSizeLabelsIsEnabled { get; set; } = DefaultAutoSizeLabelsIsEnabled;

    /// <summary>
    /// Gets or sets the vertical room reserved for labels on the x-axis
    /// </summary>
    [Parameter] public int XAxisLabelHeight { get; set; } = DefaultXAxisLabelHeight;

    /// <summary>
    /// Gets or sets the horizontal room reserved for labels on the y-axis, including the multiplier if applicable
    /// </summary>
    [Parameter] public int YAxisLabelWidth { get; set; } = DefaultYAxisLabelWidth;

    /// <summary>
    /// Gets or sets the format string for the numeric labels on the y-axis
    /// </summary>
    [Parameter] public string YAxisLabelFormat { get; set; } = DefaultYAxisLabelFormat;

    /// <summary>
    /// Gets or sets the format string for the y-axis multiplier, if it's visible
    /// </summary>
    [Parameter] public string YAxisMultiplierFormat { get; set; } = DefaultYAxisMultiplierFormat;

    /// <summary>
    /// Gets or sets the format string for the numeric data labels optionally displayed at each data point
    /// </summary>
    [Parameter] public string DataLabelFormat { get; set; } = DefaultDataLabelFormat;

    private int? AutoSizeXAxisLabelHeight { get; set; }

    /// <summary>
    /// Gets the lowest value that is visible in the chart, taking automatic scaling into account if enabled
    /// </summary>
    public int ActualXAxisLabelHeight => AutoSizeXAxisLabelHeight ?? XAxisLabelHeight;

    /// <summary>
    /// X-coordinate of the top left corner of the plot area
    /// </summary>
    public int PlotAreaX => Padding + YAxisLabelWidth;

    /// <summary>
    /// Y-coordinate of the top left corner of the plot area
    /// </summary>
    public int PlotAreaY => Padding + (Chart.Legend.IsEnabled && Chart.Legend.Position == LegendPosition.Top ? Chart.Legend.Height : 0);

    /// <summary>
    /// Width of the plot area
    /// </summary>
    public int PlotAreaWidth => Width - Padding * 2 - YAxisLabelWidth;

    /// <summary>
    /// Height of the plot area
    /// </summary>
    public int PlotAreaHeight => Height - Padding * 2 - ActualXAxisLabelHeight - (Chart.Legend.IsEnabled ? Chart.Legend.Height : 0);

    /// <summary>
    /// Y-coordinate of the top left corner of the legend
    /// </summary>
    public int LegendY => Chart.Legend.Position switch {
        LegendPosition.Top => Padding,
        LegendPosition.Bottom => Height - Padding - Chart.Legend.Height,
        _ => throw new NotImplementedException($"No implementation found for {nameof(LegendPosition)} '{Chart.Legend.Position}'.")
    };

    /// <inheritdoc/>
    protected override Task OnInitializedAsync() => Chart.SetCanvas(this);

    /// <inheritdoc/>
    public async ValueTask DisposeAsync() {
        await Chart.ResetCanvas();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(Width)
        || parameters.HasParameterChanged(Height)
        || parameters.HasParameterChanged(Padding)
        || parameters.HasParameterChanged(XAxisLabelHeight)
        || parameters.HasParameterChanged(YAxisLabelWidth)
        || parameters.HasParameterChanged(YAxisLabelFormat)
        || parameters.HasParameterChanged(YAxisMultiplierFormat)
        || parameters.HasParameterChanged(DataLabelFormat);

    /// <summary>
    /// Gets the SVG shape for the plot area
    /// </summary>
    /// <returns>The SVG plot area shape</returns>
    public Shapes.PlotAreaShape GetPlotAreaShape() => new(Width, Height, PlotAreaX, PlotAreaY, PlotAreaWidth, PlotAreaHeight);

    // TODO test
    /// <summary>
    /// Applies automatic sizing to labels if <see cref="AutoSizeLabelsIsEnabled"/> is <see langword="true" />
    /// </summary>
    /// <returns></returns>
    public async Task AutoSize() {
        if (!AutoSizeLabelsIsEnabled) {
            AutoSizeXAxisLabelHeight = null;
            return;
        }

        await using var sizeProvider = await Chart.GetSizeProvider();

        // TODO centralize css class
        AutoSizeXAxisLabelHeight = (int)(await Task.WhenAll(Chart.Labels.Select(async label => await sizeProvider.GetTextSize(label, "x-axis-label"))))
            .Max(size => size.Height);
    }
}
