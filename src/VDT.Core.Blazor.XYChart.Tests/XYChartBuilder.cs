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
            YAxisLabelClearance = CanvasYAxisLabelClearance,
            YAxisLabelFormat = CanvasYAxisLabelFormat,
            YAxisMultiplierFormat = CanvasYAxisMultiplierFormat,
            DataLabelFormat = CanvasDataLabelFormat
        };
        Chart.PlotArea = new() {
            Chart = Chart,
            Min = PlotAreaMin,
            Max = PlotAreaMax,
            GridLineInterval = PlotAreaGridLineInterval,
            Multiplier = PlotAreaMultiplier
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

    public XYChartBuilder WithDataSeries(params decimal?[] dataPoints)
        => WithDataSeries(new DataSeries() {
            DataPoints = dataPoints,
            Chart = Chart,
            Layer = Chart.Layers.Last()
        });

    public XYChartBuilder WithDataSeries(DataSeries dataSeries) {
        dataSeries.Chart = Chart;
        dataSeries.Layer = Chart.Layers.Last();
        Chart.Layers.Last().DataSeries.Add(dataSeries);
        return this;
    }

    public XYChartBuilder WithPlotArea(decimal? min = null, decimal? max = null, decimal? gridLineInterval = null, decimal? multiplier = null)
        => WithPlotArea(new PlotArea() {
            Min = min ?? PlotAreaMin,
            Max = max ?? PlotAreaMax,
            GridLineInterval = gridLineInterval ?? PlotAreaGridLineInterval,
            Multiplier = multiplier ?? PlotAreaMultiplier
        });

    public XYChartBuilder WithPlotArea(PlotArea plotArea) {
        Chart.PlotArea = plotArea;
        plotArea.Chart = Chart;
        return this;
    }

    public XYChartBuilder WithAutoScaleSettings(bool? isEnabled = null, int? requestedGridLineCount = null, bool? includeZero = null, decimal? clearancePercentage = null)
        => WithAutoScaleSettings(new AutoScaleSettings() {
            IsEnabled = isEnabled ?? AutoScaleSettingsIsEnabled,
            RequestedGridLineCount = requestedGridLineCount ?? AutoScaleSettingsRequestedGridLineCount,
            IncludeZero = includeZero ?? AutoScaleSettingsIncludeZero,
            ClearancePercentage = clearancePercentage ?? AutoScaleSettingsClearancePercentage
        });

    public XYChartBuilder WithAutoScaleSettings(AutoScaleSettings autoScaleSettings) {
        Chart.PlotArea.AutoScaleSettings = autoScaleSettings;
        autoScaleSettings.Chart = Chart;
        autoScaleSettings.PlotArea = Chart.PlotArea;
        return this;
    }
}
