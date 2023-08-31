using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class DataSeriesTests {
    private class TestLayer : LayerBase {
        public override StackMode StackMode => throw new NotImplementedException();
        public override DataPointSpacingMode DefaultDataPointSpacingMode => throw new NotImplementedException();

        public override bool HaveParametersChanged(ParameterView parameters) => throw new NotImplementedException();

        public override IEnumerable<ShapeBase> GetDataSeriesShapes() => throw new NotImplementedException();
    }

    [Theory]
    [MemberData(nameof(HaveParametersChanged_Data))]
    public void HaveParametersChanged(string? name, string? color, List<decimal?> dataPoints, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(DataSeries.Name), name },
            { nameof(DataSeries.Color), color },
            { nameof(DataSeries.DataPoints), dataPoints }
        });

        var subject = new DataSeries() {
            Name = "Foo",
            Color = "red",
            DataPoints = { 1, 2, 3}
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<string?, string?, List<decimal?>, bool> HaveParametersChanged_Data() => new() {
        { "Foo", "red", new List<decimal?> { 1, 2, 3 }, false },
        { "Bar", "red", new List<decimal?> { 1, 2, 3 }, true },
        { "Foo", "blue", new List<decimal?> { 1, 2, 3 }, true },
        { "Foo", "red", new List<decimal?> { 1, 2 }, true }
    };

    [Fact]
    public void GetColor() {
        var subject = new DataSeries() {
            Color = "magenta"
        };

        Assert.Equal("magenta", subject.GetColor());
    }

    [Theory]
    [InlineData(0, "red")]
    [InlineData(1, "blue")]
    [InlineData(2, "green")]
    [InlineData(3, "red")]
    public void GetColor_Default(int index, string expectedColor) {
        DataSeries.DefaultColors = new List<string>() { "red", "blue", "green" };

        var layer = new TestLayer();
        layer.DataSeries.Add(new DataSeries() { Layer = layer });
        layer.DataSeries.Add(new DataSeries() { Layer = layer });
        layer.DataSeries.Add(new DataSeries() { Layer = layer });
        layer.DataSeries.Add(new DataSeries() { Layer = layer });

        var subject = layer.DataSeries[index];

        Assert.Equal(expectedColor, subject.GetColor());
    }

    [Fact]
    public void GetColor_Fallback() {
        DataSeries.DefaultColors = new();

        var layer = new TestLayer();
        var subject = new DataSeries() { Layer = layer };

        layer.DataSeries.Add(subject);

        Assert.Equal(DataSeries.FallbackColor, subject.GetColor());
    }
}
