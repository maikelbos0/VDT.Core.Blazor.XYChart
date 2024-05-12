using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class BoundingBoxTests {
    [Theory]
    [MemberData(nameof(RequiredWidth_Data))]
    public void RequiredWidth(decimal x, decimal width, int expectedResult) {
        var subject = new BoundingBox(x, 5, width, 25);

        Assert.Equal(expectedResult, subject.RequiredWidth);
    }

    public static TheoryData<decimal, decimal, int> RequiredWidth_Data() => new() {
        { 31.1M, 21.1M, 32 },
        { -31.1M, 21.1M, 32 },
        { 11.1M, 11.2M, 23 },
        { -11.1M, 11.2M, 23 }
    };

    [Theory]
    [MemberData(nameof(RequiredHeight_Data))]
    public void RequiredHeight(decimal y, decimal height, int expectedResult) {
        var subject = new BoundingBox(5, y, 25, height);

        Assert.Equal(expectedResult, subject.RequiredHeight);
    }

    public static TheoryData<decimal, decimal, int> RequiredHeight_Data() => new() {
        { 31.1M, 21.1M, 32 },
        { -31.1M, 21.1M, 32 },
        { 11.1M, 11.2M, 23 },
        { -11.1M, 11.2M, 23 }
    };
}
