using System.Collections.Generic;

namespace VDT.Core.Blazor.XYChart;

public record class CanvasDataSeries(string Color, string? CssClass, int Index, IList<CanvasDataPoint> DataPoints);
