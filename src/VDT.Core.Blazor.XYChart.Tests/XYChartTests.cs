using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
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
    public void GetShapes_AutoScale() {
        var subject = new XYChartBuilder(labelCount: 2)
            .WithAutoScaleSettings(isEnabled: true, requestedGridLineCount: 15, clearancePercentage: 0M)
            .WithLayer<BarLayer>()
            .WithDataSeries(-9M, 0M)
            .WithDataSeries(-5M, 19M)
            .Chart;

        _ = subject.GetShapes().ToList();

        Assert.Equal(-10M, subject.PlotArea.Min);
        Assert.Equal(20M, subject.PlotArea.Max);
        Assert.Equal(2M, subject.PlotArea.GridLineInterval);
    }

    [Fact]
    public void GetShapes_No_AutoScale() {
        var subject = new XYChartBuilder(labelCount: 2)
            .WithAutoScaleSettings(isEnabled: false)
            .WithLayer<BarLayer>()
            .WithDataSeries(-9M, 0M)
            .WithDataSeries(-5M, 19M)
            .Chart;

        _ = subject.GetShapes().ToList();

        Assert.Equal(PlotAreaMin, subject.PlotArea.Min);
        Assert.Equal(PlotAreaMax, subject.PlotArea.Max);
        Assert.Equal(PlotAreaGridLineInterval, subject.PlotArea.GridLineInterval);
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
    public void GetGridLineShapes() {
        var subject = new XYChartBuilder()
            .Chart;

        var result = subject.GetGridLineShapes();

        Assert.Equal(PlotAreaRange / PlotAreaGridLineInterval, result.Count());

        Assert.All(result.Select((shape, index) => new { Shape = shape, Index = index }), value => {
            Assert.Equal(PlotAreaY + (PlotAreaMax - PlotAreaGridLineInterval * value.Index) / PlotAreaRange * PlotAreaHeight, value.Shape.Y);
            Assert.Equal(PlotAreaX, value.Shape.X);
            Assert.Equal(PlotAreaWidth, value.Shape.Width);
            Assert.EndsWith($"[{value.Index}]", value.Shape.Key);
        });
    }

    [Fact]
    public void GetYAxisLabelShapes() {
        var subject = new XYChartBuilder()
            .Chart;
        
        var result = subject.GetYAxisLabelShapes();

        Assert.Equal(PlotAreaRange / PlotAreaGridLineInterval, result.Count());

        Assert.All(result.Select((shape, index) => new { Shape = shape, Index = index }), value => {
            Assert.Equal(PlotAreaY + (PlotAreaMax - PlotAreaGridLineInterval * value.Index) / PlotAreaRange * PlotAreaHeight, value.Shape.Y);
            Assert.Equal(PlotAreaX - CanvasYAxisLabelClearance, value.Shape.X);
            Assert.Equal((PlotAreaGridLineInterval * value.Index).ToString(CanvasYAxisLabelFormat), value.Shape.Value);
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
        Assert.Equal(CanvasPadding, result.X);
        Assert.Equal(CanvasPadding + PlotAreaHeight / 2M, result.Y);
        Assert.Equal(1000.ToString(CanvasYAxisMultiplierFormat), result.Multiplier);
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
            Assert.Equal(PlotAreaY + PlotAreaHeight + CanvasXAxisLabelClearance, value.Shape.Y);
            Assert.EndsWith($"[{value.Index}]", value.Shape.Key);
            Assert.Equal(PlotAreaX + (0.5M + value.Index) * PlotAreaWidth / subject.Labels.Count, value.Shape.X);
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

    [Theory]
    [MemberData(nameof(MapDataPointToCanvas_Data))]
    public void MapDataPointToCanvas(decimal dataPoint, decimal expectedValue) {
        var subject = new XYChartBuilder()
            .Chart;

        Assert.Equal(expectedValue, subject.MapDataPointToCanvas(dataPoint));
    }

    public static TheoryData<decimal, decimal> MapDataPointToCanvas_Data() => new() {
        { 50M, PlotAreaY + (PlotAreaMax - 50M) / PlotAreaRange * PlotAreaHeight },
        { 200M, PlotAreaY + (PlotAreaMax - 200M) / PlotAreaRange * PlotAreaHeight },
        { 350M, PlotAreaY + (PlotAreaMax - 350M) / PlotAreaRange * PlotAreaHeight }
    };

    [Theory]
    [MemberData(nameof(MapDataValueToPlotArea_Data))]
    public void MapDataValueToPlotArea(decimal dataPoint, decimal expectedValue) {
        var subject = new XYChartBuilder()
            .Chart;

        Assert.Equal(expectedValue, subject.MapDataValueToPlotArea(dataPoint));
    }

    public static TheoryData<decimal, decimal> MapDataValueToPlotArea_Data() => new() {
        { 50M, 50M / PlotAreaRange * PlotAreaHeight },
        { 200M, 200M / PlotAreaRange * PlotAreaHeight },
        { 350M, 350M / PlotAreaRange * PlotAreaHeight }
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
        { DataPointSpacingMode.EdgeToEdge, 3, PlotAreaWidth / 2M },
        { DataPointSpacingMode.Center, 3, PlotAreaWidth / 3M },
        { DataPointSpacingMode.EdgeToEdge, 1, PlotAreaWidth },
        { DataPointSpacingMode.Center, 1, PlotAreaWidth },
        { DataPointSpacingMode.EdgeToEdge, 0, PlotAreaWidth },
        { DataPointSpacingMode.Center, 0, PlotAreaWidth },
    };

    [Theory]
    [MemberData(nameof(MapDataIndexToCanvas_Data))]
    public void MapDataIndexToCanvas(DataPointSpacingMode dataPointSpacingMode, int index, decimal expectedValue) {
        var subject = new XYChartBuilder(labelCount: 3, dataPointSpacingMode)
            .Chart;

        Assert.Equal(expectedValue, subject.MapDataIndexToCanvas(index));
    }

    public static TheoryData<DataPointSpacingMode, int, decimal> MapDataIndexToCanvas_Data() => new() {
        { DataPointSpacingMode.EdgeToEdge, 0, PlotAreaX },
        { DataPointSpacingMode.EdgeToEdge, 1, PlotAreaX + 1 * PlotAreaWidth / 2M },
        { DataPointSpacingMode.EdgeToEdge, 2, PlotAreaX + 2 * PlotAreaWidth / 2M },
        { DataPointSpacingMode.Center, 0, PlotAreaX + 0.5M * PlotAreaWidth / 3M },
        { DataPointSpacingMode.Center, 1, PlotAreaX + 1.5M * PlotAreaWidth / 3M },
        { DataPointSpacingMode.Center, 2, PlotAreaX + 2.5M * PlotAreaWidth / 3M },
    };
}
