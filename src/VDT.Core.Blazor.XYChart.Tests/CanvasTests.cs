﻿using Microsoft.AspNetCore.Components;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class CanvasTests {
    [Theory]
    [InlineData(1000, 500, 10, 100, 100, "#", "x #", "#", false, false)]
    [InlineData(900, 500, 10, 100, 100, "#", "x #", "#", false, true)]
    [InlineData(1000, 600, 10, 100, 100, "#", "x #", "#", false, true)]
    [InlineData(1000, 500, 20, 100, 100, "#", "x #", "#", false, true)]
    [InlineData(1000, 500, 10, 75, 100, "#", "x #", "#", false, true)]
    [InlineData(1000, 500, 10, 100, 75, "#", "x #", "#", false, true)]
    [InlineData(1000, 500, 10, 100, 100, "#.##", "x #", "#", false, true)]
    [InlineData(1000, 500, 10, 100, 100, "#", "x #.##", "#", false, true)]
    [InlineData(1000, 500, 10, 100, 100, "#", "x #", "#.##", false, true)]
    [InlineData(1000, 500, 10, 100, 100, "#", "x #", "#", true, true)]
    public void HaveParametersChanged(int width, int height, int padding, int xAxisLabelHeight, int yAxisLabelWidth, string yAxisLabelFormat, string yAxisMultiplierFormat, string dataLabelFormat, bool autoSizeLabelsIsEnabled, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(Canvas.Width), width },
            { nameof(Canvas.Height), height },
            { nameof(Canvas.Padding), padding },
            { nameof(Canvas.XAxisLabelHeight), xAxisLabelHeight },
            { nameof(Canvas.YAxisLabelWidth), yAxisLabelWidth },
            { nameof(Canvas.YAxisLabelFormat), yAxisLabelFormat },
            { nameof(Canvas.YAxisMultiplierFormat), yAxisMultiplierFormat },
            { nameof(Canvas.DataLabelFormat), dataLabelFormat},
            { nameof(Canvas.AutoSizeLabelsIsEnabled), autoSizeLabelsIsEnabled }
        });

        var subject = new Canvas {
            Width = 1000,
            Height = 500,
            Padding = 10,
            XAxisLabelHeight = 100,
            YAxisLabelWidth = 100,
            YAxisLabelFormat = "#",
            YAxisMultiplierFormat = "x #",
            DataLabelFormat = "#",
            AutoSizeLabelsIsEnabled = false
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
            .WithCanvas(autoSizeLabelsIsEnabled: false)
            .Chart
            .Canvas;

        await subject.AutoSize();

        Assert.Equal(Canvas_XAxisLabelHeight, subject.ActualXAxisLabelHeight);
        Assert.Equal(Canvas_YAxisLabelWidth, subject.ActualYAxisLabelWidth);
    }

    [Fact]
    public async Task AutoSize() {
        const int expectedHeight = 75;
        const int expectedWidth = 125;

        var builder = new XYChartBuilder()
            .WithCanvas(autoSizeLabelsIsEnabled: true);

        builder.SizeProvider.GetTextSize(Arg.Any<string>(), XAxisLabelShape.DefaultCssClass).Returns(new TextSize(0, expectedHeight));
        builder.SizeProvider.GetTextSize(Arg.Any<string>(), YAxisLabelShape.DefaultCssClass).Returns(new TextSize(expectedWidth, 0));

        var subject = builder
            .Chart
            .Canvas;

        await subject.AutoSize();

        Assert.Equal(expectedHeight, subject.ActualXAxisLabelHeight);
        Assert.Equal(expectedWidth, subject.ActualYAxisLabelWidth);
    }
}
