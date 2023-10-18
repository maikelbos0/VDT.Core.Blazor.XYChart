using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Xunit;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class CanvasTests {
    [Theory]
    [InlineData(1000, 500, 10, 100, 10, 100, 10, "#", "x #", LegendPosition.None, 25, 100, 25, false)]
    [InlineData(900, 500, 10, 100, 10, 100, 10, "#", "x #", LegendPosition.None, 25, 100, 25, true)]
    [InlineData(1000, 600, 10, 100, 10, 100, 10, "#", "x #", LegendPosition.None, 25, 100, 25, true)]
    [InlineData(1000, 500, 20, 100, 10, 100, 10, "#", "x #", LegendPosition.None, 25, 100, 25, true)]
    [InlineData(1000, 500, 10, 75, 10, 100, 10, "#", "x #", LegendPosition.None, 25, 100, 25, true)]
    [InlineData(1000, 500, 10, 100, 15, 100, 10, "#", "x #", LegendPosition.None, 25, 100, 25, true)]
    [InlineData(1000, 500, 10, 100, 10, 75, 10, "#", "x #", LegendPosition.None, 25, 100, 25, true)]
    [InlineData(1000, 500, 10, 100, 10, 100, 15, "#", "x #", LegendPosition.None, 25, 100, 25, true)]
    [InlineData(1000, 500, 10, 100, 10, 100, 10, "#.##", "x #", LegendPosition.None, 25, 100, 25, true)]
    [InlineData(1000, 500, 10, 100, 10, 100, 10, "#", "x #.##", LegendPosition.None, 25, 100, 25, true)]
    [InlineData(1000, 500, 10, 100, 10, 100, 10, "#", "x #", LegendPosition.Top, 25, 100, 25, true)]
    [InlineData(1000, 500, 10, 100, 10, 100, 10, "#", "x #", LegendPosition.None, 50, 100, 25, true)]
    [InlineData(1000, 500, 10, 100, 10, 100, 10, "#", "x #", LegendPosition.None, 25, 75, 25, true)]
    [InlineData(1000, 500, 10, 100, 10, 100, 10, "#", "x #", LegendPosition.None, 25, 100, 10, true)]
    public void HaveParametersChanged(int width, int height, int padding, int xAxisLabelHeight, int xAxisLabelClearance, int yAxisLabelWidth, int yAxisLabelClearance, string yAxisLabelFormat, string yAxisMultiplierFormat, LegendPosition legendPosition, int legendHeight, int legendItemWidth, int legendItemHeight, bool expectedResult) {
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
            { nameof(Canvas.LegendHeight), legendHeight },
            { nameof(Canvas.LegendItemWidth), legendItemWidth },
            { nameof(Canvas.LegendItemHeight), legendItemHeight }
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
            LegendHeight = 25,
            LegendItemWidth = 100,
            LegendItemHeight = 25
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
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
