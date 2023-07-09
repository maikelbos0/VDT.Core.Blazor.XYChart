﻿using BlazorPlayground.Chart.Shapes;

namespace BlazorPlayground.Chart;

public class XYChart {
    public const string FallbackColor = "#000000";

    public static bool DefaultAutoScale { get; set; } = true;
    public static List<string> DefaultColors { get; set; } = new() {
        // https://coolors.co/550527-688e26-faa613-f44708-a10702
        // "#550527", "#688e26", "#faa613", "#f44708", "#a10702"

        // https://coolors.co/264653-2a9d8f-e9c46a-f4a261-e76f51
        // "#264653", "#2a9d8f", "#e9c46a", "#f4a261", "#e76f51"
        
        // https://coolors.co/2274a5-f75c03-f1c40f-d90368-00cc66
        "#2274a5", "#f75c03", "#f1c40f", "#d90368", "#00cc66"
    };

    public static string GetColor(int index) {
        if (DefaultColors.Any() && index >= 0) {
            return DefaultColors[index % DefaultColors.Count];
        }

        return FallbackColor;
    }

    public Canvas Canvas = new();
    public PlotArea PlotArea { get; set; } = new();
    public List<string> Labels { get; set; } = new();
    public List<DataSeries> DataSeries { get; set; } = new();
    public AutoScaleSettings AutoScaleSettings { get; set; } = new();

    public IEnumerable<ShapeBase> GetShapes() {
        PlotArea.AutoScale(AutoScaleSettings, DataSeries.SelectMany(dataSeries => dataSeries.Where(dataPoint => dataPoint != null).Select(dataPoint => dataPoint!.Value)));

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

        if (GetYAxisMultiplierShape() is ShapeBase multiplierShape) 
            yield return multiplierShape;
    }

    public IEnumerable<GridLineShape> GetGridLineShapes() 
        => PlotArea.GetGridLineDataPoints().Select(dataPoint => new GridLineShape(Canvas.PlotAreaX, MapDataPointToCanvas(dataPoint), Canvas.PlotAreaWidth, dataPoint));

    public IEnumerable<YAxisLabelShape> GetYAxisLabelShapes()
        => PlotArea.GetGridLineDataPoints().Select(dataPoint => new YAxisLabelShape(Canvas.PlotAreaX - Canvas.YAxisLabelClearance, MapDataPointToCanvas(dataPoint), (dataPoint / PlotArea.Multiplier).ToString(Canvas.YAxisLabelFormat)));

    public YAxisMultiplierShape? GetYAxisMultiplierShape()
        => PlotArea.Multiplier == 1M ? null : new YAxisMultiplierShape(Canvas.Padding, Canvas.PlotAreaY + Canvas.PlotAreaHeight / 2M, PlotArea.Multiplier.ToString(Canvas.YAxisMultiplierFormat));

    // Temporary - we'll need to abstract this out to DataSeries if we want different types of series rendered
    public IEnumerable<BarDataShape> GetDataSeriesShapes() {
        var zeroY = MapDataPointToCanvas(0M);

        return DataSeries.SelectMany(dataSeries => dataSeries
            .Select((dataPoint, index) => (DataPoint: dataPoint, Index: index))
            .Where(value => value.DataPoint != null && value.Index < Labels.Count)
            .Select(value => {
                var y = MapDataPointToCanvas(value.DataPoint!.Value);
                decimal height;

                if (y < zeroY) {
                    height = zeroY - y;
                }
                else {
                    height = y - zeroY;
                    y = zeroY;
                }

                return new BarDataShape(
                    x: MapDataIndexToCanvas(value.Index) - 5, // TODO width
                    y: y,
                    width: 10, // TODO width
                    height: height,
                    color: dataSeries.Color
                );
            }));
    }

    public decimal MapDataPointToCanvas(decimal dataPoint) => Canvas.PlotAreaY + (PlotArea.Max - dataPoint) / (PlotArea.Max - PlotArea.Min) * Canvas.PlotAreaHeight;

    public decimal MapDataIndexToCanvas(int index) => Canvas.PlotAreaX + (index + 0.5M) * Canvas.PlotAreaWidth / Labels.Count;

    public DataSeries AddDataSeries(string name) => AddDataSeries(name, null);

    public DataSeries AddDataSeries(string name, string? color) {
        var dataSeries = new DataSeries(name, color ?? GetColor(DataSeries.Count));

        dataSeries.AddRange(Enumerable.Range(0, Labels.Count).Select<int, decimal?>(i => null));
        DataSeries.Add(dataSeries);

        return dataSeries;
    }

    public void AddDataPoint(string label) {
        Labels.Add(label);

        foreach (var dataSeries in DataSeries) {
            dataSeries.AddRange(Enumerable.Range(0, Labels.Count - dataSeries.Count).Select<int, decimal?>(i => null));
        }
    }
}
