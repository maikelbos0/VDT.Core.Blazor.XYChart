using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Xunit;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class LegendTests {
    [Theory]
    [InlineData(75, 10)]
    [InlineData(100, 7)]
    [InlineData(150, 5)]
    [InlineData(151, 4)]
    public void ItemsPerRow(int itemWidth, int expectedResult) {
        var subject = new XYChartBuilder()
            .WithLegend(itemWidth: itemWidth)
            .Chart.Legend;

        Assert.Equal(expectedResult, subject.ItemsPerRow);
    }

    [Theory]
    [InlineData(1, Legend_ItemHeight)]
    [InlineData(7, Legend_ItemHeight)]
    [InlineData(8, 2 * Legend_ItemHeight)]
    [InlineData(14, 2 * Legend_ItemHeight)]
    [InlineData(15, 3 * Legend_ItemHeight)]
    public void Height(int dataSeriesCount, int expectedResult) {
        var builder = new XYChartBuilder()
            .WithLayer<BarLayer>();

        for (int i = 0; i < dataSeriesCount; i++) {
            builder.WithDataSeries();
        }

        var subject = builder.Chart.Legend;

        Assert.Equal(expectedResult, subject.Height);
    }

    [Theory]
    [InlineData(true, LegendPosition.Top, LegendAlignment.Center, 100, 25, 16, false)]
    [InlineData(false, LegendPosition.Top, LegendAlignment.Center, 100, 25, 16, true)]
    [InlineData(true, LegendPosition.Bottom, LegendAlignment.Center, 100, 25, 16, true)]
    [InlineData(true, LegendPosition.Top, LegendAlignment.Left, 100, 25, 16, true)]
    [InlineData(true, LegendPosition.Top, LegendAlignment.Center, 75, 25, 16, true)]
    [InlineData(true, LegendPosition.Top, LegendAlignment.Center, 100, 15, 16, true)]
    [InlineData(true, LegendPosition.Top, LegendAlignment.Center, 100, 25, 20, true)]
    public void HaveParametersChanged(bool isEnabled, LegendPosition position, LegendAlignment alignment, int itemWidth, int itemHeight, int keySize, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(Legend.IsEnabled), isEnabled },
            { nameof(Legend.Position), position },
            { nameof(Legend.Alignment), alignment },
            { nameof(Legend.ItemWidth), itemWidth },
            { nameof(Legend.ItemHeight), itemHeight },
            { nameof(Legend.KeySize), keySize }
        });

        var subject = new Legend() {
            IsEnabled = true,
            Position = LegendPosition.Top,
            Alignment = LegendAlignment.Center,
            ItemWidth = 100,
            ItemHeight = 25,
            KeySize = 16
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }
}
