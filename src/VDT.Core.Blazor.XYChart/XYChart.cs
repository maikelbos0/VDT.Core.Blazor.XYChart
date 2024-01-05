using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Component to render charts with a category X-axis and a value Y-axis
/// </summary>
public class XYChart : ComponentBase {
    // TODO rename
    // TODO only create one svg element per thing
    // Move to sizeprovider
    internal const string ModuleLocation = "./_content/VDT.Core.Blazor.XYChart/xychart.bfc42c57b9.js";

    /// <summary>
    /// Gets or sets the default value for the the way data points are spaced out over the plot area
    /// </summary>
    public static DataPointSpacingMode DefaultDataPointSpacingMode { get; set; } = DataPointSpacingMode.Auto;

    [Inject] internal IJSRuntime JSRuntime { get; set; } = null!;

    /// <summary>
    /// Gets or sets the content containing chart components
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets the category labels of of the chart; this list determines the amount of data point values shown
    /// </summary>
    [Parameter] public IList<string> Labels { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the the way data points are spaced out over the plot area
    /// </summary>
    [Parameter] public DataPointSpacingMode DataPointSpacingMode { get; set; } = DefaultDataPointSpacingMode;

    internal Canvas Canvas { get; set; }
    internal Legend Legend { get; set; }
    internal PlotArea PlotArea { get; set; }
    internal List<LayerBase> Layers { get; set; } = new();
    internal Action? StateHasChangedHandler { get; init; }
    internal Func<Task<ISizeProvider>>? SizeProviderProvider { get; init; }

    /// <summary>
    /// Create an XY chart
    /// </summary>
    public XYChart() {
        Canvas = new Canvas() { Chart = this };
        Legend = new Legend() { Chart = this };
        PlotArea = new PlotArea() { Chart = this };
    }

    /// <inheritdoc/>
    public override async Task SetParametersAsync(ParameterView parameters) {
        var parametersHaveChanged = HaveParametersChanged(parameters);

        await base.SetParametersAsync(parameters);

        if (parametersHaveChanged) {
            await HandleStateChange();
        }
    }

    /// <summary>
    /// Determines whether or not any parameters have changed and the chart has to be re-rendered
    /// </summary>
    /// <param name="parameters">New parameters</param>
    /// <returns><see langword="true"/> of any parameter has changed; otherwise <see langword="false"/></returns>
    public bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(Labels)
        || parameters.HasParameterChanged(DataPointSpacingMode);

    /// <inheritdoc/>
    protected override void BuildRenderTree(RenderTreeBuilder builder) {
        builder.OpenElement(1, "svg");
        builder.AddAttribute(2, "xmlns", "http://www.w3.org/2000/svg");
        builder.AddAttribute(3, "class", "chart-main");
        builder.AddAttribute(4, "viewbox", $"0 0 {Canvas.Width} {Canvas.Height}");
        builder.AddAttribute(5, "width", Canvas.Width);
        builder.AddAttribute(6, "height", Canvas.Height);

        builder.OpenRegion(7);
        foreach (var shape in GetShapes()) {
            builder.OpenElement(1, shape.ElementName);
            builder.SetKey(shape.Key);
            builder.AddAttribute(2, "class", shape.CssClass);
            builder.AddMultipleAttributes(3, shape.GetAttributes());
            builder.AddContent(4, shape.GetContent());
            builder.CloseElement();
        }
        builder.CloseRegion();

        builder.OpenComponent<CascadingValue<XYChart>>(8);
        builder.AddAttribute(9, "Value", this);
        builder.AddAttribute(10, "ChildContent", ChildContent);
        builder.CloseComponent();

        builder.CloseElement();
    }

    internal async Task SetCanvas(Canvas canvas) {
        Canvas = canvas;
        await HandleStateChange();
    }

    internal async Task ResetCanvas() {
        Canvas = new();
        await HandleStateChange();
    }

    internal async Task SetLegend(Legend legend) {
        Legend = legend;
        await HandleStateChange();
    }

    internal async Task ResetLegend() {
        Legend = new();
        await HandleStateChange();
    }

    internal async Task SetPlotArea(PlotArea plotArea) {
        PlotArea = plotArea;
        await HandleStateChange();
    }

    internal async Task ResetPlotArea() {
        PlotArea = new();
        await HandleStateChange();
    }

    internal async Task AddLayer(LayerBase layer) {
        // TODO do we need this check?
        if (!Layers.Contains(layer)) {
            Layers.Add(layer);
        }

        await HandleStateChange();
    }

    internal async Task RemoveLayer(LayerBase layer) {
        Layers.Remove(layer);
        await HandleStateChange();
    }

    // TODO check if debounce is needed
    internal async Task HandleStateChange() {
        if (StateHasChangedHandler != null) {
            StateHasChangedHandler();
        }
        else {
            PlotArea.AutoScale(Layers.SelectMany(layer => layer.GetScaleDataPoints()));
            await Canvas.AutoSize();
            StateHasChanged();
        }
    }

    internal Task<ISizeProvider> GetSizeProvider() => SizeProviderProvider?.Invoke() ?? SizeProvider.Create(JSRuntime);

    /// <summary>
    /// Gets the SVG shapes needed to display the chart
    /// </summary>
    /// <returns>The SVG shapes</returns>
    public IEnumerable<ShapeBase> GetShapes() {
        foreach (var shape in GetGridLineShapes()) {
            yield return shape;
        }

        foreach (var shape in GetDataSeriesShapes()) {
            yield return shape;
        }

        yield return Canvas.GetPlotAreaShape();

        foreach (var shape in GetDataLabelShapes()) {
            yield return shape;
        }

        foreach (var shape in GetYAxisLabelShapes()) {
            yield return shape;
        }

        if (GetYAxisMultiplierShape() is ShapeBase multiplierShape) {
            yield return multiplierShape;
        }

        foreach (var shape in GetXAxisLabelShapes()) {
            yield return shape;
        }

        foreach (var shape in GetLegendShapes()) {
            yield return shape;
        }
    }

    /// <summary>
    /// Gets the SVG shapes for all grid lines in the chart plot area
    /// </summary>
    /// <returns>The SVG grid line shapes</returns>
    public IEnumerable<GridLineShape> GetGridLineShapes()
        => PlotArea.GetGridLineDataPoints().Select((dataPoint, index) => new GridLineShape(Canvas.PlotAreaX, MapDataPointToCanvas(dataPoint), Canvas.PlotAreaWidth, dataPoint, index));

    /// <summary>
    /// Gets the SVG shapes for the chart Y-axis labels
    /// </summary>
    /// <returns>The SVG Y-axis label shapes</returns>
    public IEnumerable<YAxisLabelShape> GetYAxisLabelShapes()
        => PlotArea.GetGridLineDataPoints().Select((dataPoint, index) => new YAxisLabelShape(Canvas.PlotAreaX, MapDataPointToCanvas(dataPoint), (dataPoint / PlotArea.Multiplier).ToString(Canvas.YAxisLabelFormat), index));

    /// <summary>
    /// Gets the SVG shape for the chart Y-axis multiplier, if the multiplier value is not exactly 1
    /// </summary>
    /// <returns>The SVG Y-axis multiplier shape or <see langword="null"/></returns>
    public YAxisMultiplierShape? GetYAxisMultiplierShape()
        => PlotArea.Multiplier == 1M ? null : new YAxisMultiplierShape(Canvas.Padding, Canvas.PlotAreaY + Canvas.PlotAreaHeight / 2M, PlotArea.Multiplier.ToString(Canvas.YAxisMultiplierFormat));

    /// <summary>
    /// Gets the SVG shapes for the chart X-axis labels
    /// </summary>
    /// <returns>The SVG X-axis label shapes</returns>
    public IEnumerable<XAxisLabelShape> GetXAxisLabelShapes()
        => Labels.Select((label, index) => new XAxisLabelShape(MapDataIndexToCanvas(index), Canvas.PlotAreaY + Canvas.PlotAreaHeight, label, index));

    /// <summary>
    /// Gets the SVG shapes for the chart data series
    /// </summary>
    /// <returns>The SVG data series shapes</returns>
    public IEnumerable<ShapeBase> GetDataSeriesShapes()
        => Layers.SelectMany(layer => layer.GetDataSeriesShapes());

    /// <summary>
    /// Gets the SVG shapes for the chart data labels
    /// </summary>
    /// <returns>The SVG data label shapes</returns>
    public IEnumerable<DataLabelShape> GetDataLabelShapes()
        => Layers.SelectMany(layer => layer.GetDataLabelShapes());

    /// <summary>
    /// Gets the SVG shapes for the chart legend items
    /// </summary>
    /// <returns>The SVG legend key and text shapes</returns>
    /// <exception cref="NotImplementedException">Thrown when the legend uses an unknown <see cref="LegendAlignment"/></exception>
    public IEnumerable<ShapeBase> GetLegendShapes() {
        if (!Legend.IsEnabled) {
            yield break;
        }

        var itemsPerRow = Canvas.PlotAreaWidth / Legend.ItemWidth;
        var rows = Layers
            .SelectMany(layer => layer.GetLegendItems())
            .Select((item, index) => new { Item = item, Index = index })
            .GroupBy(value => value.Index / itemsPerRow, value => value.Item);
        Func<int, int, decimal> offsetProvider = Legend.Alignment switch {
            LegendAlignment.Left => (index, _) => Canvas.PlotAreaX + index * Legend.ItemWidth,
            LegendAlignment.Center => (index, count) => Canvas.PlotAreaX + Canvas.PlotAreaWidth / 2 - (count / 2M - index) * Legend.ItemWidth,
            LegendAlignment.Right => (index, count) => Canvas.Width - Canvas.Padding - (count - index) * Legend.ItemWidth,
            _ => throw new NotImplementedException($"No implementation found for {nameof(LegendAlignment)} '{Legend.Alignment}'.")
        };

        foreach (var row in rows) {
            var rowIndex = row.Key;
            var rowItems = row.ToList();

            for (var index = 0; index < rowItems.Count; index++) {
                var item = rowItems[index];

                yield return new LegendKeyShape(
                    offsetProvider(index, rowItems.Count) + (Legend.ItemHeight - Legend.KeySize) / 2M,
                    Canvas.LegendY + (rowIndex + 0.5M) * Legend.ItemHeight - Legend.KeySize / 2M,
                    Legend.KeySize,
                    item.Color,
                    item.CssClass,
                    item.LayerIndex,
                    item.DataSeriesIndex
                );

                yield return new LegendTextShape(
                    offsetProvider(index, rowItems.Count) + Legend.ItemHeight,
                    Canvas.LegendY + (rowIndex + 0.5M) * Legend.ItemHeight,
                    item.Text,
                    item.CssClass,
                    item.LayerIndex,
                    item.DataSeriesIndex
                );
            }
        }
    }

    /// <summary>
    /// Maps a data point value to a Y-coordinate on the canvas
    /// </summary>
    /// <param name="dataPoint">Data point value</param>
    /// <returns>The Y-coordinate</returns>
    public decimal MapDataPointToCanvas(decimal dataPoint) => Canvas.PlotAreaY + MapDataValueToPlotArea(PlotArea.ActualMax - dataPoint);

    /// <summary>
    /// Maps a data point value to a plot area value in pixels
    /// </summary>
    /// <param name="dataPoint">Data point value</param>
    /// <returns>The value in pixels</returns>
    public decimal MapDataValueToPlotArea(decimal dataPoint) => dataPoint / (PlotArea.ActualMax - PlotArea.ActualMin) * Canvas.PlotAreaHeight;

    /// <summary>
    /// Gets the actual <see cref="DataPointSpacingMode"/> for this chart, determining it based on the layers if needed
    /// </summary>
    /// <returns>The actual <see cref="DataPointSpacingMode"/></returns>
    public DataPointSpacingMode GetDataPointSpacingMode() => DataPointSpacingMode switch {
        DataPointSpacingMode.Auto => Layers.Select(layer => layer.DefaultDataPointSpacingMode).DefaultIfEmpty(DataPointSpacingMode.EdgeToEdge).Max(),
        _ => DataPointSpacingMode
    };

    /// <summary>
    /// Gets the width in pixels that's reserved for each data point
    /// </summary>
    /// <returns>The width of the data point in pixels</returns>
    /// <exception cref="NotImplementedException">Thrown when the chart uses an unknown or unusable <see cref="DataPointSpacingMode"/></exception>
    public decimal GetDataPointWidth() => GetDataPointSpacingMode() switch {
        DataPointSpacingMode.EdgeToEdge => (decimal)Canvas.PlotAreaWidth / Math.Max(1, Labels.Count - 1),
        DataPointSpacingMode.Center => (decimal)Canvas.PlotAreaWidth / Math.Max(1, Labels.Count),
        _ => throw new NotImplementedException($"No implementation found for {nameof(DataPointSpacingMode)} '{DataPointSpacingMode}'.")
    };

    /// <summary>
    /// Maps a data point index to an X-coordinate on the canvas
    /// </summary>
    /// <param name="index">Data point index</param>
    /// <returns>The X-coordinate</returns>
    /// <exception cref="NotImplementedException">Thrown when the chart uses an unknown or unusable <see cref="DataPointSpacingMode"/></exception>
    public decimal MapDataIndexToCanvas(int index) => GetDataPointSpacingMode() switch {
        DataPointSpacingMode.EdgeToEdge => Canvas.PlotAreaX + index * GetDataPointWidth(),
        DataPointSpacingMode.Center => Canvas.PlotAreaX + (index + 0.5M) * GetDataPointWidth(),
        _ => throw new NotImplementedException($"No implementation found for {nameof(DataPointSpacingMode)} '{DataPointSpacingMode}'.")
    };
}
