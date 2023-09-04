using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VDT.Core.Blazor.XYChart;

public static class ParameterViewExtensions {
    public static bool HasParameterChanged(this ParameterView parameters, string? oldValue, [CallerArgumentExpression(nameof(oldValue))] string parameterName = "")
        => parameters.TryGetValue(parameterName, out string? value) && !string.Equals(value, oldValue);

    public static bool HasParameterChanged<T>(this ParameterView parameters, T oldValue, [CallerArgumentExpression(nameof(oldValue))] string parameterName = "") where T : struct
        => parameters.TryGetValue(parameterName, out T? value) && !value.Equals(oldValue);

    public static bool HasParameterChanged(this ParameterView parameters, Delegate oldValue, [CallerArgumentExpression(nameof(oldValue))] string parameterName = "")
        => parameters.TryGetValue(parameterName, out Delegate? value) && (value == null || !value.Equals(oldValue));

    public static bool HasParameterChanged<T>(this ParameterView parameters, IEnumerable<T?> oldValue, [CallerArgumentExpression(nameof(oldValue))] string parameterName = "") where T : struct
        => parameters.TryGetValue(parameterName, out IEnumerable<T?>? value) && (value == null || !value.SequenceEqual(oldValue));

    public static bool HasParameterChanged(this ParameterView parameters, IEnumerable<string> oldValue, [CallerArgumentExpression(nameof(oldValue))] string parameterName = "")
        => parameters.TryGetValue(parameterName, out IEnumerable<string>? value) && (value == null || !value.SequenceEqual(oldValue));
}
