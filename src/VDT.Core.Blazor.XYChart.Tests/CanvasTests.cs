using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Xunit;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class CanvasTests {
    [Theory]
    [InlineData(1000, 500, 10, 100, 100, "#", "x #", false)]
    [InlineData(900, 500, 10, 100, 100, "#", "x #", true)]
    [InlineData(1000, 600, 10, 100, 100, "#", "x #", true)]
    [InlineData(1000, 500, 20, 100, 100, "#", "x #", true)]
    [InlineData(1000, 500, 10, 75, 100, "#", "x #", true)]
    [InlineData(1000, 500, 10, 100, 75, "#", "x #", true)]
    [InlineData(1000, 500, 10, 100, 100, "#.##", "x #", true)]
    [InlineData(1000, 500, 10, 100, 100, "#", "x #.##", true)]
    public void HaveParametersChanged(int width, int height, int padding, int xAxisLabelHeight, int yAxisLabelWidth, string yAxisLabelFormat, string yAxisMultiplierFormat, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(Canvas.Width), width },
            { nameof(Canvas.Height), height },
            { nameof(Canvas.Padding), padding },
            { nameof(Canvas.XAxisLabelHeight), xAxisLabelHeight },
            { nameof(Canvas.YAxisLabelWidth), yAxisLabelWidth },
            { nameof(Canvas.YAxisLabelFormat), yAxisLabelFormat },
            { nameof(Canvas.YAxisMultiplierFormat), yAxisMultiplierFormat }
        });

        var subject = new Canvas {
            Width = 1000,
            Height = 500,
            Padding = 10,
            XAxisLabelHeight = 100,
            YAxisLabelWidth = 100,
            YAxisLabelFormat = "#",
            YAxisMultiplierFormat = "x #"
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    [Fact]
    public void GetPlotAreaShape() {
        var subject = new XYChartBuilder()
            .Chart
            .Canvas;

        var result = subject.GetPlotAreaShape();

        Assert.Equal(PlotArea_X, result.X);
        Assert.Equal(PlotArea_Y, result.Y);
        Assert.Equal(PlotArea_Width, result.Width);
        Assert.Equal(PlotArea_Height, result.Height);
    }
}
