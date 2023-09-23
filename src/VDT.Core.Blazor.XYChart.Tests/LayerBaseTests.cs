using System.Collections.Generic;
using System;
using Xunit;
using VDT.Core.Blazor.XYChart.Shapes;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace VDT.Core.Blazor.XYChart.Tests;

public class LayerBaseTests {
    private class TestLayer : LayerBase {
        public override StackMode StackMode { get; }
        public override DataPointSpacingMode DefaultDataPointSpacingMode => throw new NotImplementedException();
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
    [MemberData(nameof(GetScaleDataPoints_Data))]
    public void GetScaleDataPoints(bool isStacked, StackMode stackMode, bool nullAsZero, decimal[] expectedDataPoints) {
        var subject = new XYChartBuilder(labelCount: 4)
            .WithLayer(new TestLayer(stackMode, nullAsZero) {
                IsStacked = isStacked
            })
            .WithDataSeries(-5M, -3M, null, null, 15M)
            .WithDataSeries(-7M, -3M, null, null, 15M)
            .WithDataSeries(7M, null, 3M)
            .WithDataSeries(5M, null, 3M)
            .Chart.Layers.Single();

        Assert.Equal(expectedDataPoints, subject.GetScaleDataPoints());
    }

    public static TheoryData<bool, StackMode, bool, decimal[]> GetScaleDataPoints_Data() => new() {
        { false, StackMode.Single, true, new[] { -5M, -3M, 0M, 0M, -7M, -3M, 0M, 0M, 7M, 0M, 3M, 0M, 5M, 0M, 3M, 0M } },
        { false, StackMode.Split, false, new[] { -5M, -3M, -7M, -3M, 7M, 3M, 5M, 3M } },
        { true, StackMode.Single, false, new[] { -5M, -3M, -12M, -6M, -5M, 3M, 0M, 6M } },
        { true, StackMode.Split, false, new[] { -5M, -3M, -12M, -6M, 7M, 3M, 12M, 6M } }
    };
}
