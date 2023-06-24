﻿using BlazorPlayground.Chart.Shapes;
using System.Linq;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class XYChartTests {
    [Fact]
    public void AutoScale() {
        var subject = new XYChart() {
            DataSeries = {
                new("Foo") { -9, 0 },
                new("Bar") {-5, 19 }
            }
        };

        subject.AutoScale();

        Assert.Equal(-10, subject.PlotArea.Min);
        Assert.Equal(20, subject.PlotArea.Max);
        Assert.Equal(5, subject.PlotArea.GridLineInterval);
    }

    [Fact]
    public void GetShapes_PlotAreaShape() {
        var subject = new XYChart();

        Assert.Single(subject.GetShapes(), shape => shape is PlotAreaShape);
    }

    [Fact]
    public void GetShapes_GridLineShapes() {
        var subject = new XYChart();

        Assert.Contains(subject.GetShapes(), shape => shape is GridLineShape);
    }

    [Fact]
    public void GetGridLineShapes() {
        var subject = new XYChart() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 75,
                YAxisLabelClearance = 10
            },
            PlotArea = {
                 Min = -100,
                 Max = 500,
                 GridLineInterval = 200
            }
        };

        var gridLines = subject.GetGridLineShapes();

        Assert.Equal(3, gridLines.Count());

        Assert.All(gridLines, gridLine => {
            Assert.Equal(25 + 75, gridLine.X);
            Assert.Equal(1000 - 25 - 25 - 75, gridLine.Width);
        });

        Assert.Single(gridLines, gridLine => gridLine.Y == 25 + (0 - -100) / 600.0 * (500 - 25 - 25 - 50));
        Assert.Single(gridLines, gridLine => gridLine.Y == 25 + (200 - -100) / 600.0 * (500 - 25 - 25 - 50));
        Assert.Single(gridLines, gridLine => gridLine.Y == 25 + (400 - 100) / 600.0 * (500 - 25 - 25 - 50));
    }

    [Fact]
    public void MapToPlotArea() {
        var subject = new XYChart() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 75,
                YAxisLabelClearance = 10
            },
            PlotArea = {
                 Min = -100,
                 Max = 500
            }
        };

        Assert.Equal(100, subject.MapToPlotArea(50));
    }
}
