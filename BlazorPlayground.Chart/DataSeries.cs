﻿namespace BlazorPlayground.Chart;

public class DataSeries : List<double?> {
    public string Name { get; set; }
    public string Color { get; set; }

    public DataSeries(string name, string color) {
        Name = name;
        Color = color;
    }
}
