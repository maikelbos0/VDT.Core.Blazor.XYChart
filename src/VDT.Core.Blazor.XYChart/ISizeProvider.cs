using System;
using System.Threading.Tasks;

namespace VDT.Core.Blazor.XYChart;

internal interface ISizeProvider : IAsyncDisposable {
    Task<BoundingBox> GetTextSize(string text, string? cssClass);
}
