using Microsoft.AspNetCore.Components;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class CanvasTests {
    [Theory]
    [InlineData(1000, 500, 10, false, 100, false, 100, "#", "x #", "#", false)]
    [InlineData(900, 500, 10, false, 100, false, 100, "#", "x #", "#", true)]
    [InlineData(1000, 600, 10, false, 100, false, 100, "#", "x #", "#", true)]
    [InlineData(1000, 500, 20, false, 100, false, 100, "#", "x #", "#", true)]
    [InlineData(1000, 500, 10, true, 100, false, 100, "#", "x #", "#", true)]
    [InlineData(1000, 500, 10, false, 75, false, 100, "#", "x #", "#", true)]
    [InlineData(1000, 500, 10, false, 100, true, 75, "#", "x #", "#", true)]
    [InlineData(1000, 500, 10, false, 100, false, 75, "#", "x #", "#", true)]
    [InlineData(1000, 500, 10, false, 100, false, 100, "#.##", "x #", "#", true)]
    [InlineData(1000, 500, 10, false, 100, false, 100, "#", "x #.##", "#", true)]
    [InlineData(1000, 500, 10, false, 100, false, 100, "#", "x #", "#.##", true)]
    public void HaveParametersChanged(
        int width,
        int height,
        int padding,
        bool autoSizeXAxisLabelsIsEnabled,
        int xAxisLabelHeight,
        bool autoSizeYAxisLabelsIsEnabled,
        int yAxisLabelWidth,
        string yAxisLabelFormat,
        string yAxisMultiplierFormat,
        string dataLabelFormat,
        bool expectedResult
    ) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(Canvas.Width), width },
            { nameof(Canvas.Height), height },
            { nameof(Canvas.Padding), padding },
            { nameof(Canvas.AutoSizeXAxisLabelsIsEnabled), autoSizeXAxisLabelsIsEnabled },
            { nameof(Canvas.XAxisLabelHeight), xAxisLabelHeight },
            { nameof(Canvas.AutoSizeYAxisLabelsIsEnabled), autoSizeYAxisLabelsIsEnabled },
            { nameof(Canvas.YAxisLabelWidth), yAxisLabelWidth },
            { nameof(Canvas.YAxisLabelFormat), yAxisLabelFormat },
            { nameof(Canvas.YAxisMultiplierFormat), yAxisMultiplierFormat },
            { nameof(Canvas.DataLabelFormat), dataLabelFormat}
        });

        var subject = new Canvas {
            Width = 1000,
            Height = 500,
            Padding = 10,
            AutoSizeXAxisLabelsIsEnabled = false,
            XAxisLabelHeight = 100,
            AutoSizeYAxisLabelsIsEnabled = false,
            YAxisLabelWidth = 100,
            YAxisLabelFormat = "#",
            YAxisMultiplierFormat = "x #",
            DataLabelFormat = "#"
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

    [Fact]
    public async Task AutoSize_Disabled() {
        var subject = new XYChartBuilder()
            .WithCanvas(autoSizeXAxisLabelsIsEnabled: false)
            .Chart
            .Canvas;

        await subject.AutoSize();

        Assert.Equal(Canvas_XAxisLabelHeight, subject.ActualXAxisLabelHeight);
        Assert.Equal(Canvas_YAxisLabelWidth, subject.ActualYAxisLabelWidth);
    }

    [Theory]
    [InlineData(1, 125)]
    [InlineData(1000, 150)]
    public async Task AutoSize(decimal multiplier, int expectedWidth) {
        var builder = new XYChartBuilder()
            .WithPlotArea(multiplier: multiplier)
            .WithCanvas(autoSizeXAxisLabelsIsEnabled: true);

        builder.SizeProvider.GetTextSize(Arg.Any<string>(), XAxisLabelShape.DefaultCssClass).Returns(new TextSize(0, 74.1M));
        builder.SizeProvider.GetTextSize(Arg.Any<string>(), YAxisLabelShape.DefaultCssClass).Returns(new TextSize(124.1M, 0));
        builder.SizeProvider.GetTextSize(Arg.Any<string>(), YAxisMultiplierShape.DefaultCssClass).Returns(new TextSize(24.1M, 0));

        var subject = builder
            .Chart
            .Canvas;

        await subject.AutoSize();

        Assert.Equal(75, subject.ActualXAxisLabelHeight);
        Assert.Equal(expectedWidth, subject.ActualYAxisLabelWidth);
    }
}
