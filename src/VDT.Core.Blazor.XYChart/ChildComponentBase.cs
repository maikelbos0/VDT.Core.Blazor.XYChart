using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

    public bool HasParameterChanged<T>(ParameterView parameters, string parameterName, T oldValue) {
        return parameters.TryGetValue(parameterName, out T? value) && !EqualityComparer<T>.Default.Equals(value, oldValue);
    }
}
