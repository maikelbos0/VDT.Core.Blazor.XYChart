using Microsoft.AspNetCore.Components;
using System;

namespace VDT.Core.Blazor.XYChart;

public class Canvas : ChildComponentBase, IDisposable {
    public static int DefaultWidth { get; set; } = 1200;
    public static int DefaultHeight { get; set; } = 500;
    public static int DefaultPadding { get; set; } = 25;
    public static int DefaultXAxisLabelHeight { get; set; } = 50;
    public static int DefaultYAxisLabelWidth { get; set; } = 75;
    public static string DefaultYAxisLabelFormat { get; set; } = "#,##0.######";
    public static string DefaultYAxisMultiplierFormat { get; set; } = "x #,##0.######";
    public static string DefaultDataLabelFormat { get; set; } = "#,##0.######";

    [Parameter] public int Width { get; set; } = DefaultWidth;
    [Parameter] public int Height { get; set; } = DefaultHeight;
    [Parameter] public int Padding { get; set; } = DefaultPadding;
    [Parameter] public int XAxisLabelHeight { get; set; } = DefaultXAxisLabelHeight;
    [Parameter] public int YAxisLabelWidth { get; set; } = DefaultYAxisLabelWidth;
    [Parameter] public string YAxisLabelFormat { get; set; } = DefaultYAxisLabelFormat;
    [Parameter] public string YAxisMultiplierFormat { get; set; } = DefaultYAxisMultiplierFormat;
    [Parameter] public string DataLabelFormat { get; set; } = DefaultDataLabelFormat;
    public int PlotAreaX => Padding + YAxisLabelWidth;
    public int PlotAreaY => Padding + (Chart.Legend.IsEnabled && Chart.Legend.Position == LegendPosition.Top ? Chart.Legend.Height : 0);
    public int PlotAreaWidth => Width - Padding * 2 - YAxisLabelWidth;
    public int PlotAreaHeight => Height - Padding * 2 - XAxisLabelHeight - (Chart.Legend.IsEnabled ? Chart.Legend.Height : 0);
    public int LegendY => Chart.Legend.Position switch {
        LegendPosition.Top => Padding,
        LegendPosition.Bottom => Height - Padding - Chart.Legend.Height,
        _ => throw new NotImplementedException($"No implementation found for {nameof(LegendPosition)} '{Chart.Legend.Position}'.")
    };

    protected override void OnInitialized() => Chart.SetCanvas(this);

    public void Dispose() {
        Chart.ResetCanvas();
        GC.SuppressFinalize(this);
    }

    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(Width)
        || parameters.HasParameterChanged(Height)
        || parameters.HasParameterChanged(Padding)
        || parameters.HasParameterChanged(XAxisLabelHeight)
        || parameters.HasParameterChanged(YAxisLabelWidth)
        || parameters.HasParameterChanged(YAxisLabelFormat)
        || parameters.HasParameterChanged(YAxisMultiplierFormat)
        || parameters.HasParameterChanged(DataLabelFormat);

    public Shapes.PlotAreaShape GetPlotAreaShape() => new(Width, Height, PlotAreaX, PlotAreaY, PlotAreaWidth, PlotAreaHeight);
}
