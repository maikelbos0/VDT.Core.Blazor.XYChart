﻿using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class YAxisMultiplierShapeTests {
    [Fact]
    public void Key() {
        var subject = new YAxisMultiplierShape(100M, 50M, "x 1.000");

        Assert.Equal($"{nameof(YAxisMultiplierShape)}[]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new YAxisMultiplierShape(100M, 50M, "x 1.000");

        var result = subject.GetAttributes();

        Assert.Equal(2, result.Count());
        Assert.Equal("100", Assert.Single(result, attribute => attribute.Key == "x").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y").Value);
    }

    [Fact]
    public void GetContent() {
        var subject = new YAxisMultiplierShape(100M, 50M, "x 1.000");

        Assert.Equal("x 1.000", subject.GetContent());
    }
}
