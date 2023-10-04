using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

public delegate ShapeBase DataMarkerDelegate(decimal x, decimal y, decimal size, string color, string? cssClass, int layerIndex, int dataSeriesIndex, int dataPointIndex);
