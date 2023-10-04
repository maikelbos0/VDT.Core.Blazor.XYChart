namespace VDT.Core.Blazor.XYChart.Examples.Shared;

public class ExampleData {
    public static List<string> Labels => new() { "Foo", "Bar", "Baz", "Qux", "Quux", "Corge", "Grault", "Garply" };

    public static List<ExampleData> BarData => new() {
        new("Bar", "#ffcc11", 11000, 19000, 31500, -2500, 9500, 4500, 3500, 4000),
        new("Baz", "#22cc55", 21000, -12000, 15500, -4000, 3500, 2500, -500, 3000)
    };
    public static List<ExampleData> LineData => new() {
        new("Line", "#3366bb", 12500, 9500, 20500, 14500, 11000, 15500, 13500, 14000),
        new("Linz", "#dd3377", 6500, -2500, null, 4000, 2000, null, 5500, 7000)
    };
    public static List<ExampleData> AreaData => new() {
        new("Area", "#ff9933", 7000, 12500, 8500, 3500, 11000, 8000, 8500, 6000),
        new("Arez", "#aa66ee", 2500, -2500, 3500, 5000, 3000, null, null, 4500)
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
