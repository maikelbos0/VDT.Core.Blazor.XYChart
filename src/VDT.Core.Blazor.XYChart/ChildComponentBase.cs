using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace VDT.Core.Blazor.XYChart;

/// <summary>
/// Base class for child components in an <see cref="XYChart"/>
/// </summary>
public abstract class ChildComponentBase : ComponentBase {
    [CascadingParameter] internal XYChart Chart { get; set; } = null!;

    /// <inheritdoc/>
    public override async Task SetParametersAsync(ParameterView parameters) {
        var parametersHaveChanged = HaveParametersChanged(parameters);

        await base.SetParametersAsync(parameters);

        if (parametersHaveChanged) {
            Chart.StateHasChanged();
        }
    }
    
    /// <summary>
    /// Determines whether or not any parameters have changed and the containing chart has to be re-rendered
    /// </summary>
    /// <param name="parameters">New parameters</param>
    /// <returns><see langword="true"/> of any parameter has changed; otherwise <see langword="false"/></returns>
    public abstract bool HaveParametersChanged(ParameterView parameters);
}
