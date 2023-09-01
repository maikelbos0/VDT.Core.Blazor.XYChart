using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VDT.Core.Blazor.XYChart;

public abstract class ChildComponentBase : ComponentBase {
    [CascadingParameter] internal XYChart Chart { get; set; } = null!;

    public override async Task SetParametersAsync(ParameterView parameters) {
        var parametersHaveChanged = HaveParametersChanged(parameters);

        await base.SetParametersAsync(parameters);

        if (parametersHaveChanged) {
            Chart.HandleStateChange();
        }
    }
    
    public abstract bool HaveParametersChanged(ParameterView parameters);

    // TODO maybe differentiate between nullable and not nullable string
    public static bool HasParameterChanged(ParameterView parameters, string parameterName, string? oldValue)
        => parameters.TryGetValue(parameterName, out string? value) && !string.Equals(value, oldValue);

    public static bool HasParameterChanged<T>(ParameterView parameters, string parameterName, T oldValue) where T : struct
        => parameters.TryGetValue(parameterName, out T? value) && value != null && !value.Value.Equals(oldValue);

    public static bool HasParameterChanged(ParameterView parameters, string parameterName, Delegate oldValue)
        => parameters.TryGetValue(parameterName, out Delegate? value) && value != null && !value.Equals(oldValue);

    public static bool HasParameterChanged<T>(ParameterView parameters, string parameterName, IEnumerable<T?> oldValue) where T : struct
        => parameters.TryGetValue(parameterName, out IEnumerable<T?>? value) && value != null && !value.SequenceEqual(oldValue);
}
