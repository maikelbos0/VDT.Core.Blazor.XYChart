using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Helper methods to determine if values have changed inside a <see cref="ParameterView"/>
/// </summary>
public static class ParameterViewExtensions {
    /// <summary>
    /// Checks if the value of the parameter has changed
    /// </summary>
    /// <param name="parameters">The parameter view</param>
    /// <param name="oldValue">Previous value of the parameter</param>
    /// <param name="parameterName">Name of the parameter</param>
    /// <returns><see langword="true"/> of the value has chanbed; otherwise <see langword="false"/></returns>
    public static bool HasParameterChanged(this ParameterView parameters, string? oldValue, [CallerArgumentExpression(nameof(oldValue))] string parameterName = "")
        => parameters.TryGetValue(parameterName, out string? value) && !string.Equals(value, oldValue);

    /// <summary>
    /// Checks if the value of the parameter has changed
    /// </summary>
    /// <param name="parameters">The parameter view</param>
    /// <param name="oldValue">Previous value of the parameter</param>
    /// <param name="parameterName">Name of the parameter</param>
    /// <returns><see langword="true"/> of the value has chanbed; otherwise <see langword="false"/></returns>
    public static bool HasParameterChanged<T>(this ParameterView parameters, T oldValue, [CallerArgumentExpression(nameof(oldValue))] string parameterName = "") where T : struct
        => parameters.TryGetValue(parameterName, out T? value) && !value.Equals(oldValue);

    /// <summary>
    /// Checks if the value of the parameter has changed
    /// </summary>
    /// <param name="parameters">The parameter view</param>
    /// <param name="oldValue">Previous value of the parameter</param>
    /// <param name="parameterName">Name of the parameter</param>
    /// <returns><see langword="true"/> of the value has chanbed; otherwise <see langword="false"/></returns>
    public static bool HasParameterChanged(this ParameterView parameters, Delegate oldValue, [CallerArgumentExpression(nameof(oldValue))] string parameterName = "")
        => parameters.TryGetValue(parameterName, out Delegate? value) && (value == null || !value.Equals(oldValue));

    /// <summary>
    /// Checks if the value of the parameter has changed
    /// </summary>
    /// <param name="parameters">The parameter view</param>
    /// <param name="oldValue">Previous value of the parameter</param>
    /// <param name="parameterName">Name of the parameter</param>
    /// <returns><see langword="true"/> of the value has chanbed; otherwise <see langword="false"/></returns>
    public static bool HasParameterChanged<T>(this ParameterView parameters, IEnumerable<T?> oldValue, [CallerArgumentExpression(nameof(oldValue))] string parameterName = "") where T : struct
        => parameters.TryGetValue(parameterName, out IEnumerable<T?>? value) && (value == null || !value.SequenceEqual(oldValue));

    /// <summary>
    /// Checks if the value of the parameter has changed
    /// </summary>
    /// <param name="parameters">The parameter view</param>
    /// <param name="oldValue">Previous value of the parameter</param>
    /// <param name="parameterName">Name of the parameter</param>
    /// <returns><see langword="true"/> of the value has chanbed; otherwise <see langword="false"/></returns>
    public static bool HasParameterChanged(this ParameterView parameters, IEnumerable<string> oldValue, [CallerArgumentExpression(nameof(oldValue))] string parameterName = "")
        => parameters.TryGetValue(parameterName, out IEnumerable<string>? value) && (value == null || !value.SequenceEqual(oldValue));
}
