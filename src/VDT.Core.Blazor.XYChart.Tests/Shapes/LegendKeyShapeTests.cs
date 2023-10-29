using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class LegendKeyShapeTests {
    [Fact]
    public void Key() {
        var subject = new LegendKeyShape(20, 50, 80, "red", "example-data", 1, 2);

        Assert.Equal($"{nameof(LegendKeyShape)}[1,2]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new LegendKeyShape(20, 50, 80, "red", "example-data", 1, 2);

        var result = subject.GetAttributes();

        Assert.Equal(5, result.Count());
        Assert.Equal("20", Assert.Single(result, attribute => attribute.Key == "x").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y").Value);
        Assert.Equal("80", Assert.Single(result, attribute => attribute.Key == "width").Value);
        Assert.Equal("80", Assert.Single(result, attribute => attribute.Key == "height").Value);
        Assert.Equal("red", Assert.Single(result, attribute => attribute.Key == "fill").Value);
    }
}
