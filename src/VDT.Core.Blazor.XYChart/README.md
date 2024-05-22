# VDT.Core.Blazor.XYChart

Blazor component to create SVG charts with a category X-axis and a value Y-axis, such as bar, line and area charts.

## Features

- Chart with one or multiple layers, either stacked or unstacked, of data series
- Line layers with configurable lines and data markers
- Bar layers with configurable bar width and gaps
- Area layers
- Possibility to extend with your own layer types
- Fully customizable canvas and legend
- Plot area with manual or automatic scaling
- CSS classes for customizable appearance of all chart elements

## Bar layer

In a bar layer the data is displayed as vertical bars which have a height corresponding to the data values. Bar layers offer the following customization 
parameters:

- `IsStacked` toggles stacking of data points; a stacked bar layer divides its bars into a positive stack of bars displayed above the 0 line and a negative
  stack of bars below the 0 line, so that positive and negative values are always visible without overlap
- `ShowDataLabels` toggles visibility of labels with the value at each data point
- `ClearancePercentage` is the amount of white space on either side of the group of bars for an index, expressed as a percentage of the total amount of space
  available for this index
- `GapPercentage` is the amount of white space between each bar for an index, expressed as a percentage of the total amount of space available for this index;
  this value is ignored when the bar layer is stacked

### Example

```
<XYChart Labels="@(new List<string>() { "Foo", "Bar", "Baz", "Qux", "Quux", "Corge", "Grault", "Garply" })">
    <BarLayer IsStacked="false" ShowDataLabels="true" ClearancePercentage="25" GapPercentage="-10">
        <DataSeries Name="Bar" Color="#ffcc11" DataPoints="@(new List<decimal?> { 110500, 190000, 315000, -25000, 95000, 45000, 35000, 40000 })" />
        <DataSeries Name="Baz" Color="#22cc55" DataPoints="@(new List<decimal?> { 210000, -120500, 155000, -40000, 35000, 25000, -5000, 30000 })" />
    </BarLayer>
</XYChart>
```

## Line layer

In a line layer the data is displayed as a series of points at positions corresponding to the data values, connected by lines. Line layers offer the following
customization parameters:

- `IsStacked` toggles stacking of data points; a stacked line layer stacks positive and negative values together
- `ShowDataLabels` toggles visibility of labels with the value at each data point
- `ShowDataMarkers` toggles visibility of the markers at the positions of the data points
- `DataMarkerSize` is the size of the data markers
- `DataMarkerType` is the type of data marker to use
- `DataLineMode` is the visibility and type of lines connecting the positions of data points
- `ControlPointDistancePercentage` is the distance between data points and their control points for smooth lines, expressed as a percentage of the total amount of space
  available for this index

### Example

```
<XYChart Labels="@(new List<string>() { "Foo", "Bar", "Baz", "Qux", "Quux", "Corge", "Grault", "Garply" })">
    <LineLayer IsStacked="true" ShowDataLabels="true" ShowDataMarkers="true" DataMarkerSize="16" DataMarkerType="@DefaultDataMarkerTypes.Square" DataLineMode="DataLineMode.Smooth">
        <DataSeries Name="Line" Color="#3366bb" DataPoints="@(new List<decimal?> { 125000, 95000, 205000, 145000, 110000, 155000, 135500, 140000 })" />
        <DataSeries Name="Linz" Color="#dd3377" DataPoints="@(new List<decimal?> { 65000, 55000, null, 81000, 52500, null, 58000, 70500 })" />
    </LineLayer>
</XYChart>
```

## Area layer

In an area layer the data is displayed as filled shapes connecting the points at positions corresponding to the data values. Area layers offer the following
customization parameters:

- `IsStacked` toggles stacking of data points; a stacked area layer stacks positive and negative values together
- `ShowDataLabels` toggles visibility of labels with the value at each data point

### Example

```
<XYChart Labels="@(new List<string>() { "Foo", "Bar", "Baz", "Qux", "Quux", "Corge", "Grault", "Garply" })">
    <AreaLayer IsStacked="true" ShowDataLabels="true">
        <DataSeries Name="Area" Color="#ff9933" DataPoints="@(new List<decimal?> { 70000, 125000, 85000, 35000, 110000, 80000, 85000, 60000 })" />
        <DataSeries Name="Arez" Color="#aa66ee" DataPoints="@(new List<decimal?> { 25500, -25000, 35000, 50500, 30000, null, null, 45000 })" />
    </AreaLayer>
</XYChart>
```

## Canvas
   
The canvas component is the place where all general chart dimensions and number format strings can be set. All dimension settings are in pixels. Number format
strings can be standard or custom numeric format strings.

