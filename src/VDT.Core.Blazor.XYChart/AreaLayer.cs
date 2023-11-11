using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Layer in which the data series get displayed as filled shapes connecting the data point values
/// </summary>
public class AreaLayer : LayerBase {
    /// <inheritdoc/>
    public override StackMode StackMode => StackMode.Single;

    /// <inheritdoc/>
    public override DataPointSpacingMode DefaultDataPointSpacingMode => DataPointSpacingMode.EdgeToEdge;

    /// <inheritdoc/>
    public override bool NullAsZero => true;

    /// <inheritdoc/>
    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(IsStacked)
        || parameters.HasParameterChanged(ShowDataLabels);

    /// <inheritdoc/>
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
