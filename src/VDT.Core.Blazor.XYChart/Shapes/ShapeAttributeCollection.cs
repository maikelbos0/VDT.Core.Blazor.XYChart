using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace VDT.Core.Blazor.XYChart.Shapes;

/// <summary>
/// Collection of key-value pairs representing attributes
/// </summary>
public class ShapeAttributeCollection : IEnumerable<KeyValuePair<string, object>> {
    private readonly Dictionary<string, object> attributes = new();

    /// <summary>
    /// Add an attribute
    /// </summary>
    /// <param name="key">Attribute name</param>
    /// <param name="value">Attribute value</param>
    public void Add(string key, decimal value) => attributes.Add(key, DecimalMath.Trim(value).ToString(CultureInfo.InvariantCulture));

    /// <summary>
    /// Add an attribute
    /// </summary>
    /// <param name="key">Attribute name</param>
    /// <param name="value">Attribute value</param>
    public void Add(string key, object value) => attributes.Add(key, value);

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => attributes.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
