using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class LineLayerTests {
    [Theory]
    [MemberData(nameof(HaveParametersChanged_Data))]
    public void HaveParametersChanged(bool isStacked, bool showDataMarkers, decimal dataMarkerSize, DataMarkerDelegate dataMarkerType, bool showDataLines, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(LineLayer.IsStacked), isStacked },
            { nameof(LineLayer.ShowDataMarkers), showDataMarkers },
            { nameof(LineLayer.DataMarkerSize), dataMarkerSize },
            { nameof(LineLayer.DataMarkerType), dataMarkerType },
            { nameof(LineLayer.ShowDataLines), showDataLines }
        });

        var subject = new LineLayer {
            IsStacked = false,
            ShowDataMarkers = true,
            DataMarkerSize = 10M,
            DataMarkerType = DefaultDataMarkerTypes.Square,
            ShowDataLines = true
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<bool, bool, decimal, DataMarkerDelegate, bool, bool> HaveParametersChanged_Data() => new() {
        { false, true, 10M, DefaultDataMarkerTypes.Square, true, false },
        { true, true, 10M, DefaultDataMarkerTypes.Square, true, true },
        { false, false, 10M, DefaultDataMarkerTypes.Square, true, true },
        { false, true, 15M, DefaultDataMarkerTypes.Square, true, true },
        { false, true, 10M, DefaultDataMarkerTypes.Round, true, true },
        { false, true, 10M, DefaultDataMarkerTypes.Square, false, true }
    };

    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_Markers_Data))]
    public void GetUnstackedDataSeriesShapes_Markers(int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedSize) {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .WithLayer(new LineLayer() {
                IsStacked = false,
                ShowDataMarkers = true,
                DataMarkerSize = 20M,
                DataMarkerType = DefaultDataMarkerTypes.Round
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

        subject.DataSeries[1].DataPoints[index] = dataPoint;

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<RoundDataMarkerShape>(Assert.Single(result, shape => shape.Key == $"{nameof(RoundDataMarkerShape)}[1,{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedSize, shape.Size);
        Assert.Equal("red", shape.Color);
        Assert.Equal("data-marker data-marker-round example-data", shape.CssClass);
    }

    public static TheoryData<int, decimal, decimal, decimal, decimal> GetUnstackedDataSeriesShapes_Markers_Data() {
        var dataPointWidth = PlotAreaWidth / 3M;

        return new() {
            { 0, -30M, PlotAreaX + 0.5M * dataPointWidth, PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight, 20M },
            { 1, 30M, PlotAreaX + 1.5M * dataPointWidth, PlotAreaY  + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight, 20M },
            { 2, 210M, PlotAreaX + 2.5M * dataPointWidth, PlotAreaY  + (PlotAreaMax - 210M) / PlotAreaRange * PlotAreaHeight, 20M },
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_Markers_Data))]
    public void GetStackedDataSeriesShapes_Markers(int dataSeriesIndex, int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedSize) {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .WithLayer(new LineLayer() {
                IsStacked = true,
                ShowDataMarkers = true,
                DataMarkerSize = 20M,
                DataMarkerType = DefaultDataMarkerTypes.Round
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

        var shape = Assert.IsType<RoundDataMarkerShape>(Assert.Single(result, shape => shape.Key == $"{nameof(RoundDataMarkerShape)}[{dataSeriesIndex},{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedSize, shape.Size);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
        Assert.Equal("data-marker data-marker-round example-data", shape.CssClass);
    }

    public static TheoryData<int, int, decimal, decimal, decimal, decimal> GetStackedDataSeriesShapes_Markers_Data() {
        var dataPointWidth = PlotAreaWidth / 3M;

        return new() {
            { 0, 0, -30M, PlotAreaX + 0.5M * dataPointWidth, PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight, 20M },
            { 0, 2, 30M, PlotAreaX + 2.5M * dataPointWidth, PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight, 20M },
            { 1, 0, -30M, PlotAreaX + 0.5M * dataPointWidth, PlotAreaY + (PlotAreaMax + 90M) / PlotAreaRange * PlotAreaHeight, 20M },
            { 1, 0, 30M, PlotAreaX + 0.5M * dataPointWidth, PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight, 20M },
            { 1, 2, -30M, PlotAreaX + 2.5M * dataPointWidth, PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight, 20M },
            { 1, 2, 30M, PlotAreaX + 2.5M * dataPointWidth, PlotAreaY + (PlotAreaMax - 90M) / PlotAreaRange * PlotAreaHeight, 20M }
        };
    }

    [Fact]
    public void GetStackedDataSeriesShapes_HideDataMarkers() {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .WithLayer(new LineLayer() {
                ShowDataMarkers = false
            })
            .WithDataSeries(new DataSeries() {
                Color = "blue",
                DataPoints = { -60M, -60M, 60M, 150M },
                CssClass = "example-data"
            })
            .Chart.Layers.Single();

        var result = subject.GetDataSeriesShapes();

        Assert.DoesNotContain(result, shape => shape is RoundDataMarkerShape);
    }

    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_Lines_Data))]
    public void GetUnstackedDataSeriesShapes_Lines(int startIndex, decimal startDataPoint, int endIndex, decimal endDataPoint, string expectedPath) {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .WithLayer(new LineLayer() {
                IsStacked = false,
                ShowDataLines = true
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

        var shape = Assert.IsType<LineDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(LineDataShape)}[0,1]"));

        Assert.Equal(expectedPath, Path.TrimDecimals(shape.Path));
        Assert.Equal("red", shape.Color);
        Assert.Equal("data line-data example-data", shape.CssClass);
    }

    public static TheoryData<int, decimal, int, decimal, string> GetUnstackedDataSeriesShapes_Lines_Data() {
        var dataPointWidth = PlotAreaWidth / 3M;

        return new() {
            { 0, 30M, 1, -30M, Path.Create(
                $"M {PlotAreaX + 0.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 1.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight}"
            ) },
            { 1, -30M, 2, 30M, Path.Create(
                $"M {PlotAreaX + 1.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight}"
            ) }
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_Lines_Data))]
    public void GetStackedDataSeriesShapes_Lines(int dataSeriesIndex, int startIndex, decimal startDataPoint, int endIndex, decimal endDataPoint, string expectedPath) {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .WithLayer(new LineLayer() {
                IsStacked = true,
                ShowDataLines = true
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

        var shape = Assert.IsType<LineDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(LineDataShape)}[0,{dataSeriesIndex}]"));

        Assert.Equal(expectedPath, Path.TrimDecimals(shape.Path));
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
        Assert.Equal("data line-data example-data", shape.CssClass);
    }

    public static TheoryData<int, int, decimal, int, decimal, string> GetStackedDataSeriesShapes_Lines_Data() {
        var dataPointWidth = PlotAreaWidth / 3M;

        return new() {
            { 0, 0, 30M, 1, 30M, Path.Create(
                $"M {PlotAreaX + 0.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 1.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 60M) / PlotAreaRange * PlotAreaHeight}"
            ) },
            { 1, 0, 30M, 1, -30M, Path.Create(
                $"M {PlotAreaX + 0.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 1.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax + 90M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 60M) / PlotAreaRange * PlotAreaHeight}"
            ) },
            { 1, 0, -30M, 1, 30M, Path.Create(
                $"M {PlotAreaX + 0.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax + 90M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 1.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 60M) / PlotAreaRange * PlotAreaHeight}"
            ) },
            { 1, 1, 30M, 2, -30M, Path.Create(
                $"M {PlotAreaX + 0.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax + 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 1.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax + 30M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 30M) / PlotAreaRange * PlotAreaHeight}"
            ) },
            { 1, 1, -30M, 2, 30M, Path.Create(
                $"M {PlotAreaX + 0.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax + 60M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 1.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax + 90M) / PlotAreaRange * PlotAreaHeight}",
                $"L {PlotAreaX + 2.5M * dataPointWidth} {PlotAreaY + (PlotAreaMax - 90M) / PlotAreaRange * PlotAreaHeight}"
            ) },
        };
    }

    [Fact]
    public void GetStackedDataSeriesShapes_HideDataLines() {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .WithLayer(new LineLayer() {
                ShowDataLines = false
            })
            .WithDataSeries(new DataSeries() {
                Color = "blue",
                DataPoints = { -60M, -60M, 60M, 150M },
                CssClass = "example-data"
            })
            .Chart.Layers.Single();

        var result = subject.GetDataSeriesShapes();

        Assert.DoesNotContain(result, shape => shape is LineDataShape);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GetDataSeriesShapes_Empty(bool isStacked) {
        var subject = new XYChartBuilder()
            .WithLayer(new LineLayer() {
                IsStacked = isStacked,
                ShowDataLines = true,
                ShowDataMarkers = true
            })
            .Chart.Layers.Single();

        Assert.Empty(subject.GetDataSeriesShapes());
    }
}
