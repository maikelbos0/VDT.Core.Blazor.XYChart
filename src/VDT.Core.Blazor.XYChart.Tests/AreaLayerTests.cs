using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class AreaLayerTests {
    [Theory]
    [InlineData(false, false)]
    [InlineData(true, true)]
    public void HaveParametersChanged(bool isStacked, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(AreaLayer.IsStacked), isStacked }
        });

        var subject = new AreaLayer {
            IsStacked = false
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_Data))]
    public void GetUnstackedDataSeriesShapes(int startIndex, decimal startDataPoint, int endIndex, decimal endDataPoint, string expectedPath) {
        var subject = new AreaLayer() {
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
                DataPointSpacingMode = DataPointSpacingMode.EdgeToEdge
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
            IsStacked = false
        };

        subject.DataSeries[1].DataPoints[startIndex] = startDataPoint;
        subject.DataSeries[1].DataPoints[endIndex] = endDataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<AreaDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(AreaDataShape)}[1]"));

        Assert.Equal(expectedPath, shape.Path);
        Assert.Equal("red", shape.Color);
        Assert.Equal("data area-data example-data", shape.CssClass);
    }

    public static TheoryData<int, decimal, int, decimal, string> GetUnstackedDataSeriesShapes_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (900 - 25 - 25 - 100) / 2M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaZero = 40M;
        var plotAreaRange = plotAreaZero - -10M;

        return new() {
            { 0, 5M, 1, -5M, Helpers.Path(
                $"M {plotAreaX} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX} {plotAreaY + (plotAreaZero - 5M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + dataPointWidth} {plotAreaY + (plotAreaZero + 5M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M * dataPointWidth} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M * dataPointWidth} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"Z"
            ) },
            { 1, -5M, 2, 5M, Helpers.Path(
                $"M {plotAreaX} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + dataPointWidth} {plotAreaY + (plotAreaZero + 5M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M * dataPointWidth} {plotAreaY + (plotAreaZero - 5M)  / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M * dataPointWidth} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"Z"
            ) }
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_Data))]
    public void GetStackedDataSeriesShapes(int dataSeriesIndex, int startIndex, decimal startDataPoint, int endIndex, decimal endDataPoint, string expectedPath) {
        var subject = new AreaLayer() {
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
                DataPointSpacingMode = DataPointSpacingMode.EdgeToEdge
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
            IsStacked = true
        };

        subject.DataSeries[dataSeriesIndex].DataPoints[startIndex] = startDataPoint;
        subject.DataSeries[dataSeriesIndex].DataPoints[endIndex] = endDataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<AreaDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(AreaDataShape)}[{dataSeriesIndex}]"));

        Assert.Equal(expectedPath, shape.Path);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
        Assert.Equal("data area-data example-data", shape.CssClass);
    }

    public static TheoryData<int, int, decimal, int, decimal, string> GetStackedDataSeriesShapes_Data() {
        var plotAreaX = 25 + 100;
        var plotAreaY = 25;
        var dataPointWidth = (900 - 25 - 25 - 100) / 2M;
        var plotAreaHeight = 500 - 25 - 25 - 50;
        var plotAreaZero = 30M;
        var plotAreaRange = plotAreaZero - -20M;

        return new() {
            { 0, 0, 5M, 1, 5M, Helpers.Path(
                $"M {plotAreaX} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX} {plotAreaY + (plotAreaZero - 5M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + dataPointWidth} {plotAreaY + (plotAreaZero - 5M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M * dataPointWidth} {plotAreaY + (plotAreaZero - 10M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M * dataPointWidth} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"Z"
            ) },
            { 1, 0, 5M, 1, -5M, Helpers.Path(
                $"M {plotAreaX} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX} {plotAreaY + (plotAreaZero + 5M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + dataPointWidth} {plotAreaY + (plotAreaZero + 15M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M  * dataPointWidth} {plotAreaY + (plotAreaZero - 10M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M * dataPointWidth} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"Z"
            ) },
            { 1, 0, -5M, 1, 5M, Helpers.Path(
                $"M {plotAreaX} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX} {plotAreaY + (plotAreaZero + 15M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + dataPointWidth} {plotAreaY + (plotAreaZero + 5M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M * dataPointWidth} {plotAreaY + (plotAreaZero - 10M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M * dataPointWidth} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"Z"
            ) },
            { 1, 1, 5M, 2, -5M, Helpers.Path(
                $"M {plotAreaX} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX} {plotAreaY + (plotAreaZero + 10M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + dataPointWidth} {plotAreaY + (plotAreaZero + 5M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M * dataPointWidth} {plotAreaY + (plotAreaZero - 5M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M * dataPointWidth} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"Z"
            ) },
            { 1, 1, -5M, 2, 5M, Helpers.Path(
                $"M {plotAreaX} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX} {plotAreaY + (plotAreaZero + 10M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + dataPointWidth} {plotAreaY + (plotAreaZero + 15M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M * dataPointWidth} {plotAreaY + (plotAreaZero - 15M) / plotAreaRange * plotAreaHeight}",
                $"L {plotAreaX + 2M * dataPointWidth} {plotAreaY + plotAreaZero / plotAreaRange * plotAreaHeight}",
                $"Z"
            ) },
        };
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GetDataSeriesShapes_Empty(bool isStacked) {
        var subject = new AreaLayer() {
            Chart = new(),
            IsStacked = isStacked
        };

        Assert.Empty(subject.GetDataSeriesShapes());
    }
}
