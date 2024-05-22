using VDT.Core.Blazor.XYChart.Shapes;
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

    [Fact]
    public void QuadraticBezierTo() {
        Assert.Equal("Q 2.5 3.5, 5.5 7.5", PathCommandFactory.QuadraticBezierTo(2.5M, 3.5M, 5.5M, 7.5M));
    }

    [Fact]
    public void CubicBezierTo() {
        Assert.Equal("C 2.5 3.5, 3.2 4.2, 5.5 7.5", PathCommandFactory.CubicBezierTo(2.5M, 3.5M, 3.2M, 4.2M, 5.5M, 7.5M));
    }
}
