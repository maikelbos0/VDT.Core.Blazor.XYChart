using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Layer in an <see cref="XYChart"/> in which the data is displayed as a series of points at positions corresponding to the data values, connected by fluent lines
/// </summary>
public class SmoothLineLayer : LayerBase {
    /// <summary>
    /// Gets or sets the default value for visibility of the markers at the positions of the data points
    /// </summary>
    public static bool DefaultShowDataMarkers { get; set; } = true;

    /// <summary>
    /// Gets or sets the default value for the size of the data markers
    /// </summary>
    public static decimal DefaultDataMarkerSize { get; set; } = 10M;

    /// <summary>
    /// Gets or sets the default value for the type of data marker to use; this library offers options in the <see cref="DefaultDataMarkerTypes"/> static
    /// class, but implementing your own is as simple as creating your own implementation of <see cref="ShapeBase"/> and setting this property to a
    /// <see cref="DataMarkerDelegate"/> that returns it
    /// </summary>
    public static DataMarkerDelegate DefaultDataMarkerType { get; set; } = DefaultDataMarkerTypes.Round;

    /// <summary>
    /// Gets or sets the default value for visibility of the lines connecting the positions of the data points
    /// </summary>
    public static bool DefaultShowDataLines { get; set; } = true;

    /// <inheritdoc/>
    public override StackMode StackMode => StackMode.Single;

    /// <inheritdoc/>
    public override DataPointSpacingMode DefaultDataPointSpacingMode => DataPointSpacingMode.Center;

    /// <inheritdoc/>
    public override bool NullAsZero => IsStacked;

    /// <summary>
    /// Gets or sets visibility of the markers at the positions of the data points
    /// </summary>
    [Parameter] public bool ShowDataMarkers { get; set; } = DefaultShowDataMarkers;

    /// <summary>
    /// Gets or sets the size of the data markers
    /// </summary>
    [Parameter] public decimal DataMarkerSize { get; set; } = DefaultDataMarkerSize;

    /// <summary>
    /// Gets or sets the type of data marker to use; this library offers options in the <see cref="DefaultDataMarkerTypes"/> static class, but implementing
    /// your own is as simple as creating your own implementation of <see cref="ShapeBase"/> and setting this property to a <see cref="DataMarkerDelegate"/>
    /// that returns it
    /// </summary>
    [Parameter] public DataMarkerDelegate DataMarkerType { get; set; } = DefaultDataMarkerType;

    /// <summary>
    /// Gets or sets visibility of the lines connecting the positions of the data points
    /// </summary>
    [Parameter] public bool ShowDataLines { get; set; } = DefaultShowDataLines;

    /// <inheritdoc/>
    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(IsStacked)
        || parameters.HasParameterChanged(ShowDataLabels)
        || parameters.HasParameterChanged(ShowDataMarkers)
        || parameters.HasParameterChanged(DataMarkerSize)
        || parameters.HasParameterChanged(DataMarkerType)
        || parameters.HasParameterChanged(ShowDataLines);

    /// <inheritdoc/>
    public override IEnumerable<ShapeBase> GetDataSeriesShapes() {
        var layerIndex = Chart.Layers.IndexOf(this);

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
                            layerIndex,
                            canvasDataSeries.Index,
                            dataPoint.Index
                        );
                    }
                }

                if (ShowDataLines) {
                    var controlPoints = new ControlPoints?[canvasDataSeries.DataPoints.Count];

                    for (var i = 1; i < canvasDataSeries.DataPoints.Count - 1; i++) {
                        if (canvasDataSeries.DataPoints[i - 1].Index == canvasDataSeries.DataPoints[i].Index - 1 && canvasDataSeries.DataPoints[i + 1].Index == canvasDataSeries.DataPoints[i].Index + 1) {
                            controlPoints[i] = GetControlPoints(canvasDataSeries.DataPoints[i - 1], canvasDataSeries.DataPoints[i], canvasDataSeries.DataPoints[i + 1]);
                        }
                    }

                    var commands = new List<string>();

                    for (var i = 0; i < canvasDataSeries.DataPoints.Count; i++) {
                        if (i == 0 || canvasDataSeries.DataPoints[i - 1].Index < canvasDataSeries.DataPoints[i].Index - 1) {
                            commands.Add(PathCommandFactory.MoveTo(canvasDataSeries.DataPoints[i].X, canvasDataSeries.DataPoints[i].Y));
                        }
                        else if (i > 0 && controlPoints[i - 1] != null && controlPoints[i] != null) {
                            commands.Add(PathCommandFactory.CubicBezierTo(
                                controlPoints[i - 1]!.RightX,
                                controlPoints[i - 1]!.RightY,
                                controlPoints[i]!.LeftX,
                                controlPoints[i]!.LeftY,
                                canvasDataSeries.DataPoints[i].X,
                                canvasDataSeries.DataPoints[i].Y
                            ));
                        }
                        else if (i > 0 && controlPoints[i - 1] != null) {
                            commands.Add(PathCommandFactory.QuadraticBezierTo(
                                controlPoints[i - 1]!.RightX,
                                controlPoints[i - 1]!.RightY,
                                canvasDataSeries.DataPoints[i].X,
                                canvasDataSeries.DataPoints[i].Y
                            ));
                        }
                        else if (controlPoints[i] != null) {
                            commands.Add(PathCommandFactory.QuadraticBezierTo(
                                controlPoints[i]!.LeftX,
                                controlPoints[i]!.LeftY,
                                canvasDataSeries.DataPoints[i].X,
                                canvasDataSeries.DataPoints[i].Y
                            ));
                        }
                        else {
                            commands.Add(PathCommandFactory.LineTo(canvasDataSeries.DataPoints[i].X, canvasDataSeries.DataPoints[i].Y));
                        }
                    }

                    yield return new LineDataShape(
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

    /// <summary>
    /// Calculate control points for bezier curves to create smooth lines
    /// </summary>
    /// <param name="left">Previous data point</param>
    /// <param name="dataPoint">Current data point</param>
    /// <param name="right">Next data point</param>
    /// <returns>The control points for this data point</returns>
    public static ControlPoints GetControlPoints(CanvasDataPoint left, CanvasDataPoint dataPoint, CanvasDataPoint right) {
        // TODO setting
        var distance = 0.25M;
        var slope = (right.Y - left.Y) / 2M;

        return new ControlPoints(
            dataPoint.X - distance * (dataPoint.X - left.X),
            dataPoint.Y - slope * distance,
            dataPoint.X + distance * (right.X - dataPoint.X),
            dataPoint.Y + slope * distance
        );
    }
}