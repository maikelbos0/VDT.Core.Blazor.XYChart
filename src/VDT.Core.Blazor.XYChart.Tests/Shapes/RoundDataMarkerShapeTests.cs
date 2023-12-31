﻿using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class RoundDataMarkerShapeTests {
    [Fact]
    public void Key() {
        var subject = new RoundDataMarkerShape(150, 50, 20, "red", "example-data", 1, 2, 5);

        Assert.Equal($"{nameof(RoundDataMarkerShape)}[1,2,5]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new RoundDataMarkerShape(150, 50, 20, "red", "example-data", 1, 2, 5);

        var result = subject.GetAttributes();

        Assert.Equal(4, result.Count());
        Assert.Equal("150", Assert.Single(result, attribute => attribute.Key == "cx").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "cy").Value);
        Assert.Equal("10", Assert.Single(result, attribute => attribute.Key == "r").Value);
        Assert.Equal("red", Assert.Single(result, attribute => attribute.Key == "fill").Value);
    }
}
