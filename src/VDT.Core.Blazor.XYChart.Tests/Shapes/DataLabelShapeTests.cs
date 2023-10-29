using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests.Shapes;

public class DataLabelShapeTests {
    [Fact]
    public void Key() {
        var subject = new DataLabelShape(100M, 50M, "150", "example-data", true, 1, 2, 5);

        Assert.Equal($"{nameof(DataLabelShape)}[1,2,5]", subject.Key);
    }

    [Theory]
    [InlineData(true, "data-positive")]
    [InlineData(false, "data-negative")]
    public void GetAttributes(bool isPositive, string signAttribute) {
        var subject = new DataLabelShape(100M, 50M, "1  50", "example-data", isPositive, 1, 2, 5);

        var result = subject.GetAttributes();

        Assert.Equal(3, result.Count());
        Assert.Equal("100", Assert.Single(result, attribute => attribute.Key == "x").Value);
        Assert.Equal("50", Assert.Single(result, attribute => attribute.Key == "y").Value);
        Assert.True(Assert.IsType<bool>(Assert.Single(result, attribute => attribute.Key == signAttribute).Value));
    }

    [Fact]
    public void GetContent() {
        var subject = new DataLabelShape(100M, 50M, "150", "example-data", true, 1, 2, 5);

        Assert.Equal("150", subject.GetContent());
    }
}
