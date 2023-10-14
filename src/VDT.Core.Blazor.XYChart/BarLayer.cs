using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

public class BarLayer : LayerBase {
    public static decimal DefaultClearancePercentage { get; set; } = 10M;
    public static decimal DefaultGapPercentage { get; set; } = 5M;

    public override StackMode StackMode => StackMode.Split;
    public override DataPointSpacingMode DefaultDataPointSpacingMode => DataPointSpacingMode.Center;
    public override bool NullAsZero => false;

    [Parameter] public decimal ClearancePercentage { get; set; } = DefaultClearancePercentage;
    [Parameter] public decimal GapPercentage { get; set; } = DefaultGapPercentage;

    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(IsStacked)
        || parameters.HasParameterChanged(ShowDataLabels)
        || parameters.HasParameterChanged(ClearancePercentage)
        || parameters.HasParameterChanged(GapPercentage);

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var layerIndex = Chart.Layers.IndexOf(this);

        foreach (var canvasDataSeries in GetCanvasDataSeries()) {
            foreach (var canvasDataPoint in canvasDataSeries.DataPoints) {
                yield return new BarDataShape(
                    canvasDataPoint.X - canvasDataPoint.Width / 2M,
                    canvasDataPoint.Y,
                    canvasDataPoint.Width,
                    canvasDataPoint.Height,
                    canvasDataSeries.Color,
                    canvasDataSeries.CssClass,
                    layerIndex,
                    canvasDataSeries.Index,
                    canvasDataPoint.Index
                );
            }
        }
    }

    public override IEnumerable<CanvasDataSeries> GetCanvasDataSeries() {
        var dataPointTransformer = GetDataPointTransformer();
        var width = Chart.GetDataPointWidth() / 100M * (100M - ClearancePercentage * 2);
        var offsetProvider = (int dataSeriesIndex) => 0M;

        if (!IsStacked && DataSeries.Any()) {
            var gapWidth = Chart.GetDataPointWidth() / 100M * GapPercentage;
            var dataSeriesWidth = (width - gapWidth * (DataSeries.Count - 1)) / DataSeries.Count;

            width = (width - gapWidth * (DataSeries.Count - 1)) / DataSeries.Count;
            offsetProvider = dataSeriesIndex => (dataSeriesIndex - DataSeries.Count / 2M + 0.5M) * dataSeriesWidth + (dataSeriesIndex - (DataSeries.Count - 1) / 2M) * gapWidth;
        }

        return DataSeries.Select((dataSeries, index) => new CanvasDataSeries(
            dataSeries.GetColor(),
            dataSeries.CssClass,
            index,
            dataSeries.GetDataPoints().Select(value => new CanvasDataPoint(
                Chart.MapDataIndexToCanvas(value.Index) + offsetProvider(index),
                Chart.MapDataPointToCanvas(dataPointTransformer(value.DataPoint, value.Index)),
                Chart.MapDataValueToPlotArea(value.DataPoint),
                width,
                value.Index,
                (value.DataPoint / Chart.PlotArea.Multiplier)
            )).ToList())
        ).ToList();
    }

}
