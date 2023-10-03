namespace VDT.Core.Blazor.XYChart.Examples.Shared;

public class ExampleData {
    public static List<string> Labels => new() { "Foo", "Bar", "Baz", "Quux", "Quuux" };

    public static List<ExampleData> BarData = new() {
        new("Bar", "#f1c40f", 11000, 19000, 31500, -2500, 9500),
        new("Baz", "#00cc66", 21000, -12000, 15500, -4000, 3500)
    };
    public static List<ExampleData> LineData = new() {
        new("Line", "#2274a5", 12500, 9500, 20500, 14500, 11000),
        new("Linz", "#d90368", 6500, -2500, null, 4000, 2000)
    };
    public static List<ExampleData> AreaData = new() {
        new("Area", "#ff9933", 7000, 12500, 8500, 3500, 11000),
        new("Arez", "#cc99ff", 2500, -2500, 3500, 5000, 3000)
    };

    public string Name { get; }
    public string Color { get; }
    public List<decimal?> DataPoints { get; }

    public ExampleData(string name, string color, params decimal?[] dataPoints) {
        Name = name;
        Color = color;
        DataPoints = dataPoints.ToList(); // TODO?
    }
}
