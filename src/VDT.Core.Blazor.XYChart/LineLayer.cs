using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Layer in an <see cref="XYChart"/> in which the data is displayed as a series of points at positions corresponding to the data values, connected by lines
/// </summary>
public class LineLayer : LayerBase {
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
    [Obsolete($"Default data line visibility is now determined by {nameof(DefaultDataLineMode)}")]
    public static bool DefaultShowDataLines { get; set; } = true;

    /// <summary>
    /// Gets or sets the default value for the visibility and type of lines connecting the positions of data points
    /// </summary>
    public static DataLineMode DefaultDataLineMode { get; set; } = DataLineMode.Straight;

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
    [Obsolete($"Data line visibility is now determined by {nameof(DataLineMode)}")]
    [Parameter] public bool ShowDataLines { get; set; } = DefaultShowDataLines;

    /// <summary>
    /// Gets or sets the visibility and type of lines connecting the positions of data points
    /// </summary>
    [Parameter] public DataLineMode DataLineMode { get; set; } = DefaultDataLineMode;

    /// <inheritdoc/>
    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(IsStacked)
        || parameters.HasParameterChanged(ShowDataLabels)
        || parameters.HasParameterChanged(ShowDataMarkers)
        || parameters.HasParameterChanged(DataMarkerSize)
        || parameters.HasParameterChanged(DataMarkerType)
        || parameters.HasParameterChanged(DataLineMode);

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

                if (DataLineMode != DataLineMode.Hidden) {
                    yield return DataLineMode switch {
                        DataLineMode.Straight => GetStraightDataLine(layerIndex, canvasDataSeries),
                        _ => throw new NotImplementedException($"No implementation found for {nameof(DataLineMode)} '{DataLineMode}'.")
                    };
                }
            }
        }
    }

    private ShapeBase GetStraightDataLine(int layerIndex, CanvasDataSeries canvasDataSeries) {
        var commands = new List<string>();

        for (var i = 0; i < canvasDataSeries.DataPoints.Count; i++) {
            if (i == 0 || canvasDataSeries.DataPoints[i - 1].Index < canvasDataSeries.DataPoints[i].Index - 1) {
                commands.Add(PathCommandFactory.MoveTo(canvasDataSeries.DataPoints[i].X, canvasDataSeries.DataPoints[i].Y));
            }
            else {
                commands.Add(PathCommandFactory.LineTo(canvasDataSeries.DataPoints[i].X, canvasDataSeries.DataPoints[i].Y));
            }
        }

        return new LineDataShape(
            commands,
            canvasDataSeries.Color,
            canvasDataSeries.CssClass,
            layerIndex,
            canvasDataSeries.Index
        );
    }
}