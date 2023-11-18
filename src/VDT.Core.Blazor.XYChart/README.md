# VDT.Core.Blazor.XYChart

Blazor component to create SVG charts with a category X-axis and a value Y-axis, such as bar, line and area charts.

## Features

- Chart with one or multiple layers, either stacked or unstacked, of data series
- Line layers with configurable lines and data markers
- Bar layers with configurable bar width and gaps
- Area layers
- Possibiliy to extend with your own layer types
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

TODO

## Line layer

In a line layer the data is displayed as a series of points at positions corresponding to the data values, connected by lines. Line layers offer the following
customization parameters:

- `IsStacked` toggles stacking of data points; a stacked line layer stacks positive and negative values together
- `ShowDataLabels` toggles visibility of labels with the value at each data point
- `ShowDataMarkers` toggles visibility of the markers at the positions of the data points
- `DataMarkerSize` is the size of the data markers
- `DataMarkerType` is the type of data marker to use
- `ShowDataLines` toggles visibility of the lines connecting the positions of the data points

### Example

TODO

## Area layer

In an area layer the data is displayed as filled shapes connecting the points at positions corresponding to the data values. Area layers offer the following
customization parameters:

- `IsStacked` toggles stacking of data points; a stacked area layer stacks positive and negative values together
- `ShowDataLabels` toggles visibility of labels with the value at each data point

### Example

TODO

## Chart options

TODO

### Example

TODO

## Canvas

TODO

### Example

TODO

## Plot area

TODO

### Example

TODO

## Legend

TODO

### Example

TODO

## Style

TODO

### Example

TODO

## Defaults

TODO

### Example

TODO
