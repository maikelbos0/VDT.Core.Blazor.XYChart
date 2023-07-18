﻿using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class XYChart {
        public Canvas Canvas = new();
    public PlotArea PlotArea { get; set; } = new();
    public List<string> Labels { get; set; } = new();
    public List<DataSeriesLayer> DataSeriesLayers { get; set; } = new();
    public AutoScaleSettings AutoScaleSettings { get; set; } = new();
    public decimal DataPointWidth => ((decimal)Canvas.PlotAreaWidth) / Labels.Count;

    public IEnumerable<ShapeBase> GetShapes() {
        PlotArea.AutoScale(AutoScaleSettings, DataSeriesLayers.SelectMany(layer => layer.DataSeries.SelectMany(dataSeries => dataSeries.Where(dataPoint => dataPoint != null).Select(dataPoint => dataPoint!.Value))));

        foreach (var shape in GetGridLineShapes()) {
            yield return shape;
        }

        foreach (var shape in GetDataSeriesShapes()) {
            yield return shape;
        }

        yield return Canvas.GetPlotAreaShape();

        foreach (var shape in GetYAxisLabelShapes()) {
            yield return shape;
        }

        if (GetYAxisMultiplierShape() is ShapeBase multiplierShape) {
            yield return multiplierShape;
        }

        foreach (var shape in GetXAxisLabelShapes()) {
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
        => Labels.Select((label, index) => new XAxisLabelShape(MapDataIndexToCanvas(index), Canvas.PlotAreaY + Canvas.PlotAreaHeight + Canvas.XAxisLabelClearance, label, index));

    public IEnumerable<ShapeBase> GetDataSeriesShapes()
        => DataSeriesLayers.SelectMany(layer => layer.GetDataSeriesShapes());

    public decimal MapDataPointToCanvas(decimal dataPoint) => Canvas.PlotAreaY + (PlotArea.Max - dataPoint) / (PlotArea.Max - PlotArea.Min) * Canvas.PlotAreaHeight;

    public decimal MapDataIndexToCanvas(int index) => Canvas.PlotAreaX + (index + 0.5M) * DataPointWidth;

    public BarDataSeriesLayer AddBarLayer() {
        var layer = new BarDataSeriesLayer(this);

        DataSeriesLayers.Add(layer);

        return layer;
    }

    public void AddDataPoint(string label) {
        Labels.Add(label);

        foreach (var dataSeries in DataSeriesLayers.SelectMany(layer => layer.DataSeries)) {
            dataSeries.AddRange(Enumerable.Range(0, Labels.Count - dataSeries.Count).Select<int, decimal?>(i => null));
        }
    }
}
