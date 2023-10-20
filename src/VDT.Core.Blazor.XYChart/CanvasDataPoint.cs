﻿namespace VDT.Core.Blazor.XYChart;

public record class CanvasDataPoint(decimal X, decimal Y, decimal Height, decimal Width, int Index, decimal Value);

public record class LegendItem(string Color, string Text);
