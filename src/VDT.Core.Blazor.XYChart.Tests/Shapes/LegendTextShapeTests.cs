using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class LegendTextShapeTests {
    [Fact]
    public void Key() {
        var subject = new LegendTextShape(100M, 50M, "Foo", "example-data", 1, 2);

        Assert.Equal($"{nameof(LegendTextShape)}[1,2]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new LegendTextShape(100M, 50M, "Foo", "example-data", 1, 2);

        var result = subject.GetAttributes();

        Assert.Equal(3, result.Count());
        Assert.Equal("100", Assert.Single(result, attribute => attribute.Key == "x").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y").Value);
        Assert.Equal("middle", Assert.Single(result, attribute => attribute.Key == "dominant-baseline").Value);
    }

    [Fact]
    public void GetContent() {
        var subject = new LegendTextShape(100M, 50M, "Foo", "example-data", 1, 2);

        Assert.Equal("Foo", subject.GetContent());
    }
}
