﻿using BlazorPlayground.Chart.Shapes;
using Microsoft.AspNetCore.Components;

namespace BlazorPlayground.Chart;

public class LineLayer : LayerBase {
    public static bool DefaultShowDataMarkers { get; set; } = true;
    public static decimal DefaultDataMarkerSize { get; set; } = 10M;
    public static DataMarkerDelegate DefaultDataMarkerType { get; set; } = DefaultDataMarkerTypes.Round;
    public static bool DefaultShowDataLines { get; set; } = true;
    public static decimal DefaultDataLineWidth { get; set; } = 2M;
    public static LineGapMode DefaultLineGapMode { get; set; } = LineGapMode.Skip;

    [Parameter] public bool ShowDataMarkers { get; set; } = DefaultShowDataMarkers;
    [Parameter] public decimal DataMarkerSize { get; set; } = DefaultDataMarkerSize;
    [Parameter] public DataMarkerDelegate DataMarkerType { get; set; } = DefaultDataMarkerType;
    [Parameter] public bool ShowDataLines { get; set; } = DefaultShowDataLines;
    [Parameter] public decimal DataLineWidth { get; set; } = DefaultDataLineWidth;
    [Parameter] public LineGapMode LineGapMode { get; set; } = DefaultLineGapMode;
    public override StackMode StackMode => StackMode.Single;

    // TODO fluent lines?
    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var dataPointsByDataSeries = GetCanvasDataPoints().ToLookup(dataSeriesPoint => dataSeriesPoint.DataSeriesIndex);

        for (var dataSeriesIndex = 0; dataSeriesIndex < DataSeries.Count; dataSeriesIndex++) {
            var dataPoints = dataPointsByDataSeries[dataSeriesIndex].OrderBy(dataPoint => dataPoint.Index).ToList();

            if (ShowDataMarkers) {
                foreach (var dataPoint in dataPoints) {
                    yield return DataMarkerType(
                        dataPoint.X,
                        dataPoint.Y,
                        DataMarkerSize,
                        DataSeries[dataSeriesIndex].GetColor(),
                        dataSeriesIndex,
                        dataPoint.Index
                    );
                }
            }

            if (ShowDataLines) {
                var commands = dataPoints.Select((dataPoint, index) => {
                    if (index == 0) {
                        return PathCommandFactory.MoveTo(dataPoint.X, dataPoint.Y);
                    }
                    else if (dataPoints[index - 1].Index < dataPoint.Index - 1 && LineGapMode == LineGapMode.Skip) {
                        return PathCommandFactory.MoveTo(dataPoint.X, dataPoint.Y);
                    }
                    else {
                        return PathCommandFactory.LineTo(dataPoint.X, dataPoint.Y);
                    }
                });

                yield return new DataLineShape(
                    commands,
                    DataLineWidth,
                    DataSeries[dataSeriesIndex].GetColor(),
                    dataSeriesIndex
                );
            }
        }
    }
}