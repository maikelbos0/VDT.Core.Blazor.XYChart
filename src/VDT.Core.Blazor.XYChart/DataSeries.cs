using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VDT.Core.Blazor.XYChart;

public class DataSeries : ChildComponentBase, IDisposable {
    public const string FallbackColor = "#000000";

    public static List<string> DefaultColors { get; set; } = new() {
        // https://coolors.co/550527-688e26-faa613-f44708-a10702
        // "#550527", "#688e26", "#faa613", "#f44708", "#a10702"

        // https://coolors.co/264653-2a9d8f-e9c46a-f4a261-e76f51
        // "#264653", "#2a9d8f", "#e9c46a", "#f4a261", "#e76f51"
        
        // https://coolors.co/2274a5-f75c03-f1c40f-d90368-00cc66
        "#2274a5", "#f75c03", "#f1c40f", "#d90368", "#00cc66"
    };

    [CascadingParameter] internal LayerBase Layer { get; set; } = null!;
    [Parameter] public string? Name { get; set; }
    [Parameter] public string? Color { get; set; }
    [Parameter] public List<decimal?> DataPoints { get; set; } = new();
    [Parameter] public string? CssClass { get; set; }

    protected override void OnInitialized() => Layer.AddDataSeries(this);

    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(Name)
        || parameters.HasParameterChanged(Color)
        || parameters.HasParameterChanged(DataPoints)
        || parameters.HasParameterChanged(CssClass);

    public void Dispose() {
        Layer.RemoveDataSeries(this);
        GC.SuppressFinalize(this);
    }

    public IEnumerable<(int Index, decimal DataPoint)> GetDataPoints() {
        var dataPoints = DataPoints.Take(Chart.Labels.Count);

        if (DataPoints.Count < Chart.Labels.Count) {
            dataPoints = dataPoints.Concat(Enumerable.Repeat<decimal?>(null, Chart.Labels.Count - DataPoints.Count));
        }

        if (Layer.NullAsZero) {
            return dataPoints.Select((dataPoint, index) => (index, dataPoint ?? 0M));
        }
        else {
            return dataPoints
                .Select((dataPoint, index) => (index, dataPoint))
                .Where(value => value.dataPoint != null)
                .Select(value => (value.index, value.dataPoint!.Value));
        }
    }

    public string GetColor() {
        if (Color != null) {
            return Color;
        }

        var index = Layer.DataSeries.IndexOf(this);

        if (DefaultColors.Any() && index >= 0) {
            return DefaultColors[index % DefaultColors.Count];
        }

        return FallbackColor;
    }
}
