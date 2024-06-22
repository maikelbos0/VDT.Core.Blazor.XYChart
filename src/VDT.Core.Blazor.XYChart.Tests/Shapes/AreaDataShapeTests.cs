using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class AreaDataShapeTests {
    [Fact]
    public void Key() {
        var subject = new AreaDataShape(["M 20 50", "L 80 90", "L 20 90 Z"], "red", "example-data", 1, 2);

        Assert.Equal($"{nameof(AreaDataShape)}[1,2]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new AreaDataShape(["M 20 50", "L 80 90", "L 20 90 Z"], "red", "example-data", 1, 2);

        var result = subject.GetAttributes();

        Assert.Equal(2, result.Count());
        Assert.Equal("M 20 50 L 80 90 L 20 90 Z", Assert.Single(result, attribute => attribute.Key == "d").Value);
        Assert.Equal("red", Assert.Single(result, attribute => attribute.Key == "fill").Value);
    }
}
