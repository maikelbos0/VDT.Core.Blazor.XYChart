using System;
using System.Threading.Tasks;

namespace VDT.Core.Blazor.XYChart;

internal interface IBoundingBoxProvider : IAsyncDisposable {
    Task<BoundingBox> GetBoundingBox(string text, string? cssClass);
}
