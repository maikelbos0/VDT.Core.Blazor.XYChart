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

        var subject = new XYChart {
            Labels = new() { "Foo", "Bar" },
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
        var stateHasChangedInvoked = false;
        var canvas = new Canvas();
        var subject = new XYChart() {
            StateHasChangedHandler = () => stateHasChangedInvoked = true
        };

        subject.SetCanvas(canvas);

        Assert.Same(canvas, subject.Canvas);
        Assert.True(stateHasChangedInvoked);
    }

    [Fact]
    public void ResetCanvas() {
        var stateHasChangedInvoked = false;
        var canvas = new Canvas();
        var subject = new XYChart() {
            StateHasChangedHandler = () => stateHasChangedInvoked = true,
            Canvas = canvas
        };

        subject.ResetCanvas();

        Assert.NotSame(canvas, subject.Canvas);
        Assert.True(stateHasChangedInvoked);
    }

    [Fact]
    public void SetPlotArea() {
        var stateHasChangedInvoked = false;
        var plotArea = new PlotArea();
        var subject = new XYChart() {
            StateHasChangedHandler = () => stateHasChangedInvoked = true
        };

        subject.SetPlotArea(plotArea);

        Assert.Same(plotArea, subject.PlotArea);
        Assert.True(stateHasChangedInvoked);
    }

    [Fact]
    public void ResetPlotArea() {
        var stateHasChangedInvoked = false;
        var plotArea = new PlotArea();
        var subject = new XYChart() {
            StateHasChangedHandler = () => stateHasChangedInvoked = true,
            PlotArea = plotArea
        };

        subject.ResetPlotArea();

        Assert.NotSame(plotArea, subject.PlotArea);
        Assert.True(stateHasChangedInvoked);
    }

    [Fact]
    public void AddLayer() {
        var stateHasChangedInvoked = false;
        var layer = new BarLayer();
        var subject = new XYChart() {
            StateHasChangedHandler = () => stateHasChangedInvoked = true
        };

        subject.AddLayer(layer);

        Assert.Same(layer, Assert.Single(subject.Layers));
        Assert.True(stateHasChangedInvoked);
    }

    [Fact]
    public void RemoveLayer() {
        var stateHasChangedInvoked = false;
        var layer = new BarLayer();
        var subject = new XYChart() {
            StateHasChangedHandler = () => stateHasChangedInvoked = true,
            Layers = {
                layer
            }
        };

        subject.RemoveLayer(layer);

        Assert.Empty(subject.Layers);
        Assert.True(stateHasChangedInvoked);
    }

    [Fact]
    public void GetShapes_AutoScale() {
        var subject = new XYChart() {
            PlotArea = {
                Min = -4M,
                Max = 10M,
                GridLineInterval = 1M,
                AutoScaleSettings = {
                    IsEnabled = true,
                    ClearancePercentage = 0M
                }
            },
            Canvas = {
                Height = 800,
                Padding = 25,
                XAxisLabelHeight = 50
            },
            Labels = { "Foo", "Bar", "Baz" },
        };

        var layer = new BarLayer() {
            Chart = subject,
            IsStacked = false
        };

        layer.DataSeries.Add(new() {
            Chart = subject,
            Layer = layer,
            Color = "blue",
            DataPoints = { -9M, 0M }
        });
        layer.DataSeries.Add(new() {
            Chart = subject,
            Layer = layer,
            Color = "red",
            DataPoints = { -5M, 19M }
        });
        subject.Layers.Add(layer);

        _ = subject.GetShapes().ToList();

        Assert.Equal(-10M, subject.PlotArea.Min);
        Assert.Equal(20M, subject.PlotArea.Max);
        Assert.Equal(2M, subject.PlotArea.GridLineInterval);
    }

    [Fact]
    public void GetShapes_No_AutoScale() {
        var subject = new XYChart() {
            PlotArea = {
                Min = -4M,
                Max = 10M,
                GridLineInterval = 1M,
                AutoScaleSettings = {
                    IsEnabled = false
                }
            },
            Canvas = {
                Height = 800,
                Padding = 25,
                XAxisLabelHeight = 50
            },
            Labels = { "Foo", "Bar", "Baz" }
        };

        var layer = new BarLayer() {
            Chart = subject,
            IsStacked = false
        };

        layer.DataSeries.Add(new() {
            Chart = subject,
            Layer = layer,
            Color = "blue",
            DataPoints = { -9M, 0M }
        });
        layer.DataSeries.Add(new() {
            Chart = subject,
            Layer = layer,
            Color = "red",
            DataPoints = { -5M, 19M }
        });
        subject.Layers.Add(layer);

        _ = subject.GetShapes().ToList();

        Assert.Equal(-4M, subject.PlotArea.Min);
        Assert.Equal(10M, subject.PlotArea.Max);
        Assert.Equal(1M, subject.PlotArea.GridLineInterval);
    }

    [Fact]
    public void GetShapes_PlotAreaShape() {
        var subject = new XYChart();

        Assert.Single(subject.GetShapes(), shape => shape is PlotAreaShape);
    }

    [Fact]
    public void GetShapes_GridLineShapes() {
        var subject = new XYChart();

        Assert.Contains(subject.GetShapes(), shape => shape is GridLineShape);
    }

    [Fact]
    public void GetShapes_YAxisLabelShapes() {
        var subject = new XYChart();

        Assert.Contains(subject.GetShapes(), shape => shape is YAxisLabelShape);
    }

    [Fact]
    public void GetShapes_YAxisMultiplierShape() {
        var subject = new XYChart() {
            PlotArea = {
                Multiplier = 1000
            }
        };

        Assert.Single(subject.GetShapes(), shape => shape is YAxisMultiplierShape);
    }

    [Fact]
    public void GetShapes_YAxisMultiplierShape_Without_Multiplier() {
        var subject = new XYChart();

        Assert.DoesNotContain(subject.GetShapes(), shape => shape == null);
    }

    [Fact]
    public void GetShapes_XAxisLabelShapes() {
        var subject = new XYChart() {
            Labels = { "Foo", "Bar" }
        };

        Assert.Contains(subject.GetShapes(), shape => shape is XAxisLabelShape);
    }

    [Fact]
    public void GetShapes_DataSeriesShapes() {
        var subject = new XYChart() {
            Labels = { "Foo", "Bar" }
        };

        var layer = new BarLayer() {
            Chart = subject,
            IsStacked = false
        };

        layer.DataSeries.Add(new() {
            Chart = subject,
            Layer = layer,
            Color = "blue",
            DataPoints = { 5M, 10M }
        });
        subject.Layers.Add(layer);

        Assert.Contains(subject.GetShapes(), shape => shape is BarDataShape);
    }

    [Fact]
    public void GetGridLineShapes() {
        var subject = new XYChart() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 75,
                YAxisLabelClearance = 10
            },
            PlotArea = {
                 Min = -100M,
                 Max = 500M,
                 GridLineInterval = 200M
            }
        };

        var result = subject.GetGridLineShapes();

        Assert.Equal(3, result.Count());

        Assert.All(result, shape => {
            Assert.Equal(25 + 75, shape.X);
            Assert.Equal(1000 - 25 - 25 - 75, shape.Width);
        });

        var plotAreaRange = subject.PlotArea.Max - subject.PlotArea.Min;

        Assert.Single(result, shape => shape.Key.EndsWith("[0]") && shape.Y == subject.Canvas.PlotAreaY + (400M - subject.PlotArea.Min) / plotAreaRange * subject.Canvas.PlotAreaHeight);
        Assert.Single(result, shape => shape.Key.EndsWith("[1]") && shape.Y == subject.Canvas.PlotAreaY + (200M - subject.PlotArea.Min) / plotAreaRange * subject.Canvas.PlotAreaHeight);
        Assert.Single(result, shape => shape.Key.EndsWith("[2]") && shape.Y == subject.Canvas.PlotAreaY + (0M - subject.PlotArea.Min) / plotAreaRange * subject.Canvas.PlotAreaHeight);
    }

    [Fact]
    public void GetYAxisLabelShapes() {
        var subject = new XYChart() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 75,
                YAxisLabelClearance = 10,
                YAxisLabelFormat = "#000"
            },
            PlotArea = {
                 Min = -100M,
                 Max = 500M,
                 GridLineInterval = 200M,
                 Multiplier = 10M
            }
        };

        var result = subject.GetYAxisLabelShapes();

        Assert.Equal(3, result.Count());

        Assert.All(result, shape => {
            Assert.Equal(25 + 75 - 10, shape.X);
        });

        var plotAreaRange = subject.PlotArea.Max - subject.PlotArea.Min;

        Assert.Single(result, shape => shape.Key.EndsWith("[0]") && shape.Y == subject.Canvas.PlotAreaY + (400M - subject.PlotArea.Min) / plotAreaRange * subject.Canvas.PlotAreaHeight && shape.Value == "000");
        Assert.Single(result, shape => shape.Key.EndsWith("[1]") && shape.Y == subject.Canvas.PlotAreaY + (200M - subject.PlotArea.Min) / plotAreaRange * subject.Canvas.PlotAreaHeight && shape.Value == "020");
        Assert.Single(result, shape => shape.Key.EndsWith("[2]") && shape.Y == subject.Canvas.PlotAreaY + (0M - subject.PlotArea.Min) / plotAreaRange * subject.Canvas.PlotAreaHeight && shape.Value == "040");
    }

    [Fact]
    public void GetYAxisMultiplierShape() {
        var subject = new XYChart() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 75,
                YAxisLabelClearance = 10,
                YAxisMultiplierFormat = "x 0000"
            },
            PlotArea = {
                 Min = -100M,
                 Max = 500M,
                 GridLineInterval = 200M,
                 Multiplier = 1000
            }
        };

        var result = subject.GetYAxisMultiplierShape();

        Assert.NotNull(result);
        Assert.Equal(25, result.X);
        Assert.Equal(25 + (500 - 25 - 25 - 50) / 2, result.Y);
        Assert.Equal("x 1000", result.Multiplier);
    }

    [Fact]
    public void GetYAxisMultiplierShape_Without_Multiplier() {
        var subject = new XYChart() {
            Canvas = {
                Width = 1000,
                Height = 500,
                Padding = 25,
                XAxisLabelHeight = 50,
                XAxisLabelClearance = 5,
                YAxisLabelWidth = 75,
                YAxisLabelClearance = 10,
                YAxisMultiplierFormat = "x 0000"
            },
            PlotArea = {
                 Min = -100M,
                 Max = 500M,
                 GridLineInterval = 200M
            }
        };

        var result = subject.GetYAxisMultiplierShape();

        Assert.Null(result);
    }

    [Fact]
    public void GetXAxisLabelShapes() {
        var subject = new XYChartBuilder()
            .WithLabelCount(3)
            .WithDataPointSpacingMode(DataPointSpacingMode.Center)
            .GetChart();

        var result = subject.GetXAxisLabelShapes();

        Assert.Equal(3, result.Count());

        Assert.All(result, shape => {
            Assert.Equal(PlotAreaY + PlotAreaHeight + CanvasXAxisLabelClearance, shape.Y);
        });

        Assert.Single(result, shape => shape.Key.EndsWith("[0]") && shape.X == PlotAreaX + 0.5M * PlotAreaWidth / 3M && shape.Label == "Foo");
        Assert.Single(result, shape => shape.Key.EndsWith("[1]") && shape.X == PlotAreaX + 1.5M * PlotAreaWidth / 3M && shape.Label == "Bar");
        Assert.Single(result, shape => shape.Key.EndsWith("[2]") && shape.X == PlotAreaX + 2.5M * PlotAreaWidth / 3M && shape.Label == "Baz");
    }

    [Fact]
    public void GetDataSeriesShapes() {
        var subject = new XYChartBuilder()
            .WithLabelCount(3)
            .WithLayer<BarLayer>()
            .WithDataSeries(5M, null, 15M)
            .WithDataSeries(11M, 8M, null)
            .GetChart();

        var result = subject.GetDataSeriesShapes();

        Assert.Equal(4, result.Count());

        Assert.All(result, shape => Assert.IsType<BarDataShape>(shape));
    }

    [Theory]
    [MemberData(nameof(MapDataPointToCanvas_Data))]
    public void MapDataPointToCanvas(decimal dataPoint, decimal expectedValue) {
        var subject = new XYChartBuilder()
            .GetChart();

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
            .GetChart();

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
        var subject = layers.Aggregate(new XYChartBuilder(), (builder, layer) => builder.WithLayer(layer))
            .WithDataPointSpacingMode(dataPointSpacingMode)
            .GetChart();

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
        var subject = new XYChartBuilder()
            .WithLabelCount(labelCount)
            .WithDataPointSpacingMode(dataPointSpacingMode)
            .GetChart();

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
        var subject = new XYChartBuilder()
            .WithLabelCount(3)
            .WithDataPointSpacingMode(dataPointSpacingMode)
            .GetChart();

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
