using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Layer in an <see cref="XYChart"/> in which the data is displayed as vertical bars which have a height corresponding to the data values
/// </summary>
public class BarLayer : LayerBase {
    /// <summary>
    /// Gets or sets the default value for the amount of white space on either side of the group of bars for an index, expressed as a percentage of the total
    /// amount of space available for this index
    /// </summary>
    public static decimal DefaultClearancePercentage { get; set; } = 10M;

    /// <summary>
    /// Gets or sets the default value for the amount of white space between each bar for an index, expressed as a percentage of the total amount of space 
    /// available for this index; this value is ignored when the bar layer is stacked
    /// </summary>
    public static decimal DefaultGapPercentage { get; set; } = 5M;

    /// <inheritdoc/>
    public override StackMode StackMode => StackMode.Split;

    /// <inheritdoc/>
    public override DataPointSpacingMode DefaultDataPointSpacingMode => DataPointSpacingMode.Center;

    /// <inheritdoc/>
    public override bool NullAsZero => false;

    /// <summary>
    /// Gets or sets the amount of white space on either side of the group of bars for an index, expressed as a percentage of the total amount of space 
    /// available for this index
    /// </summary>
    [Parameter] public decimal ClearancePercentage { get; set; } = DefaultClearancePercentage;

    /// <summary>
    /// Gets or sets the amount of white space between each bar for an index, expressed as a percentage of the total amount of space available for this index;
    /// this value is ignored when the bar layer is stacked
    /// </summary>
    [Parameter] public decimal GapPercentage { get; set; } = DefaultGapPercentage;

    /// <inheritdoc/>
    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(IsStacked)
        || parameters.HasParameterChanged(ShowDataLabels)
        || parameters.HasParameterChanged(ClearancePercentage)
        || parameters.HasParameterChanged(GapPercentage);

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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
