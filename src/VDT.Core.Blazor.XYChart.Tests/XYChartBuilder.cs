using System.Collections.Generic;
using System.Linq;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class XYChartBuilder {
    private static readonly List<string> defaultLabels = new() { "Foo", "Bar", "Baz", "Qux", "Quux" };

    public XYChart Chart { get; }
    public bool StateHasChangedInvoked { get; private set; }

    public XYChartBuilder(int labelCount = Chart_LabelCount, DataPointSpacingMode dataPointSpacingMode = Chart_DataPointSpacingMode) {
        Chart = new() {
            Labels = defaultLabels.Take(labelCount).ToList(),
            DataPointSpacingMode = dataPointSpacingMode,
            StateHasChangedHandler = () => StateHasChangedInvoked = true
        };
        Chart.Canvas = new() {
            Chart = Chart,
            Width = Canvas_Width,
            Height = Canvas_Height,
            Padding = Canvas_Padding,
            XAxisLabelHeight = Canvas_XAxisLabelHeight,
            XAxisLabelClearance = Canvas_XAxisLabelClearance,
            YAxisLabelWidth = Canvas_YAxisLabelWidth,
            YAxisLabelClearance = Canvas_YAxisLabelClearance,
            YAxisLabelFormat = Canvas_YAxisLabelFormat,
            YAxisMultiplierFormat = Canvas_YAxisMultiplierFormat,
            DataLabelFormat = Canvas_DataLabelFormat,
        };
        Chart.Legend = new() {
            Position = Legend_Position,
            Height = Legend_Height
            // TODO legend item, other legend properties
        };
        Chart.PlotArea = new() {
            Chart = Chart,
            Min = PlotArea_Min,
            Max = PlotArea_Max,
            GridLineInterval = PlotArea_GridLineInterval,
            Multiplier = PlotArea_Multiplier
        };
        Chart.AutoScaleSettings = new() {
            Chart = Chart,
            IsEnabled = AutoScaleSettings_IsEnabled,
            RequestedGridLineCount = AutoScaleSettings_RequestedGridLineCount,
            IncludeZero = AutoScaleSettings_IncludeZero,
            ClearancePercentage = AutoScaleSettings_ClearancePercentage
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
            Min = min ?? PlotArea_Min,
            Max = max ?? PlotArea_Max,
            GridLineInterval = gridLineInterval ?? PlotArea_GridLineInterval,
            Multiplier = multiplier ?? PlotArea_Multiplier
        });

    public XYChartBuilder WithPlotArea(PlotArea plotArea) {
        Chart.PlotArea = plotArea;
        plotArea.Chart = Chart;
        return this;
    }

    public XYChartBuilder WithAutoScaleSettings(bool? isEnabled = null, int? requestedGridLineCount = null, bool? includeZero = null, decimal? clearancePercentage = null)
        => WithAutoScaleSettings(new AutoScaleSettings() {
            IsEnabled = isEnabled ?? AutoScaleSettings_IsEnabled,
            RequestedGridLineCount = requestedGridLineCount ?? AutoScaleSettings_RequestedGridLineCount,
            IncludeZero = includeZero ?? AutoScaleSettings_IncludeZero,
            ClearancePercentage = clearancePercentage ?? AutoScaleSettings_ClearancePercentage
        });

    public XYChartBuilder WithAutoScaleSettings(AutoScaleSettings autoScaleSettings) {
        Chart.AutoScaleSettings = autoScaleSettings;
        autoScaleSettings.Chart = Chart;
        return this;
    }
}
