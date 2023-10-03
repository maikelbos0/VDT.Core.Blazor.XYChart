namespace VDT.Core.Blazor.XYChart.Examples.Shared;

public static class ExampleData {
    public static List<string> Labels => new() { "Foo", "Bar", "Baz", "Quux", "Quuux" };
    public static List<List<decimal?>> BarDataSeries = new() {
        new() { 11000, 19000, 31500, -2500, 9500 },
        new() { 21000, -12000, 15500, -4000, 3500 }
    };
    public static List<List<decimal?>> LineDataSeries = new() {
        new() { 12500, 9500, 20500, 14500, 11000 },
        new() { 6500, -2500, null, 4000, 2000 }
    };
    public static List<List<decimal?>> AreaDataSeries = new() {
        new() { 7000, 12500, 8500, 3500, 11000 },
        new() { 2500, -2500, 3500, 5000, 3000 }
    };
}
