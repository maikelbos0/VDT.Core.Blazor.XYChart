using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Series of data points to be displayed in a layer in an <see cref="XYChart"/>
/// </summary>
public class DataSeries : ChildComponentBase, IDisposable {
    /// <summary>
    /// Color that is applied to a data series if no <see cref="Color"/> was provided and <see cref="DefaultColors"/> is empty
    /// </summary>
    public const string FallbackColor = "#000000";

    /// <summary>
    /// Gets or sets the default colors for data series without a <see cref="Color"/>; the color is chosen by determining the data series index within the 
    /// entire <see cref="XYChart"/>, using modulus if required
    /// </summary>
    public static List<string> DefaultColors { get; set; } = new() {
        "#ff9933",
        "#11bbdd",
        "#aa66ee",
        "#22cc55",
        "#3366bb",
        "#ee4411",
        "#ffcc11",
        "#dd3377"
    };

    [CascadingParameter] internal LayerBase Layer { get; set; } = null!;

    /// <summary>
    /// Gets or sets the name of the data series to be displayed in the legend
    /// </summary>
    [Parameter] public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the color to be used for shapes representing this data series; if null a color will be determined automatically
    /// </summary>
    [Parameter] public string? Color { get; set; }

    /// <summary>
    /// Gets or sets the data point values that make up this data series; data points are matched to category labels in the <see cref="XYChart"/> by index
    /// </summary>
    [Parameter] public IList<decimal?> DataPoints { get; set; } = new List<decimal?>();

    /// <summary>
    /// Gets or sets the CSS class applied to shapes for this data series
    /// </summary>
    [Parameter] public string? CssClass { get; set; }

    /// <inheritdoc/>
    protected override void OnInitialized() => Layer.AddDataSeries(this);

    /// <inheritdoc/>
    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(Name)
        || parameters.HasParameterChanged(Color)
        || parameters.HasParameterChanged(DataPoints)
        || parameters.HasParameterChanged(CssClass);

    /// <inheritdoc/>
    public void Dispose() {
        Layer.RemoveDataSeries(this);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Gets the value and index of data points that should be rendered for this data series, taking into account the category label count and layer settings
    /// </summary>
    /// <returns>A collection of data point values and indexes</returns>
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

    /// <summary>
    /// Determines the color to be used for this data series based on <see cref="Color"/>, <see cref="DefaultColors"/> and <see cref="FallbackColor"/>
    /// </summary>
    /// <returns>The data series color</returns>
    public string GetColor() {
        if (Color != null) {
            return Color;
        }

        var index = Chart.Layers.TakeWhile(layer => layer != Layer).Sum(layer => layer.DataSeries.Count) + Layer.DataSeries.IndexOf(this);

        if (DefaultColors.Any() && index >= 0) {
            return DefaultColors[index % DefaultColors.Count];
        }

        return FallbackColor;
    }
}
