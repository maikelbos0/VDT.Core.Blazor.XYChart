using Microsoft.AspNetCore.Components;
using System;

namespace VDT.Core.Blazor.XYChart;

public class Canvas : ChildComponentBase, IDisposable {
    public static int DefaultWidth { get; set; } = 1200;
    public static int DefaultHeight { get; set; } = 600;
    public static int DefaultPadding { get; set; } = 25;
    public static int DefaultXAxisLabelHeight { get; set; } = 100;
    public static int DefaultXAxisLabelClearance { get; set; } = 10;
    public static int DefaultYAxisLabelWidth { get; set; } = 100;
    public static int DefaultYAxisLabelClearance { get; set; } = 10;
    public static string DefaultYAxisLabelFormat { get; set; } = "#,##0.######";
    public static string DefaultYAxisMultiplierFormat { get; set; } = "x #,##0.######";

    [Parameter] public int Width { get; set; } = DefaultWidth;
    [Parameter] public int Height { get; set; } = DefaultHeight;
    [Parameter] public int Padding { get; set; } = DefaultPadding;
    [Parameter] public int XAxisLabelHeight { get; set; } = DefaultXAxisLabelHeight;
    [Parameter] public int XAxisLabelClearance { get; set; } = DefaultXAxisLabelClearance;
    [Parameter] public int YAxisLabelWidth { get; set; } = DefaultYAxisLabelWidth;
    [Parameter] public int YAxisLabelClearance { get; set; } = DefaultYAxisLabelClearance;
    [Parameter] public string YAxisLabelFormat { get; set; } = DefaultYAxisLabelFormat;
    [Parameter] public string YAxisMultiplierFormat { get; set; } = DefaultYAxisMultiplierFormat;
    public int PlotAreaX => Padding + YAxisLabelWidth;
    public int PlotAreaY => Padding;
    public int PlotAreaWidth => Width - Padding * 2 - YAxisLabelWidth;
    public int PlotAreaHeight => Height - Padding * 2 - XAxisLabelHeight;

    protected override void OnInitialized() => Chart.SetCanvas(this);

    public void Dispose() => Chart.ResetCanvas();

    public override bool HaveParametersChanged(ParameterView parameters)
        => HasParameterChanged(parameters, nameof(Width), Width)
        || HasParameterChanged(parameters, nameof(Height), Height)
        || HasParameterChanged(parameters, nameof(Padding), Padding)
        || HasParameterChanged(parameters, nameof(XAxisLabelHeight), XAxisLabelHeight)
        || HasParameterChanged(parameters, nameof(XAxisLabelClearance), XAxisLabelClearance)
        || HasParameterChanged(parameters, nameof(YAxisLabelWidth), YAxisLabelWidth)
        || HasParameterChanged(parameters, nameof(YAxisLabelClearance), YAxisLabelClearance)
        || HasParameterChanged(parameters, nameof(YAxisLabelFormat), YAxisLabelFormat)
        || HasParameterChanged(parameters, nameof(YAxisMultiplierFormat), YAxisMultiplierFormat);

    public Shapes.PlotAreaShape GetPlotAreaShape() => new(Width, Height, PlotAreaX, PlotAreaY, PlotAreaWidth, PlotAreaHeight);
}
