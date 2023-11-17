namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// Base class for SVG shape elements
/// </summary>
public abstract class ShapeBase {
    /// <summary>
    /// Gets the CSS class attribute for this shape
    /// </summary>
    public abstract string CssClass { get; }

    /// <summary>
    /// Gets the SVG element name
    /// </summary>
    public abstract string ElementName { get; }

    /// <summary>
    /// Gets the unique key value for this shape element in the chart component list
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Creates an SVG shape
    /// </summary>
    /// <param name="indexes">Indexes that, together with the shape type, will form the unique key value</param>
    public ShapeBase(params int[] indexes) {
        Key = $"{GetType().Name}[{string.Join(",", indexes)}]";
    }

    /// <summary>
    /// Gets the additional attributes needed to render this SVG shape element
    /// </summary>
    /// <returns>A collection of key-value pairs representing attributes</returns>
    public abstract ShapeAttributeCollection GetAttributes();

    /// <summary>
    /// Gets the text content for this SVG element if applicable
    /// </summary>
    /// <returns>The text content</returns>
    public virtual string? GetContent() => null;
}
