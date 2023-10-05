using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

public static class DefaultDataMarkerTypes {
    public static ShapeBase Round(decimal x, decimal y, decimal size, string color, string? cssClass, int layerIndex, int dataSeriesIndex, int dataPointIndex)
        => new RoundDataMarkerShape(x, y, size, color, cssClass, layerIndex, dataSeriesIndex, dataPointIndex);

    public static ShapeBase Square(decimal x, decimal y, decimal size, string color, string? cssClass, int layerIndex, int dataSeriesIndex, int dataPointIndex)
        => new SquareDataMarkerShape(x, y, size, color, cssClass, layerIndex, dataSeriesIndex, dataPointIndex);
}
