﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Xunit;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class PlotAreaTests {
    [Theory]
    [MemberData(nameof(HaveParametersChanged_Data))]
    public void HaveParametersChanged(decimal min, decimal max, decimal gridLineInterval, decimal multiplier, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(PlotArea.Min), min },
            { nameof(PlotArea.Max), max },
            { nameof(PlotArea.GridLineInterval), gridLineInterval },
            { nameof(PlotArea.Multiplier), multiplier}
        });

        var subject = new PlotArea {
            Min = 0M,
            Max = 100M,
            GridLineInterval = 10M,
            Multiplier = 1M
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<decimal, decimal, decimal, decimal, bool> HaveParametersChanged_Data() => new() {
        { 0M, 100M, 10M, 1M, false },
        { -10M, 100M, 10M, 1M, true },
        { 0M, 150M, 10M, 1M, true },
        { 0M, 100M, 5M, 1M, true },
        { 0M, 100M, 10M, 100M, true },
    };

    [Theory]
    [MemberData(nameof(AutoScale_Data))]
    public void AutoScale(decimal[] dataPoints, int requestedGridLineCount, decimal expectedGridLineInterval, decimal expectedMin, decimal expectedMax) {
        var subject = new XYChartBuilder()
            .WithAutoScaleSettings(isEnabled: true, includeZero: false, clearancePercentage: 0M, requestedGridLineCount: requestedGridLineCount)
            .Chart.PlotArea;

        subject.AutoScale(dataPoints);

        Assert.Equal(expectedMin, subject.Min);
        Assert.Equal(expectedMax, subject.Max);
        Assert.Equal(expectedGridLineInterval, subject.GridLineInterval);
    }

    public static TheoryData<decimal[], int, decimal, decimal, decimal> AutoScale_Data() => new() {
        { new[] { 1M, 49M }, 6, 10M, 0M, 50M },
        { new[] { 1M, 49M }, 11, 5M, 0M, 50M },
        { new[] { 50M, 100M }, 11, 5M, 50M, 100M },

        { new[] { 0.1M, 4.9M }, 6, 1M, 0M, 5M },
        { new[] { 0.1M, 4.9M }, 11, 0.5M, 0M, 5M },
        { new[] { 5M, 10M }, 11, 0.5M, 5M, 10M },

        { new[] { 0.001M, 0.049M }, 6, 0.01M, 0M, 0.05M },
        { new[] { 0.001M, 0.049M }, 11, 0.005M, 0M, 0.05M },
        { new[] { 0.05M, 0.1M }, 11, 0.005M, 0.05M, 0.1M },
    };

    [Theory]
    [MemberData(nameof(AutoScale_No_DataPoints_Data))]
    public void AutoScale_No_DataPoints(int requestedGridLineCount, decimal expectedGridLineInterval, decimal expectedMin, decimal expectedMax) {
        var subject = new XYChartBuilder()
            .WithAutoScaleSettings(isEnabled: true, includeZero: false, clearancePercentage: 0M, requestedGridLineCount: requestedGridLineCount)
            .Chart.PlotArea;

        subject.AutoScale(Array.Empty<decimal>());

        Assert.Equal(expectedMin, subject.Min);
        Assert.Equal(expectedMax, subject.Max);
        Assert.Equal(expectedGridLineInterval, subject.GridLineInterval);
    }

    public static TheoryData<int, decimal, decimal, decimal> AutoScale_No_DataPoints_Data() => new() {
        { 1, 50M, 0M, 50M }, // 2
        { 2, 20M, 0M, 60M }, // 4
        { 3, 20M, 0M, 60M }, // 4
        { 4, 10M, 0M, 50M }, // 6
        { 7, 10M, 0M, 50M }, // 6
        { 8, 5M, 0M, 50M }, // 11
        { 17, 5M, 0M, 50M }, // 11
        { 18, 2M, 0M, 50M }, // 21
        { 25, 2M, 0M, 50M }, // 21
    };

    [Theory]
    [MemberData(nameof(AutoScale_IncludeZero_Data))]
    public void AutoScale_IncludeZero(decimal[] dataPoints, decimal expectedGridLineInterval, decimal expectedMin, decimal expectedMax) {
        var subject = new XYChartBuilder()
            .WithAutoScaleSettings(isEnabled: true, includeZero: true, clearancePercentage: 0M, requestedGridLineCount: 5)
            .Chart.PlotArea;

        subject.AutoScale(dataPoints);

        Assert.Equal(expectedMin, subject.Min);
        Assert.Equal(expectedMax, subject.Max);
        Assert.Equal(expectedGridLineInterval, subject.GridLineInterval);
    }

    public static TheoryData<decimal[], decimal, decimal, decimal> AutoScale_IncludeZero_Data() => new() {
        { new[] { 20M, 22M }, 5M, 0M, 25M },
        { new[] { -20M, -22M }, 5M, -25M, 0M },
        { new[] { 22M, -22M }, 10M, -30M, 30M },
    };

    [Theory]
    [MemberData(nameof(AutoScale_ClearancePercentage_Data))]
    public void AutoScale_ClearancePercentage(decimal[] dataPoints, decimal expectedGridLineInterval, decimal expectedMin, decimal expectedMax) {
        var subject = new XYChartBuilder()
            .WithAutoScaleSettings(isEnabled: true, includeZero: false, clearancePercentage: 5M, requestedGridLineCount: 5)
            .Chart.PlotArea;

        subject.AutoScale(dataPoints);

        Assert.Equal(expectedMin, subject.Min);
        Assert.Equal(expectedMax, subject.Max);
        Assert.Equal(expectedGridLineInterval, subject.GridLineInterval);
    }

    public static TheoryData<decimal[], decimal, decimal, decimal> AutoScale_ClearancePercentage_Data() => new() {
        { new[] { 2M, 22M }, 5M, 0M, 25M },
        { new[] { -2M, -22M }, 5M, -25M, 0M },
        { new[] { 22M, -22M }, 10M, -30M, 30M },
    };

    [Fact]
    public void AutoScale_Disabled() {
        var subject = new XYChartBuilder()
            .WithAutoScaleSettings(isEnabled: false)
            .Chart.PlotArea;

        subject.AutoScale(new[] { 0.006M, 0.044M });

        Assert.Equal(PlotArea_Min, subject.Min);
        Assert.Equal(PlotArea_Max, subject.Max);
        Assert.Equal(PlotArea_GridLineInterval, subject.GridLineInterval);
    }

    [Theory]
    [MemberData(nameof(GetGridLineDataPoints_Data))]
    public void GetGridLineDataPoints(decimal min, decimal max, decimal gridLineInterval, params decimal[] expectedDataPoints) {
        var subject = new XYChartBuilder()
            .WithPlotArea(min, max, gridLineInterval)
            .Chart.PlotArea;

        Assert.Equal(expectedDataPoints, subject.GetGridLineDataPoints());
    }

    public static TheoryData<decimal, decimal, decimal, decimal[]> GetGridLineDataPoints_Data() => new() {
        { 0M, 100M, 20M,  new decimal[] { 0M, 20M, 40M, 60M, 80M, 100M } },
        { -10M, 105M, 20M,  new decimal[] { 0M, 20M, 40M, 60M, 80M, 100.0M } },
        { -1M, 0.2M, 0.2M, new decimal[] { -1M, -0.8M, -0.6M, -0.4M, -0.2M, 0M, 0.2M } }
    };
}
