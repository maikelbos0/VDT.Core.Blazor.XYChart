﻿using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

public class AreaLayer : LayerBase {
    public override StackMode StackMode => StackMode.Single;
    public override DataPointSpacingMode DefaultDataPointSpacingMode => DataPointSpacingMode.EdgeToEdge;
    public override bool NullAsZero => true;

    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(IsStacked);

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var layerIndex = Chart.Layers.IndexOf(this);

        foreach (var canvasDataSeries in GetCanvasDataSeries()) {
            if (canvasDataSeries.DataPoints.Any()) {
                var commands = new List<string>() {
                    PathCommandFactory.MoveTo(canvasDataSeries.DataPoints[0].X, canvasDataSeries.DataPoints[0].Y)
                };

                foreach (var canvasDataPoint in canvasDataSeries.DataPoints.Skip(1)) {
                    commands.Add(PathCommandFactory.LineTo(canvasDataPoint.X, canvasDataPoint.Y));
                }

                foreach (var canvasDataPoint in canvasDataSeries.DataPoints.Reverse()) {
                    commands.Add(PathCommandFactory.LineTo(canvasDataPoint.X, canvasDataPoint.Y + canvasDataPoint.Height));
                }

                commands.Add(PathCommandFactory.ClosePath);

                yield return new AreaDataShape(
                    commands,
                    canvasDataSeries.Color,
                    canvasDataSeries.CssClass,
                    layerIndex,
                    canvasDataSeries.Index
                );
            }
        }
    }
}
