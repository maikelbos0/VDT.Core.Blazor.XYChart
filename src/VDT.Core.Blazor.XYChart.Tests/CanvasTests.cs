using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Xunit;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class CanvasTests {
    [Theory]
    [MemberData(nameof(HaveParametersChanged_Data))]
    public void HaveParametersChanged(int width, int height, int padding, int xAxisLabelHeight, int xAxisLabelClearance, int yAxisLabelWidth, int yAxisLabelClearance, string yAxisLabelFormat, string yAxisMultiplierFormat, LegendPosition legendPosition, decimal legendHeight, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(Canvas.Width), width },
            { nameof(Canvas.Height), height },
            { nameof(Canvas.Padding), padding },
            { nameof(Canvas.XAxisLabelHeight), xAxisLabelHeight },
            { nameof(Canvas.XAxisLabelClearance), xAxisLabelClearance },
            { nameof(Canvas.YAxisLabelWidth), yAxisLabelWidth },
            { nameof(Canvas.YAxisLabelClearance), yAxisLabelClearance },
            { nameof(Canvas.YAxisLabelFormat), yAxisLabelFormat },
            { nameof(Canvas.YAxisMultiplierFormat), yAxisMultiplierFormat },
            { nameof(Canvas.LegendPosition), legendPosition },
            { nameof(Canvas.LegendHeight), legendHeight }
        });

        var subject = new Canvas {
            Width = 1000,
            Height = 500,
            Padding = 10,
            XAxisLabelHeight = 100,
            XAxisLabelClearance = 10,
            YAxisLabelWidth = 100,
            YAxisLabelClearance = 10,
            YAxisLabelFormat = "#",
            YAxisMultiplierFormat = "x #",
            LegendPosition = LegendPosition.None,
            LegendHeight = 25M
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static HaveParametersChangedTheoryData HaveParametersChanged_Data() => new() {
        { 1000, 500, 10, 100, 10, 100, 10, "#", "x #", LegendPosition.None, 25M, false },
        { 1100, 500, 10, 100, 10, 100, 10, "#", "x #", LegendPosition.None, 25M, true },
        { 1000, 600, 10, 100, 10, 100, 10, "#", "x #", LegendPosition.None, 25M, true },
        { 1000, 500, 20, 100, 10, 100, 10, "#", "x #", LegendPosition.None, 25M, true },
        { 1000, 500, 10, 125, 10, 100, 10, "#", "x #", LegendPosition.None, 25M, true },
        { 1000, 500, 10, 100, 20, 100, 10, "#", "x #", LegendPosition.None, 25M, true },
        { 1000, 500, 10, 100, 10, 125, 10, "#", "x #", LegendPosition.None, 25M, true },
        { 1000, 500, 10, 100, 10, 100, 20, "#", "x #", LegendPosition.None, 25M, true },
        { 1000, 500, 10, 100, 10, 100, 10, "#.##", "x #", LegendPosition.None, 25M, true },
        { 1000, 500, 10, 100, 10, 100, 10, "#", "x #.##", LegendPosition.None, 25M, true },
        { 1000, 500, 10, 100, 10, 100, 10, "#", "x #", LegendPosition.Top, 25M, true },
        { 1000, 500, 10, 100, 10, 100, 10, "#", "x #", LegendPosition.None, 50M, true }
    };

    public class HaveParametersChangedTheoryData : TheoryData {
        public void Add(int width, int height, int padding, int xAxisLabelHeight, int xAxisLabelClearance, int yAxisLabelWidth, int yAxisLabelClearance, string yAxisLabelFormat, string yAxisMultiplierFormat, LegendPosition legendPosition, decimal legendHeight, bool expectedResult) {
            AddRow(width, height, padding, xAxisLabelHeight, xAxisLabelClearance, yAxisLabelWidth, yAxisLabelClearance, yAxisLabelFormat, yAxisMultiplierFormat, legendPosition, legendHeight, expectedResult);
        }
    }

    [Fact]
    public void GetPlotAreaShape() {
        var subject = new XYChartBuilder()
            .Chart
            .Canvas;

        var result = subject.GetPlotAreaShape();

        Assert.Equal(PlotAreaX, result.X);
        Assert.Equal(PlotAreaY, result.Y);
        Assert.Equal(PlotAreaWidth, result.Width);
        Assert.Equal(PlotAreaHeight, result.Height);
    }
}
