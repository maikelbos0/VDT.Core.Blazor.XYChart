﻿using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class PlotAreaShapeTests {
    [Fact]
    public void Key() {
        var subject = new PlotAreaShape(300, 200, 20, 50, 130, 90);

        Assert.Equal($"{nameof(PlotAreaShape)}[]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new PlotAreaShape(300, 200, 20, 50, 130, 90);

        var result = subject.GetAttributes();

        Assert.Equal(4, result.Count());
        Assert.Equal("evenodd", Assert.Single(result, attribute => attribute.Key == "fill-rule").Value);
        Assert.Equal("M-100 -100 l500 0 l0 400 l-500 0 Z M20 50 l130 0 l0 90 l-130 0 Z", Assert.Single(result, attribute => attribute.Key == "d").Value);
        Assert.Equal("grey", Assert.Single(result, attribute => attribute.Key == "stroke").Value);
        Assert.Equal("white", Assert.Single(result, attribute => attribute.Key == "fill").Value);
    }
}
