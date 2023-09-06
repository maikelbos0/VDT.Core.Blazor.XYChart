﻿using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

public class LineLayer : LayerBase {
    public static bool DefaultShowDataMarkers { get; set; } = true;
    public static decimal DefaultDataMarkerSize { get; set; } = 10M;
    public static DataMarkerDelegate DefaultDataMarkerType { get; set; } = DefaultDataMarkerTypes.Round;
    public static bool DefaultShowDataLines { get; set; } = true;
    public static LineGapMode DefaultLineGapMode { get; set; } = LineGapMode.Skip;

    [Parameter] public bool ShowDataMarkers { get; set; } = DefaultShowDataMarkers;
    [Parameter] public decimal DataMarkerSize { get; set; } = DefaultDataMarkerSize;
    [Parameter] public DataMarkerDelegate DataMarkerType { get; set; } = DefaultDataMarkerType;
    [Parameter] public bool ShowDataLines { get; set; } = DefaultShowDataLines;
    [Parameter] public LineGapMode LineGapMode { get; set; } = DefaultLineGapMode;
    public override StackMode StackMode => StackMode.Single;
    public override DataPointSpacingMode DefaultDataPointSpacingMode => DataPointSpacingMode.Center;

    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(IsStacked)
        || parameters.HasParameterChanged(ShowDataMarkers)
        || parameters.HasParameterChanged(DataMarkerSize)
        || parameters.HasParameterChanged(DataMarkerType) 
        || parameters.HasParameterChanged(ShowDataLines)
        || parameters.HasParameterChanged(LineGapMode);

    // TODO fluent lines?
    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var dataPointsByDataSeries = GetCanvasDataPoints().ToLookup(dataSeriesPoint => dataSeriesPoint.DataSeriesIndex);

        for (var dataSeriesIndex = 0; dataSeriesIndex < DataSeries.Count; dataSeriesIndex++) {
            var dataPoints = dataPointsByDataSeries[dataSeriesIndex].OrderBy(dataPoint => dataPoint.Index).ToList();

            if (dataPoints.Any()) {
                if (ShowDataMarkers) {
                    foreach (var dataPoint in dataPoints) {
                        yield return DataMarkerType(
                            dataPoint.X,
                            dataPoint.Y,
                            DataMarkerSize,
                            DataSeries[dataSeriesIndex].GetColor(),
                            DataSeries[dataSeriesIndex].CssClass,
                            dataSeriesIndex,
                            dataPoint.Index
                        );
                    }
                }

                if (ShowDataLines) {
                    var commands = new List<string>();

                    for (var i = 0; i < dataPoints.Count; i++) {
                        if (i == 0) {
                            commands.Add(PathCommandFactory.MoveTo(dataPoints[i].X, dataPoints[i].Y));
                        }
                        else if (dataPoints[i - 1].Index < dataPoints[i].Index - 1 && LineGapMode == LineGapMode.Skip) {
                            commands.Add(PathCommandFactory.MoveTo(dataPoints[i].X, dataPoints[i].Y));
                        }
                        else {
                            commands.Add(PathCommandFactory.LineTo(dataPoints[i].X, dataPoints[i].Y));
                        }
                    }

                    yield return new DataLineShape(
                        commands,
                        DataSeries[dataSeriesIndex].GetColor(),
                        DataSeries[dataSeriesIndex].CssClass,
                        dataSeriesIndex
                    );
                }
            }
        }
    }
}