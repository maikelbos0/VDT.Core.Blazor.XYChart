using System.Collections.Generic;
using System.Linq;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class XYChartBuilder {
    private static readonly List<string> defaultLabels = new() { "Foo", "Bar", "Baz", "Qux", "Quux" };
    private readonly XYChart chart;

    public XYChartBuilder() {
        chart = new();
        chart.Canvas = new() {
            Chart = chart,
            Width = CanvasWidth,
            Height = CanvasHeight,
            Padding = CanvasPadding,
            XAxisLabelHeight = CanvasXAxisLabelHeight,
            XAxisLabelClearance = CanvasXAxisLabelClearance,
            YAxisLabelWidth = CanvasYAxisLabelWidth,
            YAxisLabelClearance = CanvasYAxisLabelClearance
        };
        chart.PlotArea = new() {
            Chart = chart,
            Min = PlotAreaMin,
            Max = PlotAreaMax,
            GridLineInterval = PlotAreaGridLineInterval
        };
    }

    public XYChart GetChart() => chart;

    public XYChartBuilder WithLabelCount(int labelCount) {
        chart.Labels = defaultLabels.Take(labelCount).ToList();
        return this;
    }

    public XYChartBuilder WithDataPointSpacingMode(DataPointSpacingMode dataPointSpacingMode) {
        chart.DataPointSpacingMode = dataPointSpacingMode;
        return this;
    }

    public XYChartBuilder WithLayer<TLayer>() where TLayer : LayerBase, new()
        => WithLayer(new TLayer());

    public XYChartBuilder WithLayer(LayerBase layer) {
        chart.Layers.Add(layer);
        layer.Chart = chart;
        return this;
    }

    public XYChartBuilder WithDataSeries(params decimal?[] dataPoints) {
        var dataSeries = new DataSeries() {
            DataPoints = dataPoints.ToList(),
            Chart = chart,
            Layer = chart.Layers.Last()
        };
        chart.Layers.Last().DataSeries.Add(dataSeries);
        return this;
    }
}
