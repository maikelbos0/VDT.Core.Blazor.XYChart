﻿using System;
using Xunit;

namespace BlazorPlayground.Chart.Tests;

public class PlotAreaTests {
    [Theory]
    [MemberData(nameof(AutoScaleData))]
    public void AutoScale(AutoScaleSettings settings, decimal[] dataPoints, int requestedGridLineCount, decimal expectedGridLineInterval, decimal expectedMin, decimal expectedMax) {
        var subject = new PlotArea();

        subject.AutoScale(settings, dataPoints, requestedGridLineCount);

        Assert.Equal(expectedMin, subject.Min);
        Assert.Equal(expectedMax, subject.Max);
        Assert.Equal(expectedGridLineInterval, subject.GridLineInterval);
    }

    public static TheoryData<AutoScaleSettings, decimal[], int, decimal, decimal, decimal> AutoScaleData() => new() {
        { new AutoScaleSettings() { IsEnabled = true }, Array.Empty<decimal>(), 1, 5M, 0M, 5M }, // 2
        { new AutoScaleSettings() { IsEnabled = true }, Array.Empty<decimal>(), 2, 2M, 0M, 6M }, // 4
        { new AutoScaleSettings() { IsEnabled = true }, Array.Empty<decimal>(), 3, 2M, 0M, 6M }, // 4
        { new AutoScaleSettings() { IsEnabled = true }, Array.Empty<decimal>(), 4, 1M, 0M, 5M }, // 6
        { new AutoScaleSettings() { IsEnabled = true }, Array.Empty<decimal>(), 7, 1M, 0M, 5M }, // 6
        { new AutoScaleSettings() { IsEnabled = true }, Array.Empty<decimal>(), 8, 0.5M, 0M, 5M }, // 11
        { new AutoScaleSettings() { IsEnabled = true }, Array.Empty<decimal>(), 17, 0.5M, 0M, 5M }, // 11
        { new AutoScaleSettings() { IsEnabled = true }, Array.Empty<decimal>(), 18, 0.2M, 0M, 5M }, // 21
        { new AutoScaleSettings() { IsEnabled = true }, Array.Empty<decimal>(), 25, 0.2M, 0M, 5M }, // 21
        
        { new AutoScaleSettings() { IsEnabled = true }, new[] { 0.1M, 4.9M }, 6, 1M, 0M, 5M }, // 6
        { new AutoScaleSettings() { IsEnabled = true }, new[] { 0.1M, 4.9M }, 11, 0.5M, 0M, 5M }, // 11
        { new AutoScaleSettings() { IsEnabled = true }, new[] { 0.6M, 4.4M }, 6, 1M, 0M, 5M }, // 6
        { new AutoScaleSettings() { IsEnabled = true }, new[] { 0.6M, 4.4M }, 11, 0.5M, 0.5M, 4.5M }, // 9

        { new AutoScaleSettings() { IsEnabled = true }, new[] { 0.1M, 4.9M }, 6, 1M, 0M, 5M }, // 6
        { new AutoScaleSettings() { IsEnabled = true }, new[] { 0.1M, 4.9M }, 11, 0.5M, 0M, 5M }, // 11
        { new AutoScaleSettings() { IsEnabled = true }, new[] { 0.6M, 4.4M }, 6, 1M, 0M, 5M }, // 6
        { new AutoScaleSettings() { IsEnabled = true }, new[] { 0.6M, 4.4M }, 11, 0.5M, 0.5M, 4.5M }, // 9

        { new AutoScaleSettings() { IsEnabled = true }, new[] { 0.001M, 0.049M }, 6, 0.01M, 0M, 0.05M }, // 6
        { new AutoScaleSettings() { IsEnabled = true }, new[] { 0.001M, 0.049M }, 11, 0.005M, 0M, 0.05M }, // 11
        { new AutoScaleSettings() { IsEnabled = true }, new[] { 0.006M, 0.044M }, 6, 0.01M, 0M, 0.05M }, // 6
        { new AutoScaleSettings() { IsEnabled = true }, new[] { 0.006M, 0.044M }, 11, 0.005M, 0.005M, 0.045M }, // 9

        { new AutoScaleSettings() { IsEnabled = false }, new[] { 0.006M, 0.044M }, 0, 0.5M, 0M, 5M }, // Disabled

        { new AutoScaleSettings() { IsEnabled = true, IncludeZero = true }, new[] { 20M, 22M }, 5, 5M, 0M, 25M },
        { new AutoScaleSettings() { IsEnabled = true, IncludeZero = true }, new[] { -20M, -22M }, 5, 5M, -25M, 0M },
        { new AutoScaleSettings() { IsEnabled = true, IncludeZero = true }, new[] { 22M, -22M }, 5, 10M, -30M, 30M },
    };

    [Theory]
    [MemberData(nameof(GetGridLineDataPointsData))]
    public void GetGridLineDataPoints(decimal min, decimal max, decimal gridLineInterval, params decimal[] expectedDataPoints) {
        var subject = new PlotArea() {
            Min = min,
            Max = max,
            GridLineInterval = gridLineInterval
        };

        Assert.Equal(expectedDataPoints, subject.GetGridLineDataPoints());
    }

    public static TheoryData<decimal, decimal, decimal, decimal[]> GetGridLineDataPointsData() => new() {
        { 0M, 100M, 20M,  new decimal[] { 0M, 20M, 40M, 60M, 80M, 100M } },
        { -10M, 105M, 20M,  new decimal[] { 0M, 20M, 40M, 60M, 80M, 100.0M } },
        { -1M, 0.2M, 0.2M, new decimal[] { -1M, -0.8M, -0.6M, -0.4M, -0.2M, 0M, 0.2M } }
    };
}
