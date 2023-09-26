using System.Collections.ObjectModel;

namespace VDT.Core.Blazor.XYChart;

public record class CanvasDataSeries(string Color, string? CssClass, int Index, ReadOnlyCollection<CanvasDataPoint> DataPoints);
