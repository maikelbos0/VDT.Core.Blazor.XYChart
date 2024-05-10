using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class XYChartBuilder {
    private static readonly List<string> defaultLabels = new() { "Foo", "Bar", "Baz", "Qux", "Quux" };

    public XYChart Chart { get; }
    internal ISizeProvider SizeProvider { get; }
    public bool StateHasChangedInvoked { get; private set; }

    public XYChartBuilder(int labelCount = Chart_LabelCount, DataPointSpacingMode dataPointSpacingMode = Chart_DataPointSpacingMode) {
        SizeProvider = Substitute.For<ISizeProvider>();
        Chart = new() {
            Labels = defaultLabels.Take(labelCount).ToList(),
            DataPointSpacingMode = dataPointSpacingMode,
            StateChangeHandler = new(),
            SizeProviderProvider = () => Task.FromResult(SizeProvider)
        };
        Chart.Canvas = new() {
            Chart = Chart,
            Width = Canvas_Width,
            Height = Canvas_Height,
            Padding = Canvas_Padding,
            XAxisLabelHeight = Canvas_XAxisLabelHeight,
            YAxisLabelWidth = Canvas_YAxisLabelWidth,
            YAxisLabelFormat = Canvas_YAxisLabelFormat,
            YAxisMultiplierFormat = Canvas_YAxisMultiplierFormat,
            DataLabelFormat = Canvas_DataLabelFormat,
        };
        Chart.Legend = new() {
            Chart = Chart,
            IsEnabled = Legend_IsEnabled,
            Position = Legend_Position,
            Alignment = Legend_Alignment,
            ItemWidth = Legend_ItemWidth,
            ItemHeight = Legend_ItemHeight,
            KeySize = Legend_KeySize
        };
        Chart.PlotArea = new() {
            Chart = Chart,
            Min = PlotArea_Min,
            Max = PlotArea_Max,
            GridLineInterval = PlotArea_GridLineInterval,
            Multiplier = PlotArea_Multiplier,
            AutoScaleIsEnabled = PlotArea_AutoScaleIsEnabled,
            AutoScaleRequestedGridLineCount = PlotArea_AutoScaleRequestedGridLineCount,
            AutoScaleIncludesZero = PlotArea_AutoScaleIncludesZero,
            AutoScaleClearancePercentage = PlotArea_AutoScaleClearancePercentage
        };
        Chart.StateChangeHandler.Subscribe(() => StateHasChangedInvoked = true);
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

    public XYChartBuilder WithLegend(bool? isEnabled = null, LegendPosition? position = null, LegendAlignment? alignment = null, int? itemWidth = null, int? itemHeight = null, int? keySize = null)
        => WithLegend(new Legend() {
            IsEnabled = isEnabled ?? Legend_IsEnabled,
            Position = position ?? Legend_Position,
            Alignment = alignment ?? Legend_Alignment,
            ItemWidth = itemWidth ?? Legend_ItemWidth,
            ItemHeight = itemHeight ?? Legend_ItemHeight,
            KeySize = keySize ?? Legend_KeySize
        });

    public XYChartBuilder WithLegend(Legend legend) {
        legend.Chart = Chart;
        Chart.Legend = legend;
        return this;
    }

    public XYChartBuilder WithPlotArea(decimal? min = null, decimal? max = null, decimal? gridLineInterval = null, decimal? multiplier = null, bool? autoScaleIsEnabled = null, int? autoScaleRequestedGridLineCount = null, bool? autoScaleIncludesZero = null, decimal? autoScaleClearancePercentage = null)
        => WithPlotArea(new PlotArea() {
            Min = min ?? PlotArea_Min,
            Max = max ?? PlotArea_Max,
            GridLineInterval = gridLineInterval ?? PlotArea_GridLineInterval,
            Multiplier = multiplier ?? PlotArea_Multiplier,
            AutoScaleIsEnabled = autoScaleIsEnabled ?? PlotArea_AutoScaleIsEnabled,
            AutoScaleRequestedGridLineCount = autoScaleRequestedGridLineCount ?? PlotArea_AutoScaleRequestedGridLineCount,
            AutoScaleIncludesZero = autoScaleIncludesZero ?? PlotArea_AutoScaleIncludesZero,
            AutoScaleClearancePercentage = autoScaleClearancePercentage ?? PlotArea_AutoScaleClearancePercentage
        });

    public XYChartBuilder WithPlotArea(PlotArea plotArea) {
        Chart.PlotArea = plotArea;
        plotArea.Chart = Chart;
        return this;
    }

    public XYChartBuilder WithCanvas(int? width = null, int? height = null, int? padding = null, int? xAxisLabelHeight = null, int? yAxisLabelWidth = null, string? yAxisLabelFormat = null, string? yAxisMultiplierFormat = null, string? dataLabelFormat = null, bool? autoSizeXAxisLabelsIsEnabled = null)
        => WithCanvas(new Canvas() {
            Width = width ?? Canvas_Width,
            Height = height ?? Canvas_Height,
            Padding = padding ?? Canvas_Padding,
            XAxisLabelHeight = xAxisLabelHeight ?? Canvas_XAxisLabelHeight,
            YAxisLabelWidth = yAxisLabelWidth ?? Canvas_YAxisLabelWidth,
            YAxisLabelFormat = yAxisLabelFormat ?? Canvas_YAxisLabelFormat,
            YAxisMultiplierFormat = yAxisMultiplierFormat ?? Canvas_YAxisMultiplierFormat,
            DataLabelFormat = dataLabelFormat ?? Canvas_DataLabelFormat,
            AutoSizeXAxisLabelsIsEnabled = autoSizeXAxisLabelsIsEnabled ?? Canvas_AutoSizeXAxisLabelsIsEnabled,
        });

    public XYChartBuilder WithCanvas(Canvas canvas) {
        Chart.Canvas = canvas;
        canvas.Chart = Chart;
        return this;
    }
}
