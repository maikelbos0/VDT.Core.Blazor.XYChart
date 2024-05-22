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
    public void HaveParametersChanged(
        bool isStacked,
        bool showDataLabels,
        bool showDataMarkers,
        decimal dataMarkerSize,
        DataMarkerDelegate dataMarkerType,
        DataLineMode dataLineMode,
        decimal controlPointDistancePercentage,
        bool expectedResult
    ) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(LineLayer.IsStacked), isStacked },
            { nameof(LineLayer.ShowDataLabels), showDataLabels },
            { nameof(LineLayer.ShowDataMarkers), showDataMarkers },
            { nameof(LineLayer.DataMarkerSize), dataMarkerSize },
            { nameof(LineLayer.DataMarkerType), dataMarkerType },
            { nameof(LineLayer.DataLineMode), dataLineMode },
            { nameof(LineLayer.ControlPointDistancePercentage), controlPointDistancePercentage }
        });

        var subject = new LineLayer {
            IsStacked = false,
            ShowDataLabels = false,
            ShowDataMarkers = true,
            DataMarkerSize = 10M,
            DataMarkerType = DefaultDataMarkerTypes.Square,
            DataLineMode = DataLineMode.Straight,
            ControlPointDistancePercentage = 25M
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<bool, bool, bool, decimal, DataMarkerDelegate, DataLineMode, decimal, bool> HaveParametersChanged_Data() => new() {
        { false, false, true, 10M, DefaultDataMarkerTypes.Square, DataLineMode.Straight, 25M, false },
        { true, false, true, 10M, DefaultDataMarkerTypes.Square, DataLineMode.Straight, 25M, true },
        { false, true, true, 10M, DefaultDataMarkerTypes.Square, DataLineMode.Straight, 25M, true },
        { false, false, false, 10M, DefaultDataMarkerTypes.Square, DataLineMode.Straight, 25M, true },
        { false, false, true, 15M, DefaultDataMarkerTypes.Square, DataLineMode.Straight, 25M, true },
        { false, false, true, 10M, DefaultDataMarkerTypes.Round, DataLineMode.Straight, 25M, true },
        { false, false, true, 10M, DefaultDataMarkerTypes.Square, DataLineMode.Hidden, 25M, true },
        { false, false, true, 10M, DefaultDataMarkerTypes.Square, DataLineMode.Straight, 50M, true }
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

        var shape = Assert.IsType<RoundDataMarkerShape>(Assert.Single(result, shape => shape.Key == $"{nameof(RoundDataMarkerShape)}[0,1,{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedSize, shape.Size);
        Assert.Equal("red", shape.Color);
        Assert.Equal("data-marker data-marker-round example-data", shape.CssClass);
    }

    public static TheoryData<int, decimal, decimal, decimal, decimal> GetUnstackedDataSeriesShapes_Markers_Data() {
        var dataPointWidth = PlotArea_Width / 3M;

        return new() {
            { 0, -30M, PlotArea_X + 0.5M * dataPointWidth, PlotArea_Y + (PlotArea_Max + 30M) / PlotArea_Range * PlotArea_Height, 20M },
            { 1, 30M, PlotArea_X + 1.5M * dataPointWidth, PlotArea_Y  + (PlotArea_Max - 30M) / PlotArea_Range * PlotArea_Height, 20M },
            { 2, 210M, PlotArea_X + 2.5M * dataPointWidth, PlotArea_Y  + (PlotArea_Max - 210M) / PlotArea_Range * PlotArea_Height, 20M },
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

        var shape = Assert.IsType<RoundDataMarkerShape>(Assert.Single(result, shape => shape.Key == $"{nameof(RoundDataMarkerShape)}[0,{dataSeriesIndex},{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedSize, shape.Size);
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
        Assert.Equal("data-marker data-marker-round example-data", shape.CssClass);
    }

    public static TheoryData<int, int, decimal, decimal, decimal, decimal> GetStackedDataSeriesShapes_Markers_Data() {
        var dataPointWidth = PlotArea_Width / 3M;

        return new() {
            { 0, 0, -30M, PlotArea_X + 0.5M * dataPointWidth, PlotArea_Y + (PlotArea_Max + 30M) / PlotArea_Range * PlotArea_Height, 20M },
            { 0, 2, 30M, PlotArea_X + 2.5M * dataPointWidth, PlotArea_Y + (PlotArea_Max - 30M) / PlotArea_Range * PlotArea_Height, 20M },
            { 1, 0, -30M, PlotArea_X + 0.5M * dataPointWidth, PlotArea_Y + (PlotArea_Max + 90M) / PlotArea_Range * PlotArea_Height, 20M },
            { 1, 0, 30M, PlotArea_X + 0.5M * dataPointWidth, PlotArea_Y + (PlotArea_Max + 30M) / PlotArea_Range * PlotArea_Height, 20M },
            { 1, 2, -30M, PlotArea_X + 2.5M * dataPointWidth, PlotArea_Y + (PlotArea_Max - 30M) / PlotArea_Range * PlotArea_Height, 20M },
            { 1, 2, 30M, PlotArea_X + 2.5M * dataPointWidth, PlotArea_Y + (PlotArea_Max - 90M) / PlotArea_Range * PlotArea_Height, 20M }
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
    [MemberData(nameof(GetUnstackedDataSeriesShapes_StraightDataLines_Data))]
    public void GetUnstackedDataSeriesShapes_StraightDataLines(int startIndex, decimal startDataPoint, int endIndex, decimal endDataPoint, string expectedPath) {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .WithLayer(new LineLayer() {
                IsStacked = false,
                DataLineMode = DataLineMode.Straight
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

    public static TheoryData<int, decimal, int, decimal, string> GetUnstackedDataSeriesShapes_StraightDataLines_Data() {
        var dataPointWidth = PlotArea_Width / 3M;

        return new() {
            { 0, 30M, 1, -30M, Path.Create(
                $"M {PlotArea_X + 0.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 30M) / PlotArea_Range * PlotArea_Height}",
                $"L {PlotArea_X + 1.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 30M) / PlotArea_Range * PlotArea_Height}"
            ) },
            { 1, -30M, 2, 30M, Path.Create(
                $"M {PlotArea_X + 1.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 30M) / PlotArea_Range * PlotArea_Height}",
                $"L {PlotArea_X + 2.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 30M) / PlotArea_Range * PlotArea_Height}"
            ) }
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_StraightDataLines_Data))]
    public void GetStackedDataSeriesShapes_StraightDataLines(int dataSeriesIndex, int startIndex, decimal startDataPoint, int endIndex, decimal endDataPoint, string expectedPath) {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .WithLayer(new LineLayer() {
                IsStacked = true,
                DataLineMode = DataLineMode.Straight
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

    public static TheoryData<int, int, decimal, int, decimal, string> GetStackedDataSeriesShapes_StraightDataLines_Data() {
        var dataPointWidth = PlotArea_Width / 3M;

        return new() {
            { 0, 0, 30M, 1, 30M, Path.Create(
                $"M {PlotArea_X + 0.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 30M) / PlotArea_Range * PlotArea_Height}",
                $"L {PlotArea_X + 1.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 30M) / PlotArea_Range * PlotArea_Height}",
                $"L {PlotArea_X + 2.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 60M) / PlotArea_Range * PlotArea_Height}"
            ) },
            { 1, 0, 30M, 1, -30M, Path.Create(
                $"M {PlotArea_X + 0.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 30M) / PlotArea_Range * PlotArea_Height}",
                $"L {PlotArea_X + 1.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 90M) / PlotArea_Range * PlotArea_Height}",
                $"L {PlotArea_X + 2.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 60M) / PlotArea_Range * PlotArea_Height}"
            ) },
            { 1, 0, -30M, 1, 30M, Path.Create(
                $"M {PlotArea_X + 0.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 90M) / PlotArea_Range * PlotArea_Height}",
                $"L {PlotArea_X + 1.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 30M) / PlotArea_Range * PlotArea_Height}",
                $"L {PlotArea_X + 2.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 60M) / PlotArea_Range * PlotArea_Height}"
            ) },
            { 1, 1, 30M, 2, -30M, Path.Create(
                $"M {PlotArea_X + 0.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 60M) / PlotArea_Range * PlotArea_Height}",
                $"L {PlotArea_X + 1.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 30M) / PlotArea_Range * PlotArea_Height}",
                $"L {PlotArea_X + 2.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 30M) / PlotArea_Range * PlotArea_Height}"
            ) },
            { 1, 1, -30M, 2, 30M, Path.Create(
                $"M {PlotArea_X + 0.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 60M) / PlotArea_Range * PlotArea_Height}",
                $"L {PlotArea_X + 1.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 90M) / PlotArea_Range * PlotArea_Height}",
                $"L {PlotArea_X + 2.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 90M) / PlotArea_Range * PlotArea_Height}"
            ) },
        };
    }

    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_SmoothDataLines_Data))]
    public void GetUnstackedDataSeriesShapes_SmoothDataLines(int dataSeriesIndex, string expectedPath) {
        var subject = new XYChartBuilder(labelCount: 4, DataPointSpacingMode.Center)
            .WithLayer(new LineLayer() {
                IsStacked = false,
                DataLineMode = DataLineMode.Smooth
            })
            .WithDataSeries(new DataSeries() {
                Color = "blue",
                DataPoints = { -60M, -60M, 60M, 150M },
                CssClass = "example-data"
            })
            .WithDataSeries(new DataSeries() {
                Color = "red",
                DataPoints = { null, null, 60M, 150M },
                CssClass = "example-data"
            })
            .Chart.Layers.Single();

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<LineDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(LineDataShape)}[0,{dataSeriesIndex}]"));

        Assert.Equal(expectedPath, Path.TrimDecimals(shape.Path));
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
        Assert.Equal("data line-data example-data", shape.CssClass);
    }

    public static TheoryData<int, string> GetUnstackedDataSeriesShapes_SmoothDataLines_Data() {
        var dataPointWidth = PlotArea_Width / 4M;

        return new() {
            { 0, Path.Create(
                $"M {PlotArea_X + 0.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 60M) / PlotArea_Range * PlotArea_Height}",
                $"Q {PlotArea_X + 1.25M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 75M) / PlotArea_Range * PlotArea_Height}, {PlotArea_X + 1.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 60M) / PlotArea_Range * PlotArea_Height}",
                $"C {PlotArea_X + 1.75M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 45M) / PlotArea_Range * PlotArea_Height}, {PlotArea_X + 2.25M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 33.75M) / PlotArea_Range * PlotArea_Height}, {PlotArea_X + 2.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 60M) / PlotArea_Range * PlotArea_Height}",
                $"Q {PlotArea_X + 2.75M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 86.25M) / PlotArea_Range * PlotArea_Height}, {PlotArea_X + 3.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 150M) / PlotArea_Range * PlotArea_Height}"
            ) },
            { 1, Path.Create(
                $"M {PlotArea_X + 2.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 60M) / PlotArea_Range * PlotArea_Height}",
                $"L {PlotArea_X + 3.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 150M) / PlotArea_Range * PlotArea_Height}"
            ) },
        };
    }

    [Theory]
    [MemberData(nameof(GetStackedDataSeriesShapes_SmoothDataLines_Data))]
    public void GetStackedDataSeriesShapes_SmoothDataLines(int dataSeriesIndex, string expectedPath) {
        var subject = new XYChartBuilder(labelCount: 4, DataPointSpacingMode.Center)
            .WithLayer(new LineLayer() {
                IsStacked = true,
                DataLineMode = DataLineMode.Smooth
            })
            .WithDataSeries(new DataSeries() {
                Color = "blue",
                DataPoints = { -60M, -60M, 60M, 150M },
                CssClass = "example-data"
            })
            .WithDataSeries(new DataSeries() {
                Color = "red",
                DataPoints = { null, null, 60M, 150M },
                CssClass = "example-data"
            })
            .Chart.Layers.Single();

        var result = subject.GetDataSeriesShapes();

        var shape = Assert.IsType<LineDataShape>(Assert.Single(result, shape => shape.Key == $"{nameof(LineDataShape)}[0,{dataSeriesIndex}]"));

        Assert.Equal(expectedPath, Path.TrimDecimals(shape.Path));
        Assert.Equal(subject.DataSeries[dataSeriesIndex].Color, shape.Color);
        Assert.Equal("data line-data example-data", shape.CssClass);
    }

    public static TheoryData<int, string> GetStackedDataSeriesShapes_SmoothDataLines_Data() {
        var dataPointWidth = PlotArea_Width / 4M;

        return new() {
            { 0, Path.Create(
                $"M {PlotArea_X + 0.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 60M) / PlotArea_Range * PlotArea_Height}",
                $"Q {PlotArea_X + 1.25M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 75M) / PlotArea_Range * PlotArea_Height}, {PlotArea_X + 1.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 60M) / PlotArea_Range * PlotArea_Height}",
                $"C {PlotArea_X + 1.75M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 45M) / PlotArea_Range * PlotArea_Height}, {PlotArea_X + 2.25M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 33.75M) / PlotArea_Range * PlotArea_Height}, {PlotArea_X + 2.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 60M) / PlotArea_Range * PlotArea_Height}",
                $"Q {PlotArea_X + 2.75M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 86.25M) / PlotArea_Range * PlotArea_Height}, {PlotArea_X + 3.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 150M) / PlotArea_Range * PlotArea_Height}"
            ) },
            { 1, Path.Create(
                $"M {PlotArea_X + 0.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 60M) / PlotArea_Range * PlotArea_Height}",
                $"Q {PlotArea_X + 1.25M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 82.5M) / PlotArea_Range * PlotArea_Height}, {PlotArea_X + 1.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 60M) / PlotArea_Range * PlotArea_Height}",
                $"C {PlotArea_X + 1.75M * dataPointWidth} {PlotArea_Y + (PlotArea_Max + 37.5M) / PlotArea_Range * PlotArea_Height}, {PlotArea_X + 2.25M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 75M) / PlotArea_Range * PlotArea_Height}, {PlotArea_X + 2.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 120M) / PlotArea_Range * PlotArea_Height}",
                $"Q {PlotArea_X + 2.75M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 165M) / PlotArea_Range * PlotArea_Height}, {PlotArea_X + 3.5M * dataPointWidth} {PlotArea_Y + (PlotArea_Max - 300M) / PlotArea_Range * PlotArea_Height}"
            ) }
        };
    }

    [Fact]
    public void GetStackedDataSeriesShapes_HiddenDataLines() {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .WithLayer(new LineLayer() {
                DataLineMode = DataLineMode.Hidden
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
                ShowDataMarkers = true,
                DataLineMode = DataLineMode.Hidden
            })
            .Chart.Layers.Single();

        Assert.Empty(subject.GetDataSeriesShapes());
    }

    [Theory]
    [MemberData(nameof(GetControlPoints_Data))]
    public void GetControlPoints(decimal controlPointDistancePercentage, decimal leftX, decimal leftY, decimal rightX, decimal rightY, decimal expectedLeftX, decimal expectedLeftY, decimal expectedRightX, decimal expectedRightY) {
        var left = new CanvasDataPoint(leftX, leftY, 0, 0, 0, 0);
        var dataPoint = new CanvasDataPoint(20M, 50M, 0, 0, 0, 0);
        var right = new CanvasDataPoint(rightX, rightY, 0, 0, 0, 0);
        var subject = new LineLayer() {
            ControlPointDistancePercentage = controlPointDistancePercentage
        };

        var result = subject.GetControlPoints(left, dataPoint, right);

        Assert.Equal(expectedLeftX, result.LeftX);
        Assert.Equal(expectedLeftY, result.LeftY);
        Assert.Equal(expectedRightX, result.RightX);
        Assert.Equal(expectedRightY, result.RightY);
    }

    public static TheoryData<decimal, decimal, decimal, decimal, decimal, decimal, decimal, decimal, decimal> GetControlPoints_Data() => new() {
        { 25M, 10M, 50M, 30M, 50M, 17.5M, 50M, 22.5M, 50M },
        { 40M, 10M, 50M, 30M, 50M, 16M, 50M, 24M, 50M },
        { 25M, 10M, 100M, 30M, 100M, 17.5M, 50M, 22.5M, 50M },
        { 25M, 10M, 60M, 30M, 40M, 17.5M, 52.5M, 22.5M, 47.5M },
        { 40M, 10M, 60M, 30M, 40M, 16M, 54M, 24M, 46M },
        { 25M, 10M, 40M, 30M, 60M, 17.5M, 47.5M, 22.5M, 52.5M },
        { 25M, 10M, 40M, 30M, 70M, 17.5M, 46.25M, 22.5M, 53.75M },
    };
}
