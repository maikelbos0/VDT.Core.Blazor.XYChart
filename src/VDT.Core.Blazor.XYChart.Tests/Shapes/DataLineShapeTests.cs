﻿using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class DataLineShapeTests {
    [Fact]
    public void Key() {
        var subject = new DataLineShape(new[] { "M 20 50", "L 80 90" }, 5M, "red", "example-data", 2);

        Assert.Equal("DataLineShape[2]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new DataLineShape(new[] { "M 20 50", "L 80 90" }, 5M, "red", "example-data", 2);

        var result = subject.GetAttributes();

        Assert.Equal(3, result.Count());
        Assert.Equal("M 20 50 L 80 90", Assert.Single(result, attribute => attribute.Key == "d").Value);
        Assert.Equal("5", Assert.Single(result, attribute => attribute.Key == "stroke-width").Value);
        Assert.Equal("red", Assert.Single(result, attribute => attribute.Key == "stroke").Value);
    }
}
