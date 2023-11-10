using System;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Mathematical methods for decimal values
/// </summary>
public static class DecimalMath {
    /// <summary>
    /// Returns a specified number raised to the specified power
    /// </summary>
    /// <param name="x">Value to be raised to a power</param>
    /// <param name="y">Power to raise the value to</param>
    /// <returns>The number <paramref name="x"/> raised to the power <paramref name="y"/></returns>
    public static decimal Pow(decimal x, int y) {
        var result = 1M;

        if (y < 0) {
            y *= -1;
            x = 1 / x;
        }

        while (y > 0) {
            if ((y & 1) != 0) {
                result *= x;
            }
            y >>= 1;
            x *= x;
        }

        return result;
    }

    /// <summary>
    /// Rounds a value down, using the scale provided such that <paramref name="value"/> modulus <paramref name="scale"/> will equal zero
    /// </summary>
    /// <param name="value">Value to round</param>
    /// <param name="scale">Scale to round to</param>
    /// <returns>The rounded down value</returns>
    public static decimal FloorToScale(decimal value, decimal scale)
        => Math.Floor(value / scale) * scale;

    /// <summary>
    /// Rounds a value up, using the scale provided such that <paramref name="value"/> modulus <paramref name="scale"/> will equal zero
    /// </summary>
    /// <param name="value">Value to round</param>
    /// <param name="scale">Scale to round to</param>
    /// <returns>The rounded up value</returns>
    public static decimal CeilingToScale(decimal value, decimal scale)
        => Math.Ceiling(value / scale) * scale;

    /// <summary>
    /// Trims unwanted trailing zeros from a decimal
    /// </summary>
    /// <param name="value">Value to trim</param>
    /// <returns>The trimmed value</returns>
    public static decimal Trim(decimal value)
        => value / 1.000_000_000_000_000_000_000_000_000_000M;

    /// <summary>
    /// Adjust a value to an inclusive minimum and maximum value
    /// </summary>
    /// <param name="value">Value to adjust</param>
    /// <param name="min">Inclusive minimum value to adjust to</param>
    /// <param name="max">Inclusive maximum value to adjust to</param>
    /// <returns>The adjusted value</returns>
    public static decimal AdjustToRange(decimal value, decimal min, decimal max)
        => Math.Min(Math.Max(value, min), max);
}