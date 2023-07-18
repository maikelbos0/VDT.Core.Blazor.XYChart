﻿using BlazorPlayground.Chart.Shapes;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class BarDataSeriesLayerTests {
    [Theory]
    [MemberData(nameof(GetDataSeriesShapesData))]
    public void GetDataSeriesShapes(int dataSeriesIndex, int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedWidth, decimal expectedHeight) {
        var subject = new BarDataSeriesLayer(
            new XYChart() {
                Canvas = {
                    Width = 1000,
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
                Labels = { "Foo", "Bar", "Baz", "Quux" }
            }
        ) {
            DataSeries = {
                new("Foo", "red") { null, null, null, null, 15M },
                new("Bar", "red") { null, null, null, null, 15M }
            },
            ClearancePercentage = 25M,
            GapPercentage = 10M
        };
        
        subject.DataSeries[dataSeriesIndex][index] = dataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<BarDataShape>(Assert.Single(result));

        Assert.Equal("red", shape.Color);
        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedWidth, shape.Width);
        Assert.Equal(expectedHeight, shape.Height);
        Assert.EndsWith($"[{dataSeriesIndex},{index}]", shape.Key);
    }

    public static TheoryData<int, int, decimal, decimal, decimal, decimal, decimal> GetDataSeriesShapesData() => new() {
        { 0, 0, -5M, 25M + 100M + (0.5M - 0.25M) * 850M / 4M, 25M + 40M / 50M * 400M, 850M / 4M * 0.2M, 5M / 50M * 400M },
        { 1, 1, 5M, 25M + 100M + (1.5M + 0.1M / 2) * 850M / 4M, 25M + (40M - 5M) / 50M * 400M, 850M / 4M * 0.2M, 5M / 50M * 400M },
        { 0, 3, 35M, 25M + 100M + (3.5M - 0.25M) * 850M / 4M, 25M + (40M - 35M) / 50M * 400M, 850M / 4M * 0.2M, 35M / 50M * 400M },
    };
}
