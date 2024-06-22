using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class LineDataShapeTests {
    [Fact]
    public void Key() {
        var subject = new LineDataShape(new[] { "M 20 50", "L 80 90" }, "red", "example-data", 1, 2);

        Assert.Equal($"{nameof(LineDataShape)}[1,2]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new LineDataShape(new[] { "M 20 50", "L 80 90" }, "red", "example-data", 1, 2);

        var result = subject.GetAttributes();

        Assert.Equal(3, result.Count());
        Assert.Equal("M 20 50 L 80 90", Assert.Single(result, attribute => attribute.Key == "d").Value);
        Assert.Equal("red", Assert.Single(result, attribute => attribute.Key == "stroke").Value);
        Assert.Equal("none", Assert.Single(result, attribute => attribute.Key == "fill").Value);
    }
}
