﻿using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class PathCommandFactoryTests {
    [Fact]
    public void MoveTo() {
        Assert.Equal("M 5.5 7.5", PathCommandFactory.MoveTo(5.5M, 7.5M));
    }

    [Fact]
    public void LineTo() {
        Assert.Equal("L 5.5 7.5", PathCommandFactory.LineTo(5.5M, 7.5M));
    }
}
