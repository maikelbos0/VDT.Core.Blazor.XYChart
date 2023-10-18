using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class LegendTests {
    [Theory]
    [InlineData(LegendPosition.None, 25, 100, 25, false)]
    [InlineData(LegendPosition.Top, 25, 100, 25, true)]
    [InlineData(LegendPosition.None, 50, 100, 25, true)]
    [InlineData(LegendPosition.None, 25, 50, 25, true)]
    [InlineData(LegendPosition.None, 25, 100, 10, true)]
    public void HaveParametersChanged(LegendPosition legendPosition, int legendHeight, int legendItemWidth, int legendItemHeight, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(Legend.LegendPosition), legendPosition },
            { nameof(Legend.LegendHeight), legendHeight },
            { nameof(Legend.LegendItemWidth), legendItemWidth },
            { nameof(Legend.LegendItemHeight), legendItemHeight }
        });

        var subject = new Legend {
            LegendPosition = LegendPosition.None,
            LegendHeight = 25,
            LegendItemWidth = 100,
            LegendItemHeight = 25
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }
}
