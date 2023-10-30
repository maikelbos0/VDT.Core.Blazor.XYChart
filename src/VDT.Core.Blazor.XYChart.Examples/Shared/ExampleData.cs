﻿namespace VDT.Core.Blazor.XYChart.Examples.Shared;

public class ExampleData {
    public static List<string> Labels => new() { "Foo", "Bar", "Baz", "Qux", "Quux", "Corge", "Grault", "Garply" };

    public static List<ExampleData> BarData => new() {
        new("Bar", "#ffcc11", 110000, 190000, 315000, -25000, 95000, 45000, 35000, 40000),
        new("Baz", "#22cc55", 210000, -120000, 155000, -40000, 35000, 25000, -5000, 30000)
    };
    public static List<ExampleData> LineData => new() {
        new("Line", "#3366bb", 125000, 95000, 205000, 145000, 110000, 155000, 135000, 140000),
        new("Linz", "#dd3377", 65000, -25000, null, 40000, 20000, null, 55000, 70000)
    };
    public static List<ExampleData> AreaData => new() {
        new("Area", "#ff9933", 70000, 125000, 85000, 35000, 110000, 80000, 85000, 60000),
        new("Arez", "#aa66ee", 25000, -25000, 35000, 50000, 30000, null, null, 45000)
    };

    public string Name { get; }
    public string Color { get; }
    public IList<decimal?> DataPoints { get; }

    public ExampleData(string name, string color, params decimal?[] dataPoints) {
        Name = name;
        Color = color;
        DataPoints = dataPoints;
    }
}
