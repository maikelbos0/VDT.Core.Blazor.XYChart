namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Determines the way data points are spaced out over the plot area
/// </summary>
public enum DataPointSpacingMode {
    /// <summary>
    /// Spacing is automatically determined based on the layers in the chart; the chart will use <see cref="EdgeToEdge"/> if possible but <see cref="Center"/>
    /// if needed
    /// </summary>
    /// <remarks>This value should not be used when providing the spacing mode for a custom layer</remarks>
    Auto,

    /// <summary>
    /// Data points are spaced out all the way to the plot area edge
    /// </summary>
    EdgeToEdge,

    /// <summary>
    /// Data points are displayed as blocks and data point values are displayed in the center of those blocks as appropriate
    /// </summary>
    Center
}
