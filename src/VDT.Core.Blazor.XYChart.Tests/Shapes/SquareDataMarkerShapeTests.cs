﻿using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class SquareDataMarkerShapeTests {
    [Fact]
    public void Key() {
        var subject = new SquareDataMarkerShape(150, 50, 20, "red", "example-data", 1, 2, 5);

        Assert.Equal($"{nameof(SquareDataMarkerShape)}[1,2,5]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new SquareDataMarkerShape(150, 50, 20, "red", "example-data", 1, 2, 5);

        var result = subject.GetAttributes();

        Assert.Equal(5, result.Count());
        Assert.Equal("140", Assert.Single(result, attribute => attribute.Key == "x").Value);
        Assert.Equal("40", Assert.Single(result, attribute => attribute.Key == "y").Value);
        Assert.Equal("20", Assert.Single(result, attribute => attribute.Key == "width").Value);
        Assert.Equal("20", Assert.Single(result, attribute => attribute.Key == "height").Value);
        Assert.Equal("red", Assert.Single(result, attribute => attribute.Key == "fill").Value);
    }
}
