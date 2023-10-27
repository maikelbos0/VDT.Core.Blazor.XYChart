using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDT.Core.Blazor.XYChart.Shapes;

namespace VDT.Core.Blazor.XYChart;

public class XYChart : ComponentBase {
    public static DataPointSpacingMode DefaultDataPointSpacingMode { get; set; } = DataPointSpacingMode.Auto;

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public IList<string> Labels { get; set; } = new List<string>();
    [Parameter] public DataPointSpacingMode DataPointSpacingMode { get; set; } = DefaultDataPointSpacingMode;
    internal Canvas Canvas { get; set; }
    internal Legend Legend { get; set; }
    internal PlotArea PlotArea { get; set; }
    internal AutoScaleSettings AutoScaleSettings { get; set; }
    internal List<LayerBase> Layers { get; set; } = new();
    internal Action? StateHasChangedHandler { get; init; }

    public XYChart() {
        Canvas = new Canvas() { Chart = this };
        Legend = new Legend() { Chart = this };
        PlotArea = new PlotArea() { Chart = this };
        AutoScaleSettings = new AutoScaleSettings() { Chart = this };
    }

    public override async Task SetParametersAsync(ParameterView parameters) {
        var parametersHaveChanged = HaveParametersChanged(parameters);

        await base.SetParametersAsync(parameters);

        if (parametersHaveChanged) {
            HandleStateChange();
        }
    }

    public bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(Labels)
        || parameters.HasParameterChanged(DataPointSpacingMode);

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

    internal void SetCanvas(Canvas canvas) {
        Canvas = canvas;
        HandleStateChange();
    }

    internal void ResetCanvas() {
        Canvas = new();
        HandleStateChange();
    }

    internal void SetLegend(Legend legend) {
        Legend = legend;
        HandleStateChange();
    }

    internal void ResetLegend() {
        Legend = new();
        HandleStateChange();
    }

    internal void SetPlotArea(PlotArea plotArea) {
        PlotArea = plotArea;
        HandleStateChange();
    }

    internal void ResetPlotArea() {
        PlotArea = new();
        HandleStateChange();
    }

    internal void SetAutoScaleSettings(AutoScaleSettings autoScaleSettings) {
        AutoScaleSettings = autoScaleSettings;
        HandleStateChange();
    }

    internal void ResetAutoScaleSettings() {
        AutoScaleSettings = new();
        HandleStateChange();
    }

    internal void AddLayer(LayerBase layer) {
        if (!Layers.Contains(layer)) {
            Layers.Add(layer);
        }

        HandleStateChange();
    }

    internal void RemoveLayer(LayerBase layer) {
        Layers.Remove(layer);
        HandleStateChange();
    }

    internal void HandleStateChange() => (StateHasChangedHandler ?? StateHasChanged)();

    public IEnumerable<ShapeBase> GetShapes() {
        PlotArea.AutoScale(Layers.SelectMany(layer => layer.GetScaleDataPoints()));

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

    public IEnumerable<GridLineShape> GetGridLineShapes()
        => PlotArea.GetGridLineDataPoints().Select((dataPoint, index) => new GridLineShape(Canvas.PlotAreaX, MapDataPointToCanvas(dataPoint), Canvas.PlotAreaWidth, dataPoint, index));

    public IEnumerable<YAxisLabelShape> GetYAxisLabelShapes()
        => PlotArea.GetGridLineDataPoints().Select((dataPoint, index) => new YAxisLabelShape(Canvas.PlotAreaX - Canvas.YAxisLabelClearance, MapDataPointToCanvas(dataPoint), (dataPoint / PlotArea.Multiplier).ToString(Canvas.YAxisLabelFormat), index));

    public YAxisMultiplierShape? GetYAxisMultiplierShape()
        => PlotArea.Multiplier == 1M ? null : new YAxisMultiplierShape(Canvas.Padding, Canvas.PlotAreaY + Canvas.PlotAreaHeight / 2M, PlotArea.Multiplier.ToString(Canvas.YAxisMultiplierFormat));

    public IEnumerable<XAxisLabelShape> GetXAxisLabelShapes()
        => Labels.Select((label, index) => new XAxisLabelShape(MapDataIndexToCanvas(index), Canvas.PlotAreaY + Canvas.PlotAreaHeight, label, index));

    public IEnumerable<ShapeBase> GetDataSeriesShapes()
        => Layers.SelectMany(layer => layer.GetDataSeriesShapes());

    public IEnumerable<DataLabelShape> GetDataLabelShapes()
        => Layers.SelectMany(layer => layer.GetDataLabelShapes());

    // TODO test
    public IEnumerable<ShapeBase> GetLegendShapes() {
        if (!Legend.IsEnabled) {
            yield break;
        }

        var items = Layers.SelectMany(layer => layer.GetLegendItems()).ToList();
        var itemsPerRow = Canvas.LegendWidth / Legend.ItemWidth;
        var rows = items
            .Select((item, index) => new { Item = item, Index = index })
            .GroupBy(value => (value.Index - 1) / itemsPerRow, value => value.Item);

        foreach (var row in rows) {
            var rowIndex = row.Key;
            var rowItems = row.ToList();

            for (var index = 0; index < rowItems.Count; index++) {
                var item = rowItems[index];
                Console.WriteLine(rowIndex);

                // TODO calculate x, adjust y
                yield return new LegendKeyShape(index * 100, Canvas.LegendY + rowIndex * Legend.ItemHeight, Legend.KeySize, Legend.KeySize, item.Color, item.CssClass, item.LayerIndex, item.DataSeriesIndex);

                // TODO calculate x, adjust y
                yield return new LegendTextShape(index * 100, Canvas.LegendY + rowIndex * Legend.ItemHeight, item.Text, item.CssClass, item.LayerIndex, item.DataSeriesIndex);
            }
        }
    }

    public decimal MapDataPointToCanvas(decimal dataPoint) => Canvas.PlotAreaY + MapDataValueToPlotArea(PlotArea.Max - dataPoint);

    public decimal MapDataValueToPlotArea(decimal dataPoint) => dataPoint / (PlotArea.Max - PlotArea.Min) * Canvas.PlotAreaHeight;

    public DataPointSpacingMode GetDataPointSpacingMode() => DataPointSpacingMode switch {
        DataPointSpacingMode.Auto => Layers.Select(layer => layer.DefaultDataPointSpacingMode).DefaultIfEmpty(DataPointSpacingMode.EdgeToEdge).Max(),
        _ => DataPointSpacingMode
    };

    public decimal GetDataPointWidth() => GetDataPointSpacingMode() switch {
        DataPointSpacingMode.EdgeToEdge => (decimal)Canvas.PlotAreaWidth / Math.Max(1, Labels.Count - 1),
        DataPointSpacingMode.Center => (decimal)Canvas.PlotAreaWidth / Math.Max(1, Labels.Count),
        _ => throw new NotImplementedException($"No implementation found for {nameof(DataPointSpacingMode)} '{DataPointSpacingMode}'.")
    };

    public decimal MapDataIndexToCanvas(int index) => GetDataPointSpacingMode() switch {
        DataPointSpacingMode.EdgeToEdge => Canvas.PlotAreaX + index * GetDataPointWidth(),
        DataPointSpacingMode.Center => Canvas.PlotAreaX + (index + 0.5M) * GetDataPointWidth(),
        _ => throw new NotImplementedException($"No implementation found for {nameof(DataPointSpacingMode)} '{DataPointSpacingMode}'.")
    };
}
