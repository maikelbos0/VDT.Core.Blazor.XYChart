using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

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
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.EdgeToEdge)
            .WithLayer(new AreaLayer() {
                IsStacked = false
            })
            .WithDataSeries(new DataSeries() {
                Color = "blue",
                DataPoints = { -10M, -10M, 10M, 15M },
                CssClass = "example-data"
            })
            .WithDataSeries(new DataSeries() {
                Color = "red",
                DataPoints = { null, null, null, 15M },
                CssClass = "example-data"
            })
            .Chart.Layers.Single();

        subject.DataSeries[1].DataPoints[startIndex] = startDataPoint;
        subject.DataSeries[1].DataPoints[endIndex] = endDataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<AreaDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(AreaDataShape)}[1]"));

        Assert.Equal(expectedPath, shape.Path);
        Assert.Equal("red", shape.Color);
        Assert.Equal("data area-data example-data", shape.CssClass);
    }

    public static TheoryData<int, decimal, int, decimal, string> GetUnstackedDataSeriesShapes_Data() {
        var dataPointWidth = PlotAreaWidth / 2M;

        return new() {
            { 0, 5M, 1, -5M, Helpers.Path(
                $"M {PlotAreaX} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + (PlotAreaMax - 5M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 5M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"Z"
            ) },
            { 1, -5M, 2, 5M, Helpers.Path(
                $"M {PlotAreaX} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 5M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 5M)  / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"Z"
            ) }
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_Data))]
    public void GetStackedDataSeriesShapes(int dataSeriesIndex, int startIndex, decimal startDataPoint, int endIndex, decimal endDataPoint, string expectedPath) {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.EdgeToEdge)
            .WithLayer(new AreaLayer() {
                IsStacked = true
            })
            .WithDataSeries(new DataSeries() {
                Color = "blue",
                DataPoints = { -10M, -10M, 10M, 15M },
                CssClass = "example-data"
            })
            .WithDataSeries(new DataSeries() {
                Color = "red",
                DataPoints = { null, null, null, 15M },
                CssClass = "example-data"
            })
            .Chart.Layers.Single();

        subject.DataSeries[dataSeriesIndex].DataPoints[startIndex] = startDataPoint;
        subject.DataSeries[dataSeriesIndex].DataPoints[endIndex] = endDataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<AreaDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(AreaDataShape)}[{dataSeriesIndex}]"));

        Assert.Equal(expectedPath, shape.Path);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
        Assert.Equal("data area-data example-data", shape.CssClass);
    }

    public static TheoryData<int, int, decimal, int, decimal, string> GetStackedDataSeriesShapes_Data() {
        var dataPointWidth = PlotAreaWidth / 2M;

        return new() {
            { 0, 0, 5M, 1, 5M, Helpers.Path(
                $"M {PlotAreaX} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + (PlotAreaMax - 5M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax - 5M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 10M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"Z"
            ) },
            { 1, 0, 5M, 1, -5M, Helpers.Path(
                $"M {PlotAreaX} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + (PlotAreaMax + 5M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 15M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M  * dataPointWidth} {PlotAreaY + (PlotAreaMax - 10M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"Z"
            ) },
            { 1, 0, -5M, 1, 5M, Helpers.Path(
                $"M {PlotAreaX} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + (PlotAreaMax + 15M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 5M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 10M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"Z"
            ) },
            { 1, 1, 5M, 2, -5M, Helpers.Path(
                $"M {PlotAreaX} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + (PlotAreaMax + 10M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 5M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 5M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"Z"
            ) },
            { 1, 1, -5M, 2, 5M, Helpers.Path(
                $"M {PlotAreaX} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + (PlotAreaMax + 10M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 15M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 15M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"Z"
            ) },
        };
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GetDataSeriesShapes_Empty(bool isStacked) {
        var subject = new XYChartBuilder()
            .WithLayer(new AreaLayer() {
                IsStacked = isStacked
            })
            .Chart.Layers.Single();

        Assert.Empty(subject.GetDataSeriesShapes());
    }
}
