namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Determines the visibility and type of lines connecting the positions of data points in a <see cref="LineLayer"/>
/// </summary>
public enum DataLineMode {
    /// <summary>
    /// Don't display data lines
    /// </summary>
    Hidden,

    /// <summary>
    /// Connect data points with straight lines
    /// </summary>
    Straight,

    /// <summary>
    /// Use a smoothly curved line to connect data points
    /// </summary>
    Smooth
}
