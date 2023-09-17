namespace VDT.Core.Blazor.XYChart.Tests;

public static class Constants {
    public const int CanvasWidth = 900;
    public const int CanvasHeight = 500;
    public const int CanvasPadding = 25;
    public const int CanvasXAxisLabelHeight = 50;
    public const int CanvasXAxisLabelClearance = 5;
    public const int CanvasYAxisLabelWidth = 100;
    public const int CanvasYAxisLabelClearance = 10;

    public const int PlotAreaX = CanvasPadding + CanvasYAxisLabelWidth;
    public const int PlotAreaY = CanvasPadding;
    public const int PlotAreaWidth = CanvasWidth - CanvasPadding * 2 - CanvasYAxisLabelWidth;
    public const int PlotAreaHeight = CanvasHeight - CanvasPadding * 2 - CanvasXAxisLabelHeight;

    public const decimal PlotAreaMin = -100M;
    public const decimal PlotAreaMax = 500M;
    public const decimal PlotAreaRange = PlotAreaMax - PlotAreaMin;
    public const decimal PlotAreaGridLineInterval = 200M;
}
