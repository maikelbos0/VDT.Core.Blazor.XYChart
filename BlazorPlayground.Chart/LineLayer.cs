﻿using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class LineLayer : LayerBase {
    // TODO setting for show marker
    // TODO setting for marker type
    // TODO setting for marker size
    // TODO setting for show line
    // TODO setting for what do do for null in line

    public LineLayer(XYChart chart) : base(chart) { }

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        // TODO add actual lines
        if (IsStacked) {
            return GetStackedDataSeriesShapes();
        }
        else {
            return GetUnstackedDataSeriesShapes();
        }
    }

    public IEnumerable<ShapeBase> GetStackedDataSeriesShapes() {
        var negativeOffsets = Enumerable.Repeat(0M, Chart.Labels.Count).ToList();
        var positiveOffsets = Enumerable.Repeat(0M, Chart.Labels.Count).ToList();

        return DataSeries.SelectMany((dataSeries, dataSeriesIndex) => dataSeries
            .Select((dataPoint, index) => (DataPoint: dataPoint, Index: index))
            .Where(value => value.DataPoint != null && value.Index < Chart.Labels.Count)
            .Select(value => {
                var dataPoint = value.DataPoint!.Value;
                var offsets = dataPoint < 0 ? negativeOffsets : positiveOffsets;

                offsets[value.Index] += dataPoint;

                return new RoundDataMarkerShape(
                    Chart.MapDataIndexToCanvas(value.Index),
                    Chart.MapDataPointToCanvas(offsets[value.Index]),
                    10M,
                    dataSeries.Color,
                    dataSeriesIndex,
                    value.Index
                );
            }));
    }

    public IEnumerable<ShapeBase> GetUnstackedDataSeriesShapes() {
        var zeroY = Chart.MapDataPointToCanvas(0M);

        return DataSeries.SelectMany((dataSeries, dataSeriesIndex) => dataSeries
            .Select((dataPoint, index) => (DataPoint: dataPoint, Index: index))
            .Where(value => value.DataPoint != null && value.Index < Chart.Labels.Count)
            .Select(value => {
                return new RoundDataMarkerShape(
                    Chart.MapDataIndexToCanvas(value.Index),
                    Chart.MapDataPointToCanvas(value.DataPoint!.Value),
                    10M,
                    dataSeries.Color,
                    dataSeriesIndex,
                    value.Index
                );
            }));
    }
}
