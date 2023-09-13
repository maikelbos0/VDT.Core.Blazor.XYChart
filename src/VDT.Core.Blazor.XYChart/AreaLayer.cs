using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

public class AreaLayer : LayerBase {
    public static LineGapMode DefaultLineGapMode { get; set; } = LineGapMode.Skip;

    public override StackMode StackMode => StackMode.Single;
    public override DataPointSpacingMode DefaultDataPointSpacingMode => DataPointSpacingMode.EdgeToEdge;
    public override bool NullAsZero => true;

    [Parameter] public LineGapMode LineGapMode { get; set; } = DefaultLineGapMode;

    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(IsStacked)
        || parameters.HasParameterChanged(LineGapMode);

    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var zeroY = Chart.MapDataPointToCanvas(0M);

        foreach (var canvasDataSeries in GetCanvasDataSeries()) {
            if (canvasDataSeries.DataPoints.Any()) {
                var commands = new List<string>();

                for (var i = 0; i < canvasDataSeries.DataPoints.Count; i++) {
                    if (i == 0) {
                        commands.Add(PathCommandFactory.MoveTo(canvasDataSeries.DataPoints[i].X, zeroY));
                        commands.Add(PathCommandFactory.LineTo(canvasDataSeries.DataPoints[i].X, canvasDataSeries.DataPoints[i].Y));
                    }
                    else if (canvasDataSeries.DataPoints[i - 1].Index < canvasDataSeries.DataPoints[i].Index - 1 && LineGapMode == LineGapMode.Skip) {
                        commands.Add(PathCommandFactory.LineTo(canvasDataSeries.DataPoints[i - 1].X, zeroY));
                        commands.Add(PathCommandFactory.ClosePath);

                        commands.Add(PathCommandFactory.MoveTo(canvasDataSeries.DataPoints[i].X, zeroY));
                        commands.Add(PathCommandFactory.LineTo(canvasDataSeries.DataPoints[i].X, canvasDataSeries.DataPoints[i].Y));
                    }
                    else {
                        commands.Add(PathCommandFactory.LineTo(canvasDataSeries.DataPoints[i].X, canvasDataSeries.DataPoints[i].Y));
                    }
                }

                commands.Add(PathCommandFactory.LineTo(canvasDataSeries.DataPoints[^1].X, zeroY));
                commands.Add(PathCommandFactory.ClosePath);

                yield return new AreaDataShape(
                    commands,
                    canvasDataSeries.Color,
                    canvasDataSeries.CssClass,
                    canvasDataSeries.Index
                );
            }
        }
    }
}
