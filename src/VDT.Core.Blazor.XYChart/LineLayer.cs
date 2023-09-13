using Microsoft.AspNetCore.Components;
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

    public override StackMode StackMode => StackMode.Single;
    public override DataPointSpacingMode DefaultDataPointSpacingMode => DataPointSpacingMode.Center;
    public override bool NullAsZero => IsStacked;

    [Parameter] public bool ShowDataMarkers { get; set; } = DefaultShowDataMarkers;
    [Parameter] public decimal DataMarkerSize { get; set; } = DefaultDataMarkerSize;
    [Parameter] public DataMarkerDelegate DataMarkerType { get; set; } = DefaultDataMarkerType;
    [Parameter] public bool ShowDataLines { get; set; } = DefaultShowDataLines;
    [Parameter] public LineGapMode LineGapMode { get; set; } = DefaultLineGapMode;

    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(IsStacked)
        || parameters.HasParameterChanged(ShowDataMarkers)
        || parameters.HasParameterChanged(DataMarkerSize)
        || parameters.HasParameterChanged(DataMarkerType)
        || parameters.HasParameterChanged(ShowDataLines)
        || parameters.HasParameterChanged(LineGapMode);

    // TODO fluent lines?
    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        foreach (var canvasDataSeries in GetCanvasDataSeries()) {
            if (canvasDataSeries.DataPoints.Any()) {
                if (ShowDataMarkers) {
                    foreach (var dataPoint in canvasDataSeries.DataPoints) {
                        yield return DataMarkerType(
                            dataPoint.X,
                            dataPoint.Y,
                            DataMarkerSize,
                            canvasDataSeries.Color,
                            canvasDataSeries.CssClass,
                            canvasDataSeries.Index,
                            dataPoint.Index
                        );
                    }
                }

                if (ShowDataLines) {
                    var commands = new List<string>();

                    for (var i = 0; i < canvasDataSeries.DataPoints.Count; i++) {
                        if (i == 0) {
                            commands.Add(PathCommandFactory.MoveTo(canvasDataSeries.DataPoints[i].X, canvasDataSeries.DataPoints[i].Y));
                        }
                        else if (canvasDataSeries.DataPoints[i - 1].Index < canvasDataSeries.DataPoints[i].Index - 1 && LineGapMode == LineGapMode.Skip) {
                            commands.Add(PathCommandFactory.MoveTo(canvasDataSeries.DataPoints[i].X, canvasDataSeries.DataPoints[i].Y));
                        }
                        else {
                            commands.Add(PathCommandFactory.LineTo(canvasDataSeries.DataPoints[i].X, canvasDataSeries.DataPoints[i].Y));
                        }
                    }

                    yield return new LineDataShape(
                        commands,
                        canvasDataSeries.Color,
                        canvasDataSeries.CssClass,
                        canvasDataSeries.Index
                    );
                }
            }
        }
    }
}