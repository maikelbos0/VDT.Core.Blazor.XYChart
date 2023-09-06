using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class DefaultDataMarkerTypesTests {
    [Fact]
    public void Round() {
        var result = DefaultDataMarkerTypes.Round(50M, 150M, 10M, "red", "example-data", 1, 5);

        var shape = Assert.IsType<RoundDataMarkerShape>(result);

        Assert.Equal(50M, shape.X);
        Assert.Equal(150M, shape.Y);
        Assert.Equal(10M, shape.Size);
        Assert.Equal("red", shape.Color);
        Assert.Equal($"{nameof(RoundDataMarkerShape)}[1,5]", shape.Key);
        Assert.Equal("data-marker data-marker-round example-data", shape.CssClass);
    }

    [Fact]
    public void Square() {
        var result = DefaultDataMarkerTypes.Square(50M, 150M, 10M, "red", "example-data", 1, 5);

        var shape = Assert.IsType<SquareDataMarkerShape>(result);

        Assert.Equal(50M, shape.X);
        Assert.Equal(150M, shape.Y);
        Assert.Equal(10M, shape.Size);
        Assert.Equal("red", shape.Color);
        Assert.Equal($"{nameof(SquareDataMarkerShape)}[1,5]", shape.Key);
        Assert.Equal("data-marker data-marker-square example-data", shape.CssClass);
    }
}
