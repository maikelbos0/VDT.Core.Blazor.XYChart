using System.Collections.Generic;
using System.Linq;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class XYChartBuilder {
    private static readonly List<string> defaultLabels = new() { "Foo", "Bar", "Baz", "Qux", "Quux" };

    public XYChart Chart { get; }

    public XYChartBuilder() {
        Chart = new();
        Chart.Canvas = new() {
            Chart = Chart,
            Width = CanvasWidth,
            Height = CanvasHeight,
            Padding = CanvasPadding,
            XAxisLabelHeight = CanvasXAxisLabelHeight,
            XAxisLabelClearance = CanvasXAxisLabelClearance,
            YAxisLabelWidth = CanvasYAxisLabelWidth,
            YAxisLabelClearance = CanvasYAxisLabelClearance
        };
        Chart.PlotArea = new() {
            Chart = Chart,
            Min = PlotAreaMin,
            Max = PlotAreaMax,
            GridLineInterval = PlotAreaGridLineInterval
        };
    }

    public XYChartBuilder WithLabelCount(int labelCount) {
        Chart.Labels = defaultLabels.Take(labelCount).ToList();
        return this;
    }

    public XYChartBuilder WithDataPointSpacingMode(DataPointSpacingMode dataPointSpacingMode) {
        Chart.DataPointSpacingMode = dataPointSpacingMode;
        return this;
    }

    public XYChartBuilder WithLayer<TLayer>() where TLayer : LayerBase, new()
        => WithLayer(new TLayer());

    public XYChartBuilder WithLayer(LayerBase layer) {
        Chart.Layers.Add(layer);
        layer.Chart = Chart;
        return this;
    }

    public XYChartBuilder WithDataSeries(params decimal?[] dataPoints) {
        var dataSeries = new DataSeries() {
            DataPoints = dataPoints.ToList(),
            Chart = Chart,
            Layer = Chart.Layers.Last()
        };
        Chart.Layers.Last().DataSeries.Add(dataSeries);
        return this;
    }
}
