using Microsoft.AspNetCore.Components;
using System;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Legend settings for an <see cref="XYChart"/>
/// </summary>
public class Legend : ChildComponentBase, IDisposable {
    /// <summary>
    /// Gets or sets the default value for whether or not the legend is displayed
    /// </summary>
    public static bool DefaultIsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the default value for the legend position
    /// </summary>
    public static LegendPosition DefaultPosition { get; set; } = LegendPosition.Top;

    /// <summary>
    /// Gets or sets the default value for the horizontal alignment of the legend items inside the legend area
    /// </summary>
    public static LegendAlignment DefaultAlignment { get; set; } = LegendAlignment.Center;

    /// <summary>
    /// Gets or sets the default value for the total height reserved for the legend
    /// </summary>
    public static int DefaultHeight { get; set; } = 25;

    /// <summary>
    /// Gets or sets the default value for the total width reserved for each legend item (key and text) inside the legend
    /// </summary>
    public static int DefaultItemWidth { get; set; } = 100;

    /// <summary>
    /// Gets or sets the default value for the total height reserved for each legend item (key and text) inside the legend
    /// </summary>
    public static int DefaultItemHeight { get; set; } = 25;

    /// <summary>
    /// Gets or sets the default value for the width/height of the legend item key
    /// </summary>
    public static int DefaultKeySize { get; set; } = 16;

    /// <summary>
    /// Gets or sets whether or not the legend is displayed
    /// </summary>
    [Parameter] public bool IsEnabled { get; set; } = DefaultIsEnabled;

    /// <summary>
    /// Gets or sets the legend position
    /// </summary>
    [Parameter] public LegendPosition Position { get; set; } = DefaultPosition;

    /// <summary>
    /// Gets or sets the horizontal alignment of the legend items inside the legend area
    /// </summary>
    [Parameter] public LegendAlignment Alignment { get; set; } = DefaultAlignment;

    /// <summary>
    /// Gets or sets the total height reserved for the legend
    /// </summary>
    [Parameter] public int Height { get; set; } = DefaultHeight;

    /// <summary>
    /// Gets or sets the total width reserved for each legend item (key and text) inside the legend
    /// </summary>
    [Parameter] public int ItemWidth { get; set; } = DefaultItemWidth;

    /// <summary>
    /// Gets or sets the total height reserved for each legend item (key and text) inside the legend
    /// </summary>
    [Parameter] public int ItemHeight { get; set; } = DefaultItemHeight;

    /// <summary>
    /// Gets or sets the width/height of the legend item key
    /// </summary>
    [Parameter] public int KeySize { get; set; } = DefaultKeySize;

    /// <inheritdoc/>
    protected override void OnInitialized() => Chart.SetLegend(this);

    /// <inheritdoc/>
    public void Dispose() {
        Chart.ResetLegend();
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    public override bool HaveParametersChanged(ParameterView parameters)
        => parameters.HasParameterChanged(IsEnabled)
        || parameters.HasParameterChanged(Position)
        || parameters.HasParameterChanged(Alignment)
        || parameters.HasParameterChanged(Height)
        || parameters.HasParameterChanged(ItemWidth)
        || parameters.HasParameterChanged(ItemHeight)
        || parameters.HasParameterChanged(KeySize);
}
