namespace VDT.Core.Blazor.XYChart.Tests;

public static class Constants {
    public const int LabelCount = 5;
    public const DataPointSpacingMode SpacingMode = DataPointSpacingMode.Auto;

    public const int CanvasWidth = 900;
    public const int CanvasHeight = 500;
    public const int CanvasPadding = 25;
    public const int CanvasXAxisLabelHeight = 50;
    public const int CanvasXAxisLabelClearance = 5;
    public const int CanvasYAxisLabelWidth = 100;
    public const int CanvasYAxisLabelClearance = 10;
    public const string CanvasYAxisLabelFormat = "0.00";
    public const string CanvasYAxisMultiplierFormat = "x 0.00";
    public const string CanvasDataLabelFormat = "0.00";
    public const LegendPosition Position = LegendPosition.Top;
    public const int LegendHeight = 25;

    public const int PlotAreaX = CanvasPadding + CanvasYAxisLabelWidth;
    public const int PlotAreaY = CanvasPadding + LegendHeight;
    public const int PlotAreaWidth = CanvasWidth - CanvasPadding * 2 - CanvasYAxisLabelWidth;
    public const int PlotAreaHeight = CanvasHeight - CanvasPadding * 2 - CanvasXAxisLabelHeight - LegendHeight;

    public const decimal PlotAreaMin = -120M;
    public const decimal PlotAreaMax = 480M;
    public const decimal PlotAreaRange = PlotAreaMax - PlotAreaMin;
    public const decimal PlotAreaGridLineInterval = 200M;
    public const decimal PlotAreaMultiplier = 1M;

    public const bool AutoScaleSettingsIsEnabled = true;
    public const int AutoScaleSettingsRequestedGridLineCount = 11;
    public const bool AutoScaleSettingsIncludeZero = false;
    public const decimal AutoScaleSettingsClearancePercentage = 5M;
}
