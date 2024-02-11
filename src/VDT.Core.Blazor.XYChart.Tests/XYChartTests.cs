using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VDT.Core.Blazor.XYChart.Shapes;
using Xunit;
using static VDT.Core.Blazor.XYChart.Tests.Constants;

namespace VDT.Core.Blazor.XYChart.Tests;

public class XYChartTests {
    [Theory]
    [MemberData(nameof(HaveParametersChanged_Data))]
    public void HaveParametersChanged(List<string> labels, DataPointSpacingMode dataPointSpacingMode, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(XYChart.Labels), labels },
            { nameof(XYChart.DataPointSpacingMode), dataPointSpacingMode }
        });

        var subject = new XYChart() {
            Labels = { "Foo", "Bar" },
            DataPointSpacingMode = DataPointSpacingMode.Auto
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<List<string>, DataPointSpacingMode, bool> HaveParametersChanged_Data() => new() {
        { new List<string>() { "Foo", "Bar" }, DataPointSpacingMode.Auto, false },
        { new List<string>() { "Foo", "Baz" }, DataPointSpacingMode.Auto, true },
        { new List<string>() { "Foo", "Bar" }, DataPointSpacingMode.Center, true},
    };

    [Fact]
    public void SetCanvas() {
        var canvas = new Canvas();
        var builder = new XYChartBuilder();
        var subject = builder.Chart;

        subject.SetCanvas(canvas);

        Assert.Same(canvas, subject.Canvas);
        Assert.True(builder.StateHasChangedInvoked);
    }

    [Fact]
    public void ResetCanvas() {
        var builder = new XYChartBuilder();
        var subject = builder.Chart;
        var canvas = subject.Canvas;

        subject.ResetCanvas();

        Assert.NotSame(canvas, subject.Canvas);
        Assert.True(builder.StateHasChangedInvoked);
    }

    [Fact]
    public void SetLegend() {
        var legend = new Legend();
        var builder = new XYChartBuilder();
        var subject = builder.Chart;

        subject.SetLegend(legend);

        Assert.Same(legend, subject.Legend);
        Assert.True(builder.StateHasChangedInvoked);
    }

    [Fact]
    public void ResetLegend() {
        var builder = new XYChartBuilder();
        var subject = builder.Chart;
        var legend = subject.Legend;

        subject.ResetLegend();

        Assert.NotSame(legend, subject.Legend);
        Assert.True(builder.StateHasChangedInvoked);
    }

    [Fact]
    public void SetPlotArea() {
        var plotArea = new PlotArea();
        var builder = new XYChartBuilder();
        var subject = builder.Chart;

        subject.SetPlotArea(plotArea);

        Assert.Same(plotArea, subject.PlotArea);
        Assert.True(builder.StateHasChangedInvoked);
    }

    [Fact]
    public void ResetPlotArea() {
        var builder = new XYChartBuilder();
        var subject = builder.Chart;
        var plotArea = subject.PlotArea;

        subject.ResetPlotArea();

        Assert.NotSame(plotArea, subject.PlotArea);
        Assert.True(builder.StateHasChangedInvoked);
    }

    [Fact]
    public void AddLayer() {
        var layer = new BarLayer();
        var builder = new XYChartBuilder();
        var subject = builder.Chart;

        subject.AddLayer(layer);

        Assert.Same(layer, Assert.Single(subject.Layers));
        Assert.True(builder.StateHasChangedInvoked);
    }

    [Fact]
    public void RemoveLayer() {
        var layer = new BarLayer();
        var builder = new XYChartBuilder()
            .WithLayer(layer);
        var subject = builder.Chart;

        subject.RemoveLayer(layer);

        Assert.Empty(subject.Layers);
        Assert.True(builder.StateHasChangedInvoked);
    }

    [Fact]
    public void GetShapes_PlotAreaShape() {
        var subject = new XYChartBuilder()
            .Chart;

        Assert.Single(subject.GetShapes(), shape => shape is PlotAreaShape);
    }

    [Fact]
    public void GetShapes_GridLineShapes() {
        var subject = new XYChartBuilder()
            .Chart;

        Assert.Contains(subject.GetShapes(), shape => shape is GridLineShape);
    }

    [Fact]
    public void GetShapes_YAxisLabelShapes() {
        var subject = new XYChartBuilder()
            .Chart;

        Assert.Contains(subject.GetShapes(), shape => shape is YAxisLabelShape);
    }

    [Fact]
    public void GetShapes_YAxisMultiplierShape() {
        var subject = new XYChartBuilder()
            .WithPlotArea(multiplier: 1000)
            .Chart;

        Assert.Single(subject.GetShapes(), shape => shape is YAxisMultiplierShape);
    }

    [Fact]
    public void GetShapes_YAxisMultiplierShape_Without_Multiplier() {
        var subject = new XYChartBuilder()
            .WithPlotArea(multiplier: 1)
            .Chart;

        Assert.DoesNotContain(subject.GetShapes(), shape => shape == null);
    }

    [Fact]
    public void GetShapes_XAxisLabelShapes() {
        var subject = new XYChartBuilder()
            .Chart;

        Assert.Contains(subject.GetShapes(), shape => shape is XAxisLabelShape);
    }

    [Fact]
    public void GetShapes_DataSeriesShapes() {
        var subject = new XYChartBuilder(labelCount: 2)
            .WithLayer<BarLayer>()
            .WithDataSeries(5M, 10M)
            .Chart;

        Assert.Contains(subject.GetShapes(), shape => shape is BarDataShape);
    }

    [Fact]
    public void GetShapes_DataLabelShapes() {
        var subject = new XYChartBuilder(labelCount: 2)
            .WithLayer(new BarLayer() { ShowDataLabels = true })
            .WithDataSeries(5M, 10M)
            .Chart;

        Assert.Contains(subject.GetShapes(), shape => shape is DataLabelShape);
    }

    [Fact]
    public void GetShapes_LegendShapes() {
        var subject = new XYChartBuilder(labelCount: 2)
            .WithLayer(new BarLayer())
            .WithDataSeries(5M, 10M)
            .Chart;

        Assert.Contains(subject.GetShapes(), shape => shape is LegendKeyShape);
        Assert.Contains(subject.GetShapes(), shape => shape is LegendTextShape);
    }

    [Fact]
    public void GetGridLineShapes() {
        var subject = new XYChartBuilder()
            .Chart;

        var result = subject.GetGridLineShapes();

        Assert.Equal(PlotArea_Range / PlotArea_GridLineInterval, result.Count());

        Assert.All(result.Select((shape, index) => new { Shape = shape, Index = index }), value => {
            Assert.Equal(PlotArea_Y + (PlotArea_Max - PlotArea_GridLineInterval * value.Index) / PlotArea_Range * PlotArea_Height, value.Shape.Y);
            Assert.Equal(PlotArea_X, value.Shape.X);
            Assert.Equal(PlotArea_Width, value.Shape.Width);
            Assert.EndsWith($"[{value.Index}]", value.Shape.Key);
        });
    }

    [Fact]
    public void GetYAxisLabelShapes() {
        var subject = new XYChartBuilder()
            .Chart;

        var result = subject.GetYAxisLabelShapes();

        Assert.Equal(PlotArea_Range / PlotArea_GridLineInterval, result.Count());

        Assert.All(result.Select((shape, index) => new { Shape = shape, Index = index }), value => {
            Assert.Equal(PlotArea_Y + (PlotArea_Max - PlotArea_GridLineInterval * value.Index) / PlotArea_Range * PlotArea_Height, value.Shape.Y);
            Assert.Equal(PlotArea_X, value.Shape.X);
            Assert.Equal((PlotArea_GridLineInterval * value.Index).ToString(Canvas_YAxisLabelFormat), value.Shape.Value);
            Assert.EndsWith($"[{value.Index}]", value.Shape.Key);
        });
    }

    [Fact]
    public void GetYAxisMultiplierShape() {
        var subject = new XYChartBuilder()
            .WithPlotArea(multiplier: 1000)
            .Chart;

        var result = subject.GetYAxisMultiplierShape();

        Assert.NotNull(result);
        Assert.Equal(Canvas_Padding, result.X);
        Assert.Equal(PlotArea_Y + PlotArea_Height / 2M, result.Y);
        Assert.Equal(1000.ToString(Canvas_YAxisMultiplierFormat), result.Multiplier);
    }

    [Fact]
    public void GetYAxisMultiplierShape_Without_Multiplier() {
        var subject = new XYChartBuilder()
            .WithPlotArea(multiplier: 1)
            .Chart;

        var result = subject.GetYAxisMultiplierShape();

        Assert.Null(result);
    }

    [Fact]
    public void GetXAxisLabelShapes() {
        var subject = new XYChartBuilder(labelCount: 3, DataPointSpacingMode.Center)
            .Chart;

        var result = subject.GetXAxisLabelShapes();

        Assert.Equal(subject.Labels.Count, result.Count());

        Assert.All(result.Select((shape, index) => new { Shape = shape, Index = index }), value => {
            Assert.Equal(PlotArea_Y + PlotArea_Height, value.Shape.Y);
            Assert.EndsWith($"[{value.Index}]", value.Shape.Key);
            Assert.Equal(PlotArea_X + (0.5M + value.Index) * PlotArea_Width / subject.Labels.Count, value.Shape.X);
            Assert.Equal(subject.Labels[value.Index], value.Shape.Label);
        });
    }

    [Fact]
    public void GetDataSeriesShapes() {
        var subject = new XYChartBuilder(labelCount: 3)
            .WithLayer<BarLayer>()
            .WithDataSeries(5M, null, 15M)
            .WithDataSeries(11M, 8M, null)
            .Chart;

        var result = subject.GetDataSeriesShapes();

        Assert.Equal(4, result.Count());

        Assert.All(result, shape => Assert.IsType<BarDataShape>(shape));
    }

    [Fact]
    public void GetDataLabelShapes() {
        var subject = new XYChartBuilder(labelCount: 3)
            .WithLayer(new BarLayer() { ShowDataLabels = true })
            .WithDataSeries(5M, null, 15M)
            .WithDataSeries(11M, 8M, null)
            .Chart;

        var result = subject.GetDataLabelShapes();

        Assert.Equal(4, result.Count());

        Assert.All(result, shape => Assert.IsType<DataLabelShape>(shape));
    }

    [Fact]
    public void GetLegendShapes_Disabled() {
        var subject = new XYChartBuilder(labelCount: 3)
            .WithLegend(isEnabled: false)
            .WithLayer<BarLayer>()
            .WithDataSeries(5M, 15M, 25M)
            .Chart;

        var result = subject.GetLegendShapes();

        Assert.Empty(result);
    }

    [Theory]
    [MemberData(nameof(GetLegendShapes_KeyShapes_Data))]
    public void GetLegendShapes_KeyShapes(LegendPosition legendPosition, LegendAlignment legendAlignment, int layerIndex, int dataSeriesIndex, decimal expectedX, decimal expectedY) {
        var builder = new XYChartBuilder()
            .WithLegend(isEnabled: true, position: legendPosition, alignment: legendAlignment);

        for (var i = 0; i < 3; i++) {
            builder = builder
                .WithLayer(new BarLayer() { ShowDataLabels = true })
                .WithDataSeries(new DataSeries() { Color = "red", CssClass = "example-data" })
                .WithDataSeries(new DataSeries() { Color = "green", CssClass = "example-data" })
                .WithDataSeries(new DataSeries() { Color = "blue", CssClass = "example-data" });
        }

        var subject = builder.Chart;

        var result = subject.GetLegendShapes();

        var shape = Assert.IsType<LegendKeyShape>(Assert.Single(result, shape => shape.Key == $"{nameof(LegendKeyShape)}[{layerIndex},{dataSeriesIndex}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(Legend_KeySize, shape.Size);
        Assert.Equal(subject.Layers[layerIndex].DataSeries[dataSeriesIndex].Color, shape.Color);
        Assert.Equal("legend-key example-data", shape.CssClass);
    }

    public static TheoryData<LegendPosition, LegendAlignment, int, int, decimal, decimal> GetLegendShapes_KeyShapes_Data() => new() {
        { LegendPosition.Top, LegendAlignment.Left, 0, 0, PlotArea_X + Legend_KeyPadding, Canvas_Padding + Legend_KeyPadding },
        { LegendPosition.Top, LegendAlignment.Left, 0, 2, PlotArea_X + Legend_ItemWidth * 2 + Legend_KeyPadding, Canvas_Padding + Legend_KeyPadding },
        { LegendPosition.Top, LegendAlignment.Left, 2, 1, PlotArea_X + Legend_KeyPadding, Canvas_Padding + Legend_ItemHeight + Legend_KeyPadding },
        { LegendPosition.Top, LegendAlignment.Center, 0, 0, PlotArea_X + PlotArea_Width / 2M - 3.5M * Legend_ItemWidth + Legend_KeyPadding, Canvas_Padding + Legend_KeyPadding },
        { LegendPosition.Top, LegendAlignment.Center, 0, 2, PlotArea_X + PlotArea_Width / 2M - 1.5M * Legend_ItemWidth + Legend_KeyPadding, Canvas_Padding + Legend_KeyPadding },
        { LegendPosition.Top, LegendAlignment.Center, 2, 1, PlotArea_X + PlotArea_Width / 2M - Legend_ItemWidth + Legend_KeyPadding, Canvas_Padding + Legend_ItemHeight + Legend_KeyPadding },
        { LegendPosition.Top, LegendAlignment.Right, 0, 0, PlotArea_X + PlotArea_Width - 7M * Legend_ItemWidth + Legend_KeyPadding, Canvas_Padding + Legend_KeyPadding },
        { LegendPosition.Top, LegendAlignment.Right, 0, 2, PlotArea_X + PlotArea_Width - 5M * Legend_ItemWidth + Legend_KeyPadding, Canvas_Padding + Legend_KeyPadding },
        { LegendPosition.Top, LegendAlignment.Right, 2, 1, PlotArea_X + PlotArea_Width - 2M * Legend_ItemWidth + Legend_KeyPadding, Canvas_Padding + Legend_ItemHeight + Legend_KeyPadding },
        { LegendPosition.Bottom, LegendAlignment.Left, 0, 0, PlotArea_X + Legend_KeyPadding, Canvas_Padding + PlotArea_Height + Canvas_XAxisLabelHeight + Legend_KeyPadding },
        { LegendPosition.Bottom, LegendAlignment.Left, 0, 2, PlotArea_X + Legend_ItemWidth * 2 + Legend_KeyPadding, Canvas_Padding + PlotArea_Height + Canvas_XAxisLabelHeight + Legend_KeyPadding },
        { LegendPosition.Bottom, LegendAlignment.Left, 2, 1, PlotArea_X + Legend_KeyPadding, Canvas_Padding + PlotArea_Height + Canvas_XAxisLabelHeight + Legend_ItemHeight + Legend_KeyPadding }
    };

    [Theory]
    [MemberData(nameof(GetLegendShapes_TextShapes_Data))]
    public void GetLegendShapes_TextShapes(LegendPosition legendPosition, LegendAlignment legendAlignment, int layerIndex, int dataSeriesIndex, decimal expectedX, decimal expectedY) {
        var builder = new XYChartBuilder()
            .WithLegend(isEnabled: true, position: legendPosition, alignment: legendAlignment);

        for (var i = 0; i < 3; i++) {
            builder = builder
                .WithLayer(new BarLayer() { ShowDataLabels = true })
                .WithDataSeries(new DataSeries() { Name = "foo", CssClass = "example-data" })
                .WithDataSeries(new DataSeries() { Name = "bar", CssClass = "example-data" })
                .WithDataSeries(new DataSeries() { Name = "baz", CssClass = "example-data" });
        }

        var subject = builder.Chart;

        var result = subject.GetLegendShapes();

        var shape = Assert.IsType<LegendTextShape>(Assert.Single(result, shape => shape.Key == $"{nameof(LegendTextShape)}[{layerIndex},{dataSeriesIndex}]"));

        Assert.Equal(expectedX, shape.X);
        Assert.Equal(expectedY, shape.Y);
        Assert.Equal(subject.Layers[layerIndex].DataSeries[dataSeriesIndex].Name, shape.DataSeriesName);
        Assert.Equal("legend-text example-data", shape.CssClass);
    }

    public static TheoryData<LegendPosition, LegendAlignment, int, int, decimal, decimal> GetLegendShapes_TextShapes_Data() => new() {
        { LegendPosition.Top, LegendAlignment.Left, 0, 0, PlotArea_X + Legend_ItemHeight, Canvas_Padding + Legend_ItemHeight / 2M },
        { LegendPosition.Top, LegendAlignment.Left, 0, 2, PlotArea_X + Legend_ItemWidth * 2 + Legend_ItemHeight, Canvas_Padding + Legend_ItemHeight / 2M },
        { LegendPosition.Top, LegendAlignment.Left, 2, 1, PlotArea_X + Legend_ItemHeight, Canvas_Padding + Legend_ItemHeight + Legend_ItemHeight / 2M },
        { LegendPosition.Top, LegendAlignment.Center, 0, 0, PlotArea_X + PlotArea_Width / 2M - 3.5M * Legend_ItemWidth + Legend_ItemHeight, Canvas_Padding + Legend_ItemHeight / 2M },
        { LegendPosition.Top, LegendAlignment.Center, 0, 2, PlotArea_X + PlotArea_Width / 2M - 1.5M * Legend_ItemWidth + Legend_ItemHeight, Canvas_Padding + Legend_ItemHeight / 2M },
        { LegendPosition.Top, LegendAlignment.Center, 2, 1, PlotArea_X + PlotArea_Width / 2M - Legend_ItemWidth + Legend_ItemHeight, Canvas_Padding + Legend_ItemHeight + Legend_ItemHeight / 2M },
        { LegendPosition.Top, LegendAlignment.Right, 0, 0, PlotArea_X + PlotArea_Width - 7M * Legend_ItemWidth + Legend_ItemHeight, Canvas_Padding + Legend_ItemHeight / 2M },
        { LegendPosition.Top, LegendAlignment.Right, 0, 2, PlotArea_X + PlotArea_Width - 5M * Legend_ItemWidth + Legend_ItemHeight, Canvas_Padding + Legend_ItemHeight / 2M },
        { LegendPosition.Top, LegendAlignment.Right, 2, 1, PlotArea_X + PlotArea_Width - 2M * Legend_ItemWidth + Legend_ItemHeight, Canvas_Padding + Legend_ItemHeight + Legend_ItemHeight / 2M },
        { LegendPosition.Bottom, LegendAlignment.Left, 0, 0, PlotArea_X + Legend_ItemHeight, Canvas_Padding + PlotArea_Height + Canvas_XAxisLabelHeight + Legend_ItemHeight / 2M },
        { LegendPosition.Bottom, LegendAlignment.Left, 0, 2, PlotArea_X + Legend_ItemWidth * 2 + Legend_ItemHeight, Canvas_Padding + PlotArea_Height + Canvas_XAxisLabelHeight + Legend_ItemHeight / 2M },
        { LegendPosition.Bottom, LegendAlignment.Left, 2, 1, PlotArea_X + Legend_ItemHeight, Canvas_Padding + PlotArea_Height + Canvas_XAxisLabelHeight + Legend_ItemHeight + Legend_ItemHeight / 2M }
    };

    [Theory]
    [MemberData(nameof(MapDataPointToCanvas_Data))]
    public void MapDataPointToCanvas(decimal dataPoint, decimal expectedValue) {
        var subject = new XYChartBuilder()
            .Chart;

        Assert.Equal(expectedValue, subject.MapDataPointToCanvas(dataPoint));
    }

    public static TheoryData<decimal, decimal> MapDataPointToCanvas_Data() => new() {
        { 50M, PlotArea_Y + (PlotArea_Max - 50M) / PlotArea_Range * PlotArea_Height },
        { 200M, PlotArea_Y + (PlotArea_Max - 200M) / PlotArea_Range * PlotArea_Height },
        { 350M, PlotArea_Y + (PlotArea_Max - 350M) / PlotArea_Range * PlotArea_Height }
    };

    [Theory]
    [MemberData(nameof(MapDataValueToPlotArea_Data))]
    public void MapDataValueToPlotArea(decimal dataPoint, decimal expectedValue) {
        var subject = new XYChartBuilder()
            .Chart;

        Assert.Equal(expectedValue, subject.MapDataValueToPlotArea(dataPoint));
    }

    public static TheoryData<decimal, decimal> MapDataValueToPlotArea_Data() => new() {
        { 50M, 50M / PlotArea_Range * PlotArea_Height },
        { 200M, 200M / PlotArea_Range * PlotArea_Height },
        { 350M, 350M / PlotArea_Range * PlotArea_Height }
    };

    [Theory]
    [MemberData(nameof(GetDataPointSpacingMode_Data))]
    public void GetDataPointSpacingMode(DataPointSpacingMode dataPointSpacingMode, List<LayerBase> layers, DataPointSpacingMode expectedDataPointSpacingMode) {
        var subject = layers.Aggregate(new XYChartBuilder(dataPointSpacingMode: dataPointSpacingMode), (builder, layer) => builder.WithLayer(layer))
            .Chart;

        Assert.Equal(expectedDataPointSpacingMode, subject.GetDataPointSpacingMode());
    }

    public static TheoryData<DataPointSpacingMode, List<LayerBase>, DataPointSpacingMode> GetDataPointSpacingMode_Data() => new() {
        { DataPointSpacingMode.Center, new List<LayerBase> { new AreaLayer() }, DataPointSpacingMode.Center },
        { DataPointSpacingMode.EdgeToEdge, new List<LayerBase> { new BarLayer() }, DataPointSpacingMode.EdgeToEdge },
        { DataPointSpacingMode.Auto, new List<LayerBase>(), DataPointSpacingMode.EdgeToEdge },
        { DataPointSpacingMode.Auto, new List<LayerBase> { new AreaLayer() }, DataPointSpacingMode.EdgeToEdge },
        { DataPointSpacingMode.Auto, new List<LayerBase> { new BarLayer() }, DataPointSpacingMode.Center },
        { DataPointSpacingMode.Auto, new List<LayerBase> { new LineLayer() }, DataPointSpacingMode.Center }
    };

    [Theory]
    [MemberData(nameof(GetDataPointWidth_Data))]
    public void GetDataPointWidth(DataPointSpacingMode dataPointSpacingMode, int labelCount, decimal expectedWidth) {
        var subject = new XYChartBuilder(labelCount, dataPointSpacingMode)
            .Chart;

        Assert.Equal(expectedWidth, subject.GetDataPointWidth());
    }

    public static TheoryData<DataPointSpacingMode, int, decimal> GetDataPointWidth_Data() => new() {
        { DataPointSpacingMode.EdgeToEdge, 3, PlotArea_Width / 2M },
        { DataPointSpacingMode.Center, 3, PlotArea_Width / 3M },
        { DataPointSpacingMode.EdgeToEdge, 1, PlotArea_Width },
        { DataPointSpacingMode.Center, 1, PlotArea_Width },
        { DataPointSpacingMode.EdgeToEdge, 0, PlotArea_Width },
        { DataPointSpacingMode.Center, 0, PlotArea_Width },
    };

    [Theory]
    [MemberData(nameof(MapDataIndexToCanvas_Data))]
    public void MapDataIndexToCanvas(DataPointSpacingMode dataPointSpacingMode, int index, decimal expectedValue) {
        var subject = new XYChartBuilder(labelCount: 3, dataPointSpacingMode)
            .Chart;

        Assert.Equal(expectedValue, subject.MapDataIndexToCanvas(index));
    }

    public static TheoryData<DataPointSpacingMode, int, decimal> MapDataIndexToCanvas_Data() => new() {
        { DataPointSpacingMode.EdgeToEdge, 0, PlotArea_X },
        { DataPointSpacingMode.EdgeToEdge, 1, PlotArea_X + 1 * PlotArea_Width / 2M },
        { DataPointSpacingMode.EdgeToEdge, 2, PlotArea_X + 2 * PlotArea_Width / 2M },
        { DataPointSpacingMode.Center, 0, PlotArea_X + 0.5M * PlotArea_Width / 3M },
        { DataPointSpacingMode.Center, 1, PlotArea_X + 1.5M * PlotArea_Width / 3M },
        { DataPointSpacingMode.Center, 2, PlotArea_X + 2.5M * PlotArea_Width / 3M },
    };
}
