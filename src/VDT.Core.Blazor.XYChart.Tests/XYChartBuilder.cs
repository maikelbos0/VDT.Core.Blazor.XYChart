using System.Collections.Generic;
using System.Linq;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class XYChartBuilder {
    private static readonly List<string> defaultLabels = new() { "Foo", "Bar", "Baz", "Qux", "Quux" };

    public XYChart Chart { get; }
    public bool StateHasChangedInvoked { get; private set; }

    public XYChartBuilder(int labelCount = LabelCount, DataPointSpacingMode dataPointSpacingMode = SpacingMode) {
        Chart = new() {
            Labels = defaultLabels.Take(labelCount).ToList(),
            DataPointSpacingMode = dataPointSpacingMode,
            StateHasChangedHandler = () => StateHasChangedInvoked = true
        };
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
        Chart.PlotArea.AutoScaleSettings = new() {
            Chart = Chart,
            PlotArea = Chart.PlotArea,
            IsEnabled = AutoScaleSettingsIsEnabled,
            RequestedGridLineCount = AutoScaleSettingsRequestedGridLineCount,
            IncludeZero = AutoScaleSettingsIncludeZero,
            ClearancePercentage = AutoScaleSettingsClearancePercentage
        };
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

    public XYChartBuilder WithPlotArea(decimal? min = null, decimal? max = null, decimal? gridLineInterval = null, decimal? multiplier = null) {
        Chart.PlotArea.Min = min ?? Chart.PlotArea.Min;
        Chart.PlotArea.Max = max ?? Chart.PlotArea.Max;
        Chart.PlotArea.GridLineInterval = gridLineInterval ?? Chart.PlotArea.GridLineInterval;
        Chart.PlotArea.Multiplier = multiplier ?? Chart.PlotArea.Multiplier;
        return this;
    }

    public XYChartBuilder WithAutoScaleSettings(bool isEnabled, int? requestedGridLineCount = null, bool? includeZero = null, decimal? clearancePercentage = null) {
        Chart.PlotArea.AutoScaleSettings.IsEnabled = isEnabled;
        Chart.PlotArea.AutoScaleSettings.RequestedGridLineCount = requestedGridLineCount ?? Chart.PlotArea.AutoScaleSettings.RequestedGridLineCount;
        Chart.PlotArea.AutoScaleSettings.IncludeZero = includeZero ?? Chart.PlotArea.AutoScaleSettings.IncludeZero;
        Chart.PlotArea.AutoScaleSettings.ClearancePercentage = clearancePercentage ?? Chart.PlotArea.AutoScaleSettings.ClearancePercentage;

        return this;
    }
}
