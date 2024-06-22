using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class YAxisLabelShapeTests {
    [Fact]
    public void Key() {
        var subject = new YAxisLabelShape(100M, 50M, "150", 2);

        Assert.Equal($"{nameof(YAxisLabelShape)}[2]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new YAxisLabelShape(100M, 50M, "150", 2);

        var result = subject.GetAttributes();

        Assert.Equal(4, result.Count());
        Assert.Equal("100", Assert.Single(result, attribute => attribute.Key == "x").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y").Value);
        Assert.Equal("end", Assert.Single(result, attribute => attribute.Key == "text-anchor").Value);
        Assert.Equal("middle", Assert.Single(result, attribute => attribute.Key == "dominant-baseline").Value);
    }

    [Fact]
    public void GetContent() {
        var subject = new YAxisLabelShape(100M, 50M, "150", 2);

        Assert.Equal("150", subject.GetContent());
    }
}