- `Width` is the total width of the chart, including axis labels and padding
- `Height` is the total height of the chart, including axis labels and padding
- `Padding` is the distance between the canvas edge and any chart elements
- `AutoSizeXAxisLabelsIsEnabled` toggles the automatic calculation of x-axis label height
- `XAxisLabelHeight` is the vertical room reserved for labels on the x-axis
- `AutoSizeYAxisLabelsIsEnabled` toggles the automatic calculation of y-axis label width
- `YAxisLabelWidth` is the horizontal room reserved for labels on the y-axis, including the multiplier if applicable
- `YAxisLabelFormat` is the format string for the numeric labels on the y-axis
- `YAxisMultiplierFormat` is the format string for the y-axis multiplier, if it's visible
- `DataLabelFormat` is the format string for the numeric data labels optionally displayed at each data point

### Example

```
<XYChart Labels="@(new List<string>() { "Foo", "Bar", "Baz", "Qux", "Quux", "Corge", "Grault", "Garply" })">
    <Canvas Width="900" Height="400" Padding="20" XAxisLabelHeight="40" YAxisLabelWidth="70"
            YAxisLabelFormat="#,##0.00" YAxisMultiplierFormat="x #,##0.00" DataLabelFormat="#,##0.00" />

    <PlotArea Multiplier="1000" />

    <BarLayer ShowDataLabels="true">
        <DataSeries Name="Bar" Color="#ffcc11" DataPoints="@(new List<decimal?> { 110500, 190000, 315000, -25000, 95000, 45000, 35000, 40000 })" />
        <DataSeries Name="Baz" Color="#22cc55" DataPoints="@(new List<decimal?> { 210000, -120500, 155000, -40000, 35000, 25000, -5000, 30000 })" />
    </BarLayer>
</XYChart>
```

## Plot area

The plot area component allows you to set the range of the plot area, as well as the multiplier and the interval used to determine where to place grid
lines. It also allows you to set up parameters for automatic scaling.

- `Min` is the lowest data point value that is visible in the chart
- `Max` is the highest data point value that is visible in the chart
- `GridLineInterval` is the interval with which grid lines are shown; the starting point is zero
- `Multiplier` is the unit multiplier when showing y-axis labels and data labels; all values get divided by this value before being displayed
- `AutoScaleIsEnabled` enables automatic scaling of the plot area minimum, maximum and grid line interval; if enabled the values for `Min`, `Max` and
  `GridLineInterval` will be ignored
- `AutoScaleRequestedGridLineCount` is the number of grid lines you would ideally like to see if automatic scaling is enabled; please note that the end result
  can differ from the requested count
- `AutoScaleIncludesZero` toggles the forced inclusion of the zero line in the plot area when automatically scaling
- `AutoScaleClearancePercentage` is the minimum clearance between the highest/lowest data point and the edge of the plot area, expressed as a percentage of the
  total plot area range

### Examples

```
<XYChart Labels="@(new List<string>() { "Foo", "Bar", "Baz", "Qux", "Quux", "Corge", "Grault", "Garply" })">
    <PlotArea Min="-200000" Max="500000" GridLineInterval="100000" Multiplier="1000" AutoScaleIsEnabled="false" />

    <BarLayer>
        <DataSeries Name="Bar" Color="#ffcc11" DataPoints="@(new List<decimal?> { 110500, 190000, 315000, -25000, 95000, 45000, 35000, 40000 })" />
        <DataSeries Name="Baz" Color="#22cc55" DataPoints="@(new List<decimal?> { 210000, -120500, 155000, -40000, 35000, 25000, -5000, 30000 })" />
    </BarLayer>
</XYChart>

<XYChart Labels="@(new List<string>() { "Foo", "Bar", "Baz", "Qux", "Quux", "Corge", "Grault", "Garply" })">
    <PlotArea AutoScaleIsEnabled="true" AutoScaleRequestedGridLineCount="20" AutoScaleIncludesZero="true" AutoScaleClearancePercentage="20" />

    <BarLayer>
        <DataSeries Name="Bar" Color="#ffcc11" DataPoints="@(new List<decimal?> { 110500, 190000, 315000, -25000, 95000, 45000, 35000, 40000 })" />
        <DataSeries Name="Baz" Color="#22cc55" DataPoints="@(new List<decimal?> { 210000, -120500, 155000, -40000, 35000, 25000, -5000, 30000 })" />
    </BarLayer>
</XYChart>
```

## Legend

The &lt;Legend&gt; component allows you to configure the positioning and dimensions of the optional chart legend. All dimension settings are in pixels.

- `IsEnabled` toggles whether or not the legend is displayed
- `Position` sets the legend position:
    - `LegendPosition.Top` displays the legend above the plot area
    - `LegendPosition.Bottom` displays the legend below the plot area
