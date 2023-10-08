using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class DataSeriesTests {
    private class TestLayer : LayerBase {
        public override StackMode StackMode => throw new NotImplementedException();
        public override DataPointSpacingMode DefaultDataPointSpacingMode => throw new NotImplementedException();
        public override bool NullAsZero { get; }

        public TestLayer(bool nullAsZero) {
            NullAsZero = nullAsZero;
        }

        public override bool HaveParametersChanged(ParameterView parameters) => throw new NotImplementedException();

        public override IEnumerable<ShapeBase> GetDataSeriesShapes(int layerIndex, IEnumerable<CanvasDataSeries> canvasDataSeries) => throw new NotImplementedException();
    }

    [Theory]
    [MemberData(nameof(HaveParametersChanged_Data))]
    public void HaveParametersChanged(string? name, string? color, List<decimal?> dataPoints, string? cssClass, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(DataSeries.Name), name },
            { nameof(DataSeries.Color), color },
            { nameof(DataSeries.DataPoints), dataPoints },
            { nameof(DataSeries.CssClass), cssClass }
        });

        var subject = new DataSeries() {
            Name = "Foo",
            Color = "red",
            DataPoints = { 1, 2, 3 },
            CssClass = "foo-series"
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<string?, string?, List<decimal?>, string, bool> HaveParametersChanged_Data() => new() {
        { "Foo", "red", new List<decimal?> { 1, 2, 3 }, "foo-series", false },
        { "Bar", "red", new List<decimal?> { 1, 2, 3 }, "foo-series", true },
        { "Foo", "blue", new List<decimal?> { 1, 2, 3 }, "foo-series", true },
        { "Foo", "red", new List<decimal?> { 1, 2 }, "foo-series", true },
        { "Foo", "red", new List<decimal?> { 1, 2, 3 }, "foo-data", true }
    };

    [Theory]
    [MemberData(nameof(GetDataPoints_Data))]
    public void GetDataPoints(decimal?[] dataPoints, bool nullAsZero, List<(int, decimal)> expectedResult) {
        var subject = new XYChartBuilder(labelCount: 3)
            .WithLayer(new TestLayer(nullAsZero))
            .WithDataSeries(dataPoints)
            .Chart.Layers.Single().DataSeries.Single();

        Assert.Equal(expectedResult, subject.GetDataPoints());
    }

    public static TheoryData<decimal?[], bool, List<(int, decimal)>> GetDataPoints_Data() => new() {
        { new decimal?[]{ 5M, 10M, 15M }, false, new List<(int, decimal)>{ (0, 5M), (1, 10M), (2, 15M) } },
        { new decimal?[]{ 5M, 10M, 15M, 20M }, false, new List<(int, decimal)>{ (0, 5M), (1, 10M), (2, 15M) } },
        { new decimal?[]{ null, 10M }, false, new List<(int, decimal)>{ (1, 10M) } },
        { new decimal?[]{ null, 10M }, true, new List<(int, decimal)>{ (0, 0M), (1, 10M), (2, 0M) } }
    };

    [Fact]
    public void GetColor_Specified() {
        var subject = new XYChartBuilder()
            .WithLayer<LineLayer>()
            .WithDataSeries(new DataSeries() { Color = "magenta" })
            .Chart.Layers.Single().DataSeries.Single();

        Assert.Equal("magenta", subject.GetColor());
    }

    [Theory]
    [InlineData(0, 0, "red")]
    [InlineData(0, 2, "green")]
    [InlineData(0, 3, "red")]
    [InlineData(1, 0, "blue")]
    [InlineData(2, 0, "blue")]
    [InlineData(2, 1, "green")]
    public void GetColor_Default(int layerIndex, int index, string expectedColor) {
        DataSeries.DefaultColors = new List<string>() { "red", "blue", "green" };

        var subject = new XYChartBuilder()
            .WithLayer<LineLayer>()
            .WithDataSeries()
            .WithDataSeries()
            .WithDataSeries()
            .WithDataSeries()
            .WithLayer<LineLayer>()
            .WithDataSeries()
            .WithDataSeries()
            .WithDataSeries()
            .WithLayer<LineLayer>()
            .WithDataSeries()
            .WithDataSeries()
            .Chart.Layers[layerIndex].DataSeries[index];

        Assert.Equal(expectedColor, subject.GetColor());
    }

    [Fact]
    public void GetColor_Fallback() {
        DataSeries.DefaultColors = new();

        var subject = new XYChartBuilder()
            .WithLayer<LineLayer>()
            .WithDataSeries()
            .Chart.Layers.Single().DataSeries.Single();

        Assert.Equal(DataSeries.FallbackColor, subject.GetColor());
    }
}
