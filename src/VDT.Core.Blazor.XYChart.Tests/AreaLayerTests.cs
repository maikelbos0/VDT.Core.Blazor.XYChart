using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class AreaLayerTests {
    [Theory]
    [InlineData(false, false, false)]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)]
    public void HaveParametersChanged(bool isStacked, bool showDataLabels, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(AreaLayer.IsStacked), isStacked },
            { nameof(AreaLayer.ShowDataLabels), showDataLabels }
        });

        var subject = new AreaLayer {
            IsStacked = false,
            ShowDataLabels = false
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
                DataPoints = { -60M, -60M, 60M, 150M },
                CssClass = "example-data"
            })
            .WithDataSeries(new DataSeries() {
                Color = "red",
                DataPoints = { null, null, null, 150M },
                CssClass = "example-data"
            })
            .Chart.Layers.Single();

        subject.DataSeries[1].DataPoints[startIndex] = startDataPoint;
        subject.DataSeries[1].DataPoints[endIndex] = endDataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<AreaDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(AreaDataShape)}[0,1]"));

        Assert.Equal(expectedPath, Path.TrimDecimals(shape.Path));
        Assert.Equal("red", shape.Color);
        Assert.Equal("data area-data example-data", shape.CssClass);
    }

    public static TheoryData<int, decimal, int, decimal, string> GetUnstackedDataSeriesShapes_Data() {
        var dataPointWidth = PlotAreaWidth / 2M;

        return new() {
            { 0, 30M, 1, -30M, Path.Create(
                $"M {PlotAreaX} {PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"Z"
            ) },
            { 1, -30M, 2, 30M, Path.Create(
                $"M {PlotAreaX} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 30M)  / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
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
                DataPoints = { -60M, -60M, 60M, 150M },
                CssClass = "example-data"
            })
            .WithDataSeries(new DataSeries() {
                Color = "red",
                DataPoints = { null, null, null, 150M },
                CssClass = "example-data"
            })
            .Chart.Layers.Single();

        subject.DataSeries[dataSeriesIndex].DataPoints[startIndex] = startDataPoint;
        subject.DataSeries[dataSeriesIndex].DataPoints[endIndex] = endDataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<AreaDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(AreaDataShape)}[0,{dataSeriesIndex}]"));

        Assert.Equal(expectedPath, Path.TrimDecimals(shape.Path));
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
        Assert.Equal("data area-data example-data", shape.CssClass);
    }

    public static TheoryData<int, int, decimal, int, decimal, string> GetStackedDataSeriesShapes_Data() {
        var dataPointWidth = PlotAreaWidth / 2M;

        return new() {
            { 0, 0, 30M, 1, 30M, Path.Create(
                $"M {PlotAreaX} {PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + PlotAreaMax / PlotAreaRange * PlotAreaHeight}",
                $"Z"
            ) },
            { 1, 0, 30M, 1, -30M, Path.Create(
                $"M {PlotAreaX} {PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 90M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + (PlotAreaMax + 60M) / PlotAreaRange * PlotAreaHeight}",
                $"Z"
            ) },
            { 1, 0, -30M, 1, 30M, Path.Create(
                $"M {PlotAreaX} {PlotAreaY + (PlotAreaMax + 90M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + (PlotAreaMax + 60M) / PlotAreaRange * PlotAreaHeight}",
                $"Z"
            ) },
            { 1, 1, 30M, 2, -30M, Path.Create(
                $"M {PlotAreaX} {PlotAreaY + (PlotAreaMax + 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + (PlotAreaMax + 60M) / PlotAreaRange * PlotAreaHeight}",
                $"Z"
            ) },
            { 1, 1, -30M, 2, 30M, Path.Create(
                $"M {PlotAreaX} {PlotAreaY + (PlotAreaMax + 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 90M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 90M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + dataPointWidth} {PlotAreaY + (PlotAreaMax + 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX} {PlotAreaY + (PlotAreaMax + 60M) / PlotAreaRange * PlotAreaHeight}",
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
