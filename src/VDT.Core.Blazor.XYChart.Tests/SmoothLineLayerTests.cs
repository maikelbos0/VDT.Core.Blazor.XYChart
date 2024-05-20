﻿using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class SmoothLineLayerTests {
    [Theory]
    [MemberData(nameof(HaveParametersChanged_Data))]
    public void HaveParametersChanged(bool isStacked, bool showDataLabels, bool showDataMarkers, decimal dataMarkerSize, DataMarkerDelegate dataMarkerType, bool showDataLines, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(SmoothLineLayer.IsStacked), isStacked },
            { nameof(SmoothLineLayer.ShowDataLabels), showDataLabels },
            { nameof(SmoothLineLayer.ShowDataMarkers), showDataMarkers },
            { nameof(SmoothLineLayer.DataMarkerSize), dataMarkerSize },
            { nameof(SmoothLineLayer.DataMarkerType), dataMarkerType },
            { nameof(SmoothLineLayer.ShowDataLines), showDataLines }
        });

        var subject = new SmoothLineLayer {
            IsStacked = false,
            ShowDataLabels = false,
            ShowDataMarkers = true,
            DataMarkerSize = 10M,
            DataMarkerType = DefaultDataMarkerTypes.Square,
            ShowDataLines = true
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<bool, bool, bool, decimal, DataMarkerDelegate, bool, bool> HaveParametersChanged_Data() => new() {
        { false, false, true, 10M, DefaultDataMarkerTypes.Square, true, false },
        { true, false, true, 10M, DefaultDataMarkerTypes.Square, true, true },
        { false, true, true, 10M, DefaultDataMarkerTypes.Square, true, true },
        { false, false, false, 10M, DefaultDataMarkerTypes.Square, true, true },
        { false, false, true, 15M, DefaultDataMarkerTypes.Square, true, true },
        { false, false, true, 10M, DefaultDataMarkerTypes.Round, true, true },
        { false, false, true, 10M, DefaultDataMarkerTypes.Square, false, true }
    };

    [Theory]
    [MemberData(nameof(GetUnstackedDataSeriesShapes_Markers_Data))]
    public void GetUnstackedDataSeriesShapes_Markers(int index, decimal dataPoint, decimal expectedX, decimal expectedY, decimal expectedSize) {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .WithLayer(new SmoothLineLayer() {
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
            .WithLayer(new SmoothLineLayer() {
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
            .WithLayer(new SmoothLineLayer() {
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

    // TODO line layer

    [Fact]
    public void GetStackedDataSeriesShapes_HideDataLines() {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .WithLayer(new SmoothLineLayer() {
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
            .WithLayer(new SmoothLineLayer() {
                IsStacked = isStacked,
                ShowDataLines = true,
                ShowDataMarkers = true
            })
            .Chart.Layers.Single();

        Assert.Empty(subject.GetDataSeriesShapes());
    }
}