- `Alignment` aligns the legend items inside the legend area left, right or centered
- `ItemWidth` is the total width reserved for each legend item (key and text) inside the legend
- `ItemHeight` is the total height reserved for each legend item
- `KeySize` is the width/height of the legend item key

Legend item keys are centered horizontally and vertically inside a partition of the legend item area, dimensions of this space are `ItemHeight` by
`ItemHeight`. Legend item text is centered vertically inside the legend item area, and are offset by the previously mentioned partition,  meaning they are
offset by `ItemHeight` horizontally.

### Example

```
<XYChart Labels="@(new List<string>() { "Foo", "Bar", "Baz", "Qux", "Quux", "Corge", "Grault", "Garply" })">
    <Legend IsEnabled="true" Position="@LegendPosition.Bottom" Alignment="@LegendAlignment.Right" Height="30" ItemWidth="90" ItemHeight="30" KeySize="20" />

    <BarLayer>
        <DataSeries Name="Bar" Color="#ffcc11" DataPoints="@(new List<decimal?> { 110500, 190000, 315000, -25000, 95000, 45000, 35000, 40000 })" />
        <DataSeries Name="Baz" Color="#22cc55" DataPoints="@(new List<decimal?> { 210000, -120500, 155000, -40000, 35000, 25000, -5000, 30000 })" />
    </BarLayer>
</XYChart>
```

## Style

Since the chart is rendered as SVG, each chart element is fully customizable using the powerful SVG options in CSS. Each element type can be referenced using
the below CSS classes. The chart itself can be referenced with the CSS class `svg.chart-main`. Aside from the default CSS classes, it is also possible to
assign each data series its own custom CSS class using the `CssClass` parameter. All SVG elements that are associated with this data series will contain this
CSS class, including data labels and legend items.

- Plot area: `.plot-area`; please note that this shape is inverted so that it covers chart data elements that would otherwise extend into the rest of the
  canvas
- Grid lines: `.grid-line`
- Axes:
  - X-axis labels: `.x-axis-label`
  - Y-axis labels: `.y-axis-label`
  - Y-axis multiplier: `.y-axis-multiplier`
- Data: `.data`
  - Area data: `.data.area-data`
  - Bar data: `.data.bar-data`
  - Line data: `.data.line-data`
- Data markers for line data: `.data-marker`
  - Round data markers: `.data-marker.data-marker-round`
  - Square data markers: `.data-marker.data-marker-square`
- Data labels: `.data-label`
  - Positive values (including 0) also get the attribute `data-positive`
  - Negative values also get the attribute `data-negative`
- Legend item keys: `.legend-key`
- Legend item text: `.legend-text`

If you use Blazor CSS isolation on a child component, the `::deep` pseudo-element is used and the chart component must be inside an HTML element that the
isolation attribute will be applied to.

The chart also allows you to add SVG elements or other custom content as part of the `ChildContent`. This content is always rendered last, which means custom
content will be rendered top-most in the chart SVG.

The axis labels, data labels, y-axis multiplier and legend text only get positioned with a single x/y coordinate as an anchor, so horizontal and vertical
aligning, rotation and any other positional transforming should be done with CSS. In the below chart example the anchors of these elements are made visible
with red crosses.

Styles that are not universal, such as the color for a data series, the marker size for a line layer, or the gap width for a bar layer, can be set on the
various chart objects themselves. For more information, see the menu items for the various layer types.

## Defaults

It's possible to apply default values for parameters in almost all chart components by setting the static property corresponding to the parameter to the 
desired value. To ensure these values are set before any charts are rendered it is best to apply these values in your startup. In the example below, default
canvas and legend dimensions are set.

The only exception to this pattern is the `DataSeries` class; its parameters either have special rules or can not have a default applied.

- `DataSeries.Name` does not have a default, but can be null
- `DataSeries.Color` does not have a single default, but rather a list of defaults; if a data series doesn't have an assigned color it will be picked from this
  list by the index of the data series inside the entire chart, applying modulus if needed
- `DataSeries.DataPoints` does not have a default, but can be empty; in this case the empty values are either not shown or interpreted as zero depending on the
  layer type and settings
- `DataSeries.CssClass` does not have a default, but can be null; if you wish to apply styles across all data series elements you can use the built-in CSS
  classes

### Example

```
public class Program {
    public static async Task Main(string[] args) {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add&lt;App&gt;("#app");

        Canvas.DefaultWidth = 900;
        Canvas.DefaultHeight = 400;
        Legend.DefaultPosition = LegendPosition.Bottom;
        Legend.DefaultHeight = 50;

        await builder.Build().RunAsync();
    }
}
```
