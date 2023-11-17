namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Determines the way data points are stacked in a layer
/// </summary>
public enum StackMode {
    /// <summary>
    /// Positive and negative data points are stacked together
    /// </summary>
    Single,

    /// <summary>
    /// Positive data points are stacked separately from negative data points
    /// </summary>
    Split
}
