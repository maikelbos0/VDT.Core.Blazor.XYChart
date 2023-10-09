using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class BarLayerTests {
    [Theory]
    [MemberData(nameof(HaveParametersChanged_Data))]
    public void HaveParametersChanged(bool isStacked, bool showDataLabels, decimal clearancePercentage, decimal gapPercentage, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(BarLayer.IsStacked), isStacked },
            { nameof(BarLayer.ShowDataLabels), showDataLabels },
            { nameof(BarLayer.ClearancePercentage), clearancePercentage },
            { nameof(BarLayer.GapPercentage), gapPercentage }
        });

        var subject = new BarLayer {
            IsStacked = false,
            ShowDataLabels = false,
            ClearancePercentage = 20M,
            GapPercentage = 20M
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<bool, bool, decimal, decimal, bool> HaveParametersChanged_Data() => new() {
        { false, false, 20M, 20M, false },
        { true, false, 20M, 20M, true },
        { false, true, 20M, 20M, true },
        { false, false, 20M, 30M, true },
        { false, false, 30M, 20M, true }
    };

    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_Data))]
    public void GetUnstackedDataSeriesShapes(int dataSeriesIndex, int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedWidth, decimal expectedHeight) {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .WithLayer(new BarLayer() {
                ClearancePercentage = 25M,
                GapPercentage = 10M,
                IsStacked = false
            })
            .WithDataSeries(new DataSeries() {
                Color = "blue",
                DataPoints = { -60M, -60M, 60M, 150M },
                CssClass = "example-data"
            })
            .WithDataSeries(new DataSeries() {
                Color = "red",
                DataPoints = { null, null, null, 150M },
                CssClass = "example-data"
            })
            .Chart.Layers.Single();

        subject.DataSeries[dataSeriesIndex].DataPoints[index] = dataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<BarDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(BarDataShape)}[0,{dataSeriesIndex},{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedWidth, shape.Width);
        Assert.Equal(expectedHeight, shape.Height);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
        Assert.Equal("data bar-data example-data", shape.CssClass);
    }

    public static TheoryData<int, int, decimal, decimal, decimal, decimal, decimal> GetUnstackedDataSeriesShapes_Data() {
        var dataPointWidth = PlotAreaWidth / 3M;

        return new() {
            { 0, 0, -30M, PlotAreaX + (0.5M - 0.25M) * dataPointWidth, PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight, 0.2M * dataPointWidth, -30M / PlotAreaRange * PlotAreaHeight },
            { 1, 1, 30M, PlotAreaX + (1.5M + 0.1M / 2) * dataPointWidth, PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight, 0.2M * dataPointWidth, 30M / PlotAreaRange * PlotAreaHeight },
            { 0, 2, 210M, PlotAreaX + (2.5M - 0.25M) * dataPointWidth, PlotAreaY + (PlotAreaMax - 210M) / PlotAreaRange * PlotAreaHeight, 0.2M * dataPointWidth, 210M / PlotAreaRange * PlotAreaHeight },
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_Data))]
    public void GetStackedDataSeriesShapes(int dataSeriesIndex, int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedWidth, decimal expectedHeight) {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .WithLayer(new BarLayer() {
                ClearancePercentage = 25M,
                GapPercentage = 10M,
                IsStacked = true
            })
            .WithDataSeries(new DataSeries() {
                Color = "blue",
                DataPoints = { -60M, -60M, 60M, 150M },
                CssClass = "example-data"
            })
            .WithDataSeries(new DataSeries() {
                Color = "red",
                DataPoints = { null, null, null, 150M },
                CssClass = "example-data"
            })
            .Chart.Layers.Single();

        subject.DataSeries[dataSeriesIndex].DataPoints[index] = dataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<BarDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(BarDataShape)}[0,{dataSeriesIndex},{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedWidth, shape.Width);
        Assert.Equal(expectedHeight, shape.Height);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
        Assert.Equal("data bar-data example-data", shape.CssClass);
    }

    public static TheoryData<int, int, decimal, decimal, decimal, decimal, decimal> GetStackedDataSeriesShapes_Data() {
        var dataPointWidth = PlotAreaWidth / 3M;

        return new() {
            { 0, 0, -30M, PlotAreaX + (0.5M - 0.25M) * dataPointWidth, PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight, 0.5M * dataPointWidth, -30M / PlotAreaRange * PlotAreaHeight },
            { 0, 2, 30M, PlotAreaX + (2.5M - 0.25M) * dataPointWidth, PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight, 0.5M * dataPointWidth, 30M / PlotAreaRange * PlotAreaHeight },
            { 1, 0, -30M, PlotAreaX + (0.5M - 0.25M) * dataPointWidth, PlotAreaY + (PlotAreaMax + 90M) / PlotAreaRange * PlotAreaHeight, 0.5M * dataPointWidth, -30M / PlotAreaRange * PlotAreaHeight },
            { 1, 0, 30M, PlotAreaX + (0.5M - 0.25M) * dataPointWidth, PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight, 0.5M * dataPointWidth, 30M / PlotAreaRange * PlotAreaHeight },
            { 1, 2, -30M, PlotAreaX + (2.5M - 0.25M) * dataPointWidth, PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight, 0.5M * dataPointWidth, -30M / PlotAreaRange * PlotAreaHeight },
            { 1, 2, 30M, PlotAreaX + (2.5M - 0.25M) * dataPointWidth, PlotAreaY + (PlotAreaMax - 90M) / PlotAreaRange * PlotAreaHeight, 0.5M * dataPointWidth, 30M / PlotAreaRange * PlotAreaHeight }
        };
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GetDataSeriesShapes_Empty(bool isStacked) {
        var subject = new XYChartBuilder()
            .WithLayer(new BarLayer() {
                IsStacked = isStacked
            })
            .Chart.Layers.Single();

        Assert.Empty(subject.GetDataSeriesShapes());
    }
}
