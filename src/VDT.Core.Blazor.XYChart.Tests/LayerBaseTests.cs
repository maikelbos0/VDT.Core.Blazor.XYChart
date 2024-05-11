using System.Collections.Generic;
using System;
using Xunit;
using VDT.Core.Blazor.XYChart.Shapes;
using Microsoft.AspNetCore.Components;
using System.Linq;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class LayerBaseTests {
    private class TestLayer : LayerBase {
        public override StackMode StackMode { get; }
        public override DataPointSpacingMode DefaultDataPointSpacingMode => DataPointSpacingMode.Center;
        public override bool NullAsZero { get; }

        public TestLayer(StackMode stackMode, bool nullAsZero) {
            StackMode = stackMode;
            NullAsZero = nullAsZero;
        }

        public override bool HaveParametersChanged(ParameterView parameters) => throw new NotImplementedException();

        public override IEnumerable<ShapeBase> GetDataSeriesShapes() => throw new NotImplementedException();
    }

    [Fact]
    public void AddDataSeries() {
        var dataSeries = new DataSeries();
        var subject = new TestLayer(StackMode.Single, false);
        var builder = new XYChartBuilder()
            .WithLayer(subject);

        subject.AddDataSeries(dataSeries);

        Assert.Same(dataSeries, Assert.Single(subject.DataSeries));
        Assert.True(builder.StateHasChangedInvoked);
    }

    [Fact]
    public void RemoveDataSeries() {
        var dataSeries = new DataSeries();
        var subject = new TestLayer(StackMode.Single, false);
        var builder = new XYChartBuilder()
            .WithLayer(subject)
            .WithDataSeries(dataSeries);

        subject.RemoveDataSeries(dataSeries);

        Assert.Empty(subject.DataSeries);
        Assert.True(builder.StateHasChangedInvoked);
    }

    [Theory]
    [MemberData(nameof(GetDataLabelShapes_Data))]
    public void GetDataLabelShapes(decimal?[] dataPoints, int index, decimal expectedX, decimal expectedY, string expectedValue) {
        var subject = new XYChartBuilder(labelCount: 3)
            .WithLayer(new TestLayer(StackMode.Single, false) { ShowDataLabels = true })
            .WithDataSeries(new DataSeries() { CssClass = "example-data", DataPoints = dataPoints })
            .Chart.Layers.Single();

        var result = subject.GetDataLabelShapes().ToList();

        var shape = Assert.IsType<DataLabelShape>(Assert.Single(result, shape => shape.Key == $"{nameof(DataLabelShape)}[0,0,{index}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(expectedValue, shape.Value);
        Assert.Equal("data-label example-data", shape.CssClass);
    }

    public static TheoryData<decimal?[], int, decimal, decimal, string> GetDataLabelShapes_Data() {
        var dataPointWidth = PlotArea_Width / 3M;

        return new() {
            { new decimal?[] { -30M, 30M, 60M }, 0, PlotArea_X + 0.5M * dataPointWidth, PlotArea_Y + (PlotArea_Max + 30M) / PlotArea_Range * PlotArea_Height, (-30M).ToString(Canvas_DataLabelFormat) },
            { new decimal?[] { -30M, 30M, 60M }, 1, PlotArea_X + 1.5M * dataPointWidth, PlotArea_Y + (PlotArea_Max - 30M) / PlotArea_Range * PlotArea_Height, 30M.ToString(Canvas_DataLabelFormat) },
            { new decimal?[] { -30M, 30M, 60M }, 2, PlotArea_X + 2.5M * dataPointWidth, PlotArea_Y + (PlotArea_Max - 60M) / PlotArea_Range * PlotArea_Height, 60M.ToString(Canvas_DataLabelFormat) },
        };
    }

    [Fact]
    public void GetDataLabelShapes_HideDataLabels() {
        var subject = new XYChartBuilder(labelCount: 3)
            .WithLayer(new TestLayer(StackMode.Single, false) { ShowDataLabels = false })
            .WithDataSeries(-30M, 60M, 120M)
            .Chart.Layers.Single();

        var result = subject.GetDataLabelShapes();

        Assert.Empty(result);
    }

    [Fact]
    public void GetDataLabelShapes_Multiplier() {
        var subject = new XYChartBuilder(labelCount: 1)
            .WithPlotArea(multiplier: 10)
            .WithLayer(new TestLayer(StackMode.Single, false) { ShowDataLabels = true })
            .WithDataSeries(-30M)
            .Chart.Layers.Single();

        var result = subject.GetDataLabelShapes();

        var shape = Assert.IsType<DataLabelShape>(Assert.Single(result));

        Assert.Equal((-3M).ToString(Canvas_DataLabelFormat), shape.Value);
    }

    [Theory]
    [MemberData(nameof(GetScaleDataPoints_Data))]
    public void GetScaleDataPoints(bool isStacked, StackMode stackMode, bool nullAsZero, decimal[] expectedDataPoints) {
        var subject = new XYChartBuilder(labelCount: 4)
            .WithLayer(new TestLayer(stackMode, nullAsZero) {
                IsStacked = isStacked
            })
            .WithDataSeries(-6M, -3M, null, null, 150M)
            .WithDataSeries(-6M, -3M, null, null, 150M)
            .WithDataSeries(9M, null, 3M)
            .WithDataSeries(15M, null, 3M)
            .Chart.Layers.Single();

        Assert.Equal(expectedDataPoints, subject.GetScaleDataPoints());
    }

    public static TheoryData<bool, StackMode, bool, decimal[]> GetScaleDataPoints_Data() => new() {
        { false, StackMode.Single, true, new[] { -6M, -3M, 0M, 0M, -6M, -3M, 0M, 0M, 9M, 0M, 3M, 0M, 15M, 0M, 3M, 0M } },
        { false, StackMode.Split, false, new[] { -6M, -3M, -6M, -3M, 9M, 3M, 15M, 3M } },
        { true, StackMode.Single, false, new[] { -6M, -3M, -12M, -6M, -3M, 3M, 12M, 6M } },
        { true, StackMode.Split, false, new[] { -6M, -3M, -12M, -6M, 9M, 3M, 24M, 6M } }
    };
}
