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
    public void HaveParametersChanged(LegendPosition position, int height, int itemWidth, int itemHeight, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(Legend.Position), position },
            { nameof(Legend.Height), height },
            { nameof(Legend.ItemWidth), itemWidth },
            { nameof(Legend.ItemHeight), itemHeight }
        });

        var subject = new Legend {
            Position = LegendPosition.None,
            Height = 25,
            ItemWidth = 100,
            ItemHeight = 25
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }
}
