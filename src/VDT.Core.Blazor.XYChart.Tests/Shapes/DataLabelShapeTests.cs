using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class DataLabelShapeTests {
    [Fact]
    public void Key() {
        var subject = new DataLabelShape(100M, 50M, "150", "example-data", 1, 2, 5);

        Assert.Equal("DataLabelShape[1,2,5]", subject.Key);
    }

    [Fact]
    public void GetAttributes() {
        var subject = new DataLabelShape(100M, 50M, "150", "example-data", 1, 2, 5);

        var result = subject.GetAttributes();

        Assert.Equal(2, result.Count());
        Assert.Equal("100", Assert.Single(result, attribute => attribute.Key == "x").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y").Value);
    }

    [Fact]
    public void GetContent() {
        var subject = new DataLabelShape(100M, 50M, "150", "example-data", 1, 2, 5);

        Assert.Equal("150", subject.GetContent());
    }
}
