using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Base class for defining a layer in an <see cref="XYChart"/> that defines the layout of the data series it contains
/// </summary>
public abstract class LayerBase : ChildComponentBase, IDisposable {
    /// <summary>
    /// Gets or sets the default value for whether or not the data series should be stacked
    /// </summary>
    public static bool DefaultIsStacked { get; set; } = false;

    /// <summary>
    /// Gets or sets the default value for whether or not to show value labels at data points
    /// </summary>
    public static bool DefaultShowDataLabels { get; set; } = false;

    /// <summary>
    /// Gets or sets the content containing data series components
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets whether or not the data series should be stacked
    /// </summary>
    [Parameter] public bool IsStacked { get; set; } = DefaultIsStacked;

    /// <summary>
    /// Gets or sets whether or not to show value labels at data points
    /// </summary>
    [Parameter] public bool ShowDataLabels { get; set; } = DefaultShowDataLabels;

    internal List<DataSeries> DataSeries { get; set; } = new();

    /// <summary>
    /// Gets the way data points are stacked if stacking is enabled
    /// </summary>
    public abstract StackMode StackMode { get; }

    /// <summary>
    /// Gets the way data points are spaced out over the plot area
    /// </summary>
    public abstract DataPointSpacingMode DefaultDataPointSpacingMode { get; }

    /// <summary>
    /// Gets whether a null data point value should be interpreted as a zero value or skipped
    /// </summary>
    public abstract bool NullAsZero { get; }

    /// <inheritdoc/>
    protected override void OnInitialized() => Chart.AddLayer(this);

    /// <inheritdoc/>
    public void Dispose() {
        Chart.RemoveLayer(this);
        GC.SuppressFinalize(this);
    }

    internal async Task AddDataSeries(DataSeries dataSeries) {
        DataSeries.Add(dataSeries);
        Chart.StateHasChanged();
    }

    internal async Task RemoveDataSeries(DataSeries dataSeries) {
        DataSeries.Remove(dataSeries);
        Chart.StateHasChanged();
    }

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenComponent<CascadingValue<LayerBase>>(1);
        builder.AddAttribute(2, "Value", this);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();
    }

    /// <summary>
    /// Gets the SVG shapes that make up the data series in this layer
    /// </summary>
    /// <returns>The SVG shapes</returns>
    public abstract IEnumerable<ShapeBase> GetDataSeriesShapes();

    /// <summary>
    /// Gets the SVG shapes to display data value labels for this layer, if applicable
    /// </summary>
    /// <returns>The SVG shapes</returns>
    public IEnumerable<DataLabelShape> GetDataLabelShapes() {
        if (ShowDataLabels) {
            var layerIndex = Chart.Layers.IndexOf(this);

            return GetCanvasDataSeries().SelectMany(canvasDataSeries => canvasDataSeries.DataPoints.Select(dataPoint => new DataLabelShape(
                dataPoint.X,
                dataPoint.Y,
                dataPoint.Value.ToString(Chart.Canvas.DataLabelFormat),
                canvasDataSeries.CssClass,
                dataPoint.Value >= 0,
                layerIndex,
                canvasDataSeries.Index,
                dataPoint.Index
            )));
        }
        else {
            return Enumerable.Empty<DataLabelShape>();
        }
    }

    /// <summary>
    /// Gets a collection of data series information for displaying data points on a canvas
    /// </summary>
    /// <returns>Data series information</returns>
    public virtual IEnumerable<CanvasDataSeries> GetCanvasDataSeries() {
        var dataPointTransformer = GetDataPointTransformer();

        return DataSeries.Select((dataSeries, index) => new CanvasDataSeries(
            dataSeries.GetColor(),
            dataSeries.CssClass,
            index,
            dataSeries.GetDataPoints().Select(value => new CanvasDataPoint(
                Chart.MapDataIndexToCanvas(value.Index),
                Chart.MapDataPointToCanvas(dataPointTransformer(value.DataPoint, value.Index)),
                Chart.MapDataValueToPlotArea(value.DataPoint),
                0, // By default, data point shapes don't require a width
                value.Index,
                (value.DataPoint / Chart.PlotArea.Multiplier)
            )).ToList())
        ).ToList();
    }

    /// <summary>
    /// Gets a collection of legend items for this layer
    /// </summary>
    /// <returns>The legend items</returns>
    public IEnumerable<LegendItem> GetLegendItems() {
        var layerIndex = Chart.Layers.IndexOf(this);

        return DataSeries.Select((dataSeries, index) => new LegendItem(dataSeries.GetColor(), dataSeries.Name ?? "?", dataSeries.CssClass, layerIndex, index));
    }

    /// <summary>
    /// Gets all data points in this layer for the purpose of scaling the plot area
    /// </summary>
    /// <returns>All data points</returns>
    public IEnumerable<decimal> GetScaleDataPoints() {
        var dataPointTransformer = GetDataPointTransformer();

        return DataSeries.SelectMany(dataSeries => dataSeries.GetDataPoints()
            .Select(value => dataPointTransformer(value.DataPoint, value.Index)));
    }

    /// <summary>
    /// Gets a transformer method for transforming data point values for stacking
    /// </summary>
    /// <returns>The transformer method</returns>
    /// <exception cref="NotImplementedException">Thrown when the layer uses an unknown <see cref="StackMode"/></exception>
    protected Func<decimal, int, decimal> GetDataPointTransformer() {
        if (IsStacked) {
            switch (StackMode) {
                case StackMode.Single:
                    var offsets = new decimal[Chart.Labels.Count];

                    return (dataPoint, index) => offsets[index] += dataPoint;
                case StackMode.Split:
                    var negativeOffsets = new decimal[Chart.Labels.Count];
                    var positiveOffsets = new decimal[Chart.Labels.Count];

                    return (dataPoint, index) => (dataPoint < 0 ? negativeOffsets : positiveOffsets)[index] += dataPoint;
                default:
                    throw new NotImplementedException($"No implementation found for {nameof(StackMode)} '{StackMode}'.");
            }
        }
        else {
            return (dataPoint, index) => dataPoint;
        }
    }
}
