using Microsoft.AspNetCore.Components;
using System;

namespace VDT.Core.Blazor.XYChart;

public class Canvas : ChildComponentBase, IDisposable {
    public static int DefaultWidth { get; set; } = 1200;
    public static int DefaultHeight { get; set; } = 500;
    public static int DefaultPadding { get; set; } = 25;
    public static int DefaultXAxisLabelHeight { get; set; } = 50;
    public static int DefaultXAxisLabelClearance { get; set; } = 10;
    public static int DefaultYAxisLabelWidth { get; set; } = 75;
    public static int DefaultYAxisLabelClearance { get; set; } = 10;
    public static string DefaultYAxisLabelFormat { get; set; } = "#,##0.######";
    public static string DefaultYAxisMultiplierFormat { get; set; } = "x #,##0.######";
    public static string DefaultDataLabelFormat { get; set; } = "#,##0.######";
    public static LegendPosition DefaultLegendPosition { get; set; } = LegendPosition.None;
    public static decimal DefaultLegendHeight { get; set; } = 25;

    [Parameter] public int Width { get; set; } = DefaultWidth;
    [Parameter] public int Height { get; set; } = DefaultHeight;
    [Parameter] public int Padding { get; set; } = DefaultPadding;
    [Parameter] public int XAxisLabelHeight { get; set; } = DefaultXAxisLabelHeight;
    [Parameter] public int XAxisLabelClearance { get; set; } = DefaultXAxisLabelClearance;
    [Parameter] public int YAxisLabelWidth { get; set; } = DefaultYAxisLabelWidth;
    [Parameter] public int YAxisLabelClearance { get; set; } = DefaultYAxisLabelClearance;
    [Parameter] public string YAxisLabelFormat { get; set; } = DefaultYAxisLabelFormat;
    [Parameter] public string YAxisMultiplierFormat { get; set; } = DefaultYAxisMultiplierFormat;
    [Parameter] public string DataLabelFormat { get; set; } = DefaultDataLabelFormat;
    [Parameter] public LegendPosition LegendPosition { get; set; } = DefaultLegendPosition;
    [Parameter] public decimal LegendHeight { get; set; } = DefaultLegendHeight;
    public int PlotAreaX => Padding + YAxisLabelWidth;
    public int PlotAreaY => Padding;
    public int PlotAreaWidth => Width - Padding * 2 - YAxisLabelWidth;
    public int PlotAreaHeight => Height - Padding * 2 - XAxisLabelHeight;

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
        || parameters.HasParameterChanged(XAxisLabelClearance)
        || parameters.HasParameterChanged(YAxisLabelWidth)
        || parameters.HasParameterChanged(YAxisLabelClearance)
        || parameters.HasParameterChanged(YAxisLabelFormat)
        || parameters.HasParameterChanged(YAxisMultiplierFormat)
        || parameters.HasParameterChanged(LegendPosition)
        || parameters.HasParameterChanged(LegendHeight);

    public Shapes.PlotAreaShape GetPlotAreaShape() => new(Width, Height, PlotAreaX, PlotAreaY, PlotAreaWidth, PlotAreaHeight);
}
