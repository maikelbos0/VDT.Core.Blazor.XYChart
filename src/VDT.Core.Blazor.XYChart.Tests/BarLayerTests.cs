using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class BarLayerTests {
    [Theory]
    [MemberData(nameof(HaveParametersChanged_Data))]
    public void HaveParametersChanged(bool isStacked, decimal clearancePercentage, decimal gapPercentage, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(BarLayer.IsStacked), isStacked },
            { nameof(BarLayer.ClearancePercentage), clearancePercentage },
            { nameof(BarLayer.GapPercentage), gapPercentage }
        });

        var subject = new BarLayer {
            IsStacked = false,
            ClearancePercentage = 20M,
            GapPercentage = 20M
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<bool, decimal, decimal, bool> HaveParametersChanged_Data() => new() {
        { false, 20M, 20M, false },
        { true, 20M, 20M, true },
        { false, 20M, 30M, true },
        { false, 30M, 20M, true }
    };

    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_Data))]
    public void GetUnstackedDataSeriesShapes(int dataSeriesIndex, int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedWidth, decimal expectedHeight) {
        var subject = new BarLayer() {
            Chart = new() {
                Canvas = {
                    Width = 900,
                    Height = 500,
                    Padding = 25,
                    XAxisLabelHeight = 50,
                    XAxisLabelClearance = 5,
                    YAxisLabelWidth = 100,
                    YAxisLabelClearance = 10
                },
                PlotArea = {
                     Min = -10M,
                     Max = 40M,
                     GridLineInterval = 10M
                },
                Labels = { "Foo", "Bar", "Baz" },
                DataPointSpacingMode = DataPointSpacingMode.Center
            },
            DataSeries = {
                new() {
                    Color = "blue",
                    DataPoints = { -10M, -10M, 10M, 15M },
                    CssClass = "example-data"
                },
                new() {
                    Color = "red",
                    DataPoints = { null, null, null, 15M },
                    CssClass = "example-data"
                }
            },
            ClearancePercentage = 25M,
            GapPercentage = 10M,
            IsStacked = false
        };

        subject.DataSeries[dataSeriesIndex].DataPoints[index] = dataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<BarDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(BarDataShape)}[{dataSeriesIndex},{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedWidth, shape.Width);
        Assert.Equal(expectedHeight, shape.Height);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
        Assert.Equal("data bar-data example-data", shape.CssClass);
    }

    public static TheoryData<int, int, decimal, decimal, decimal, decimal, decimal> GetUnstackedDataSeriesShapes_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (900 - 25 - 25 - 100) / 3M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaZero = 40M;
        var plotAreaRange = plotAreaZero - -10M;

        return new() {
            { 0, 0, -5M, plotAreaX + (0.5M - 0.25M) * dataPointWidth, plotAreaY + (plotAreaZero + 5M) / plotAreaRange * plotAreaHeight, 0.2M * dataPointWidth, -5M / plotAreaRange * plotAreaHeight },
            { 1, 1, 5M, plotAreaX + (1.5M + 0.1M / 2) * dataPointWidth, plotAreaY + (plotAreaZero - 5M) / plotAreaRange * plotAreaHeight, 0.2M * dataPointWidth, 5M / plotAreaRange * plotAreaHeight },
            { 0, 2, 35M, plotAreaX + (2.5M - 0.25M) * dataPointWidth, plotAreaY + (plotAreaZero - 35M) / plotAreaRange * plotAreaHeight, 0.2M * dataPointWidth, 35M / plotAreaRange * plotAreaHeight },
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_Data))]
    public void GetStackedDataSeriesShapes(int dataSeriesIndex, int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedWidth, decimal expectedHeight) {
        var subject = new BarLayer() {
            Chart = new() {
                Canvas = {
                    Width = 900,
                    Height = 500,
                    Padding = 25,
                    XAxisLabelHeight = 50,
                    XAxisLabelClearance = 5,
                    YAxisLabelWidth = 100,
                    YAxisLabelClearance = 10
                },
                PlotArea = {
                     Min = -20M,
                     Max = 30M,
                     GridLineInterval = 10M
                },
                Labels = { "Foo", "Bar", "Baz" },
                DataPointSpacingMode = DataPointSpacingMode.Center
            },
            DataSeries = {
                new() {
                    Color = "blue",
                    DataPoints = { -10M, -10M, 10M, 15M },
                    CssClass = "example-data"
                },
                new() {
                    Color = "red",
                    DataPoints = { null, null, null, 15M },
                    CssClass = "example-data"
                }
            },
            ClearancePercentage = 25M,
            GapPercentage = 10M,
            IsStacked = true
        };

        subject.DataSeries[dataSeriesIndex].DataPoints[index] = dataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<BarDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(BarDataShape)}[{dataSeriesIndex},{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedWidth, shape.Width);
        Assert.Equal(expectedHeight, shape.Height);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
        Assert.Equal("data bar-data example-data", shape.CssClass);
    }

    public static TheoryData<int, int, decimal, decimal, decimal, decimal, decimal> GetStackedDataSeriesShapes_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (900 - 25 - 25 - 100) / 3M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaZero = 30M;
        var plotAreaRange = plotAreaZero - -20M;

        return new() {
            { 0, 0, -5M, plotAreaX + (0.5M - 0.25M) * dataPointWidth, plotAreaY + (plotAreaZero + 5M) / plotAreaRange * plotAreaHeight, 0.5M * dataPointWidth, -5M / plotAreaRange * plotAreaHeight },
            { 0, 2, 5M, plotAreaX + (2.5M - 0.25M) * dataPointWidth, plotAreaY + (plotAreaZero - 5M) / plotAreaRange * plotAreaHeight, 0.5M * dataPointWidth, 5M / plotAreaRange * plotAreaHeight },
            { 1, 0, -5M, plotAreaX + (0.5M - 0.25M) * dataPointWidth, plotAreaY + (plotAreaZero + 15M) / plotAreaRange * plotAreaHeight, 0.5M * dataPointWidth, -5M / plotAreaRange * plotAreaHeight },
            { 1, 0, 5M, plotAreaX + (0.5M - 0.25M) * dataPointWidth, plotAreaY + (plotAreaZero - 5M) / plotAreaRange * plotAreaHeight, 0.5M * dataPointWidth, 5M / plotAreaRange * plotAreaHeight },
            { 1, 2, -5M, plotAreaX + (2.5M - 0.25M) * dataPointWidth, plotAreaY + (plotAreaZero + 5M) / plotAreaRange * plotAreaHeight, 0.5M * dataPointWidth, -5M / plotAreaRange * plotAreaHeight },
            { 1, 2, 5M, plotAreaX + (2.5M - 0.25M) * dataPointWidth, plotAreaY + (plotAreaZero - 15M) / plotAreaRange * plotAreaHeight, 0.5M * dataPointWidth, 5M / plotAreaRange * plotAreaHeight }
        };
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GetDataSeriesShapes_Empty(bool isStacked) {
        var subject = new BarLayer() {
            Chart = new(),
            IsStacked = isStacked
        };

        Assert.Empty(subject.GetDataSeriesShapes());
    }
}
