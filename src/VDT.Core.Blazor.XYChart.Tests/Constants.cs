namespace VDT.Core.Blazor.XYChart.Tests;

public static class Constants {
    public const int Chart_LabelCount = 5;
    public const DataPointSpacingMode Chart_DataPointSpacingMode = DataPointSpacingMode.Auto;

    public const int Canvas_Width = 900;
    public const int Canvas_Height = 500;
    public const int Canvas_Padding = 25;
    public const int Canvas_XAxisLabelHeight = 50;
    public const int Canvas_YAxisLabelWidth = 100;
    public const string Canvas_YAxisLabelFormat = "0.00";
    public const string Canvas_YAxisMultiplierFormat = "x 0.00";
    public const string Canvas_DataLabelFormat = "0.00";

    public const bool Legend_IsEnabled = true;
    public const LegendPosition Legend_Position = LegendPosition.Top;
    public const LegendAlignment Legend_Alignment = LegendAlignment.Center;
    public const int Legend_Height = 25;
    public const int Legend_ItemWidth = 100;
    public const int Legend_ItemHeight = 25;
    public const int Legend_KeySize = 16;

    public const int PlotArea_X = Canvas_Padding + Canvas_YAxisLabelWidth;
    public const int PlotArea_Y = Canvas_Padding + Legend_Height;
    public const int PlotArea_Width = Canvas_Width - Canvas_Padding * 2 - Canvas_YAxisLabelWidth;
    public const int PlotArea_Height = Canvas_Height - Canvas_Padding * 2 - Canvas_XAxisLabelHeight - Legend_Height;

    public const decimal PlotArea_Min = -120M;
    public const decimal PlotArea_Max = 480M;
    public const decimal PlotArea_Range = PlotArea_Max - PlotArea_Min;
    public const decimal PlotArea_GridLineInterval = 200M;
    public const decimal PlotArea_Multiplier = 1M;

    public const bool AutoScaleSettings_IsEnabled = true;
    public const int AutoScaleSettings_RequestedGridLineCount = 11;
    public const bool AutoScaleSettings_IncludeZero = false;
    public const decimal AutoScaleSettings_ClearancePercentage = 5M;
}
