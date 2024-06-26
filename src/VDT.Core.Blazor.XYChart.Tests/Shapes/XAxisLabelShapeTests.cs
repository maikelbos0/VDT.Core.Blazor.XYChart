﻿using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class XAxisLabelShapeTests {
    [Fact]
    public void Key() {
        var subject = new XAxisLabelShape(100M, 50M, "Foo", 2);

        Assert.Equal($"{nameof(XAxisLabelShape)}[2]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new XAxisLabelShape(100M, 50M, "Foo", 2);

        var result = subject.GetAttributes();

        Assert.Equal(4, result.Count());
        Assert.Equal("100", Assert.Single(result, attribute => attribute.Key == "x").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y").Value);
        Assert.Equal("middle", Assert.Single(result, attribute => attribute.Key == "text-anchor").Value);
        Assert.Equal("hanging", Assert.Single(result, attribute => attribute.Key == "dominant-baseline").Value);
    }

    [Fact]
    public void GetContent() {
        var subject = new XAxisLabelShape(100M, 50M, "Foo", 2);

        Assert.Equal("Foo", subject.GetContent());
    }
}
