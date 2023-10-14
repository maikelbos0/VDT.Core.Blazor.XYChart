using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

public abstract class LayerBase : ChildComponentBase, IDisposable {
    public static bool DefaultIsStacked { get; set; } = false;
    public static bool DefaultShowDataLabels { get; set; } = false;

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool IsStacked { get; set; } = DefaultIsStacked;
    [Parameter] public bool ShowDataLabels { get; set; } = DefaultShowDataLabels;
    internal List<DataSeries> DataSeries { get; set; } = new();
    public abstract StackMode StackMode { get; }
    public abstract DataPointSpacingMode DefaultDataPointSpacingMode { get; }
    public abstract bool NullAsZero { get; }

    protected override void OnInitialized() => Chart.AddLayer(this);

    public void Dispose() {
        Chart.RemoveLayer(this);
        GC.SuppressFinalize(this);
    }

    internal void AddDataSeries(DataSeries dataSeries) {
        if (!DataSeries.Contains(dataSeries)) {
            DataSeries.Add(dataSeries);
        }

        Chart.HandleStateChange();
    }

    internal void RemoveDataSeries(DataSeries dataSeries) {
        DataSeries.Remove(dataSeries);
        Chart.HandleStateChange();
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenComponent<CascadingValue<LayerBase>>(1);
        builder.AddAttribute(2, "Value", this);
        builder.AddAttribute(3, "ChildContent", ChildContent);
        builder.CloseComponent();
    }

    public abstract IEnumerable<ShapeBase> GetDataSeriesShapes();

    public IEnumerable<DataLabelShape> GetDataLabelShapes() {
        var layerIndex = Chart.Layers.IndexOf(this);

        if (ShowDataLabels) {
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

    public IEnumerable<decimal> GetScaleDataPoints() {
        var dataPointTransformer = GetDataPointTransformer();

        return DataSeries.SelectMany(dataSeries => dataSeries.GetDataPoints()
            .Select(value => dataPointTransformer(value.DataPoint, value.Index)));
    }

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
                    throw new NotImplementedException($"Missing stacked transformer implementation for {nameof(StackMode)}{StackMode}");
            }
        }
        else {
            return (dataPoint, index) => dataPoint;
        }
    }
}
