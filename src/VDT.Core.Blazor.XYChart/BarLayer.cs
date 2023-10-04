﻿using Microsoft.AspNetCore.Components;
using System;
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
        || parameters.HasParameterChanged(ClearancePercentage)
        || parameters.HasParameterChanged(GapPercentage);

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        if (!DataSeries.Any()) {
            yield break;
        }

        var layerIndex = Chart.Layers.IndexOf(this);
        var width = Chart.GetDataPointWidth() / 100M * (100M - ClearancePercentage * 2);
        var offsetProvider = (int dataSeriesIndex) => -width / 2M;
        
        if (!IsStacked) {
            var gapWidth = Chart.GetDataPointWidth() / 100M * GapPercentage;
            var dataSeriesWidth = (width - gapWidth * (DataSeries.Count - 1)) / DataSeries.Count;

            width = (width - gapWidth * (DataSeries.Count - 1)) / DataSeries.Count;
            offsetProvider = dataSeriesIndex => (dataSeriesIndex - DataSeries.Count / 2M) * dataSeriesWidth + (dataSeriesIndex - (DataSeries.Count - 1) / 2M) * gapWidth;
        }

        foreach (var canvasDataSeries in GetCanvasDataSeries()) {
            foreach (var canvasDataPoint in canvasDataSeries.DataPoints) {
                yield return new BarDataShape(
                    canvasDataPoint.X + offsetProvider(canvasDataSeries.Index),
                    canvasDataPoint.Y,
                    width,
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
}
