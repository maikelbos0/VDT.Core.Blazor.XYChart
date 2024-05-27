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
    [InlineData(false, 1000, 500, 10, false, 100, false, 100, "#", "x #", "#", false)]
    [InlineData(false, 900, 500, 10, false, 100, false, 100, "#", "x #", "#", true)]
    [InlineData(false, 1000, 600, 10, false, 100, false, 100, "#", "x #", "#", true)]
    [InlineData(false, 1000, 500, 20, false, 100, false, 100, "#", "x #", "#", true)]
    [InlineData(false, 1000, 500, 10, true, 100, false, 100, "#", "x #", "#", true)]
    [InlineData(false, 1000, 500, 10, false, 75, false, 100, "#", "x #", "#", true)]
    [InlineData(false, 1000, 500, 10, false, 100, true, 75, "#", "x #", "#", true)]
    [InlineData(false, 1000, 500, 10, false, 100, false, 75, "#", "x #", "#", true)]
    [InlineData(false, 1000, 500, 10, false, 100, false, 100, "#.##", "x #", "#", true)]
    [InlineData(false, 1000, 500, 10, false, 100, false, 100, "#", "x #.##", "#", true)]
    [InlineData(false, 1000, 500, 10, false, 100, false, 100, "#", "x #", "#.##", true)]
    [InlineData(true, 1000, 500, 10, false, 100, false, 100, "#", "x #", "#", true)]
    public void HaveParametersChanged(
        bool autoSizeWidthIsEnabled,
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
            { nameof(Canvas.AutoSizeWidthIsEnabled), autoSizeWidthIsEnabled },
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
            AutoSizeWidthIsEnabled = false,
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

    [Theory]
    [InlineData(false, Canvas_XAxisLabelHeight)]
    [InlineData(true, 75)]
    public async Task AutoSize_XAxisLabels(bool autoSizeXAxisLabelsIsEnabled, int expectedXAxisLabelHeight) {
        var subject = new XYChartBuilder()
            .WithCanvas(autoSizeXAxisLabelsIsEnabled: autoSizeXAxisLabelsIsEnabled)
            .WithProvidedSize(XAxisLabelShape.DefaultCssClass, 0, 10, 0, 64.1M)
            .Chart
            .Canvas;

        await subject.AutoSize();

        Assert.Equal(expectedXAxisLabelHeight, subject.ActualXAxisLabelHeight);
    }

    [Theory]
    [InlineData(false, 1000, Canvas_YAxisLabelWidth)]
    [InlineData(true, 1, 125)]
    [InlineData(true, 1000, 150)]
    public async Task AutoSize_YAxisLabels(bool autoSizeYAxisLabelsIsEnabled, decimal multiplier, int expectedYAxisLabelWidth) {
        var subject = new XYChartBuilder()
            .WithPlotArea(multiplier: multiplier)
            .WithCanvas(autoSizeYAxisLabelsIsEnabled: autoSizeYAxisLabelsIsEnabled)
            .WithProvidedSize(YAxisLabelShape.DefaultCssClass, 10, 0, 114.1M, 0)
            .WithProvidedSize(YAxisMultiplierShape.DefaultCssClass, 4, 0, 20.1M, 0)
            .Chart
            .Canvas;

        await subject.AutoSize();

        Assert.Equal(expectedYAxisLabelWidth, subject.ActualYAxisLabelWidth);
    }
}
