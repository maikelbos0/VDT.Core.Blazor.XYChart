using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class BarDataShapeTests {
    [Fact]
    public void Key() {
        var subject = new BarDataShape(20, 50, 80, 90, "red", "example-data", 2, 5);

        Assert.Equal($"{nameof(BarDataShape)}[2,5]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new BarDataShape(20, 50, 80, 90, "red", "example-data", 2, 5);

        var result = subject.GetAttributes();

        Assert.Equal(5, result.Count());
        Assert.Equal("20", Assert.Single(result, attribute => attribute.Key == "x").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y").Value);
        Assert.Equal("80", Assert.Single(result, attribute => attribute.Key == "width").Value);
        Assert.Equal("90", Assert.Single(result, attribute => attribute.Key == "height").Value);
        Assert.Equal("red", Assert.Single(result, attribute => attribute.Key == "fill").Value);
    }

    [Fact]
    public void GetAttributes_Negative_Height() {
        var subject = new BarDataShape(20, 140, 80, -90, "red", "example-data", 2, 5);

        var result = subject.GetAttributes();

        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y").Value);
        Assert.Equal("90", Assert.Single(result, attribute => attribute.Key == "height").Value);
    }
}
