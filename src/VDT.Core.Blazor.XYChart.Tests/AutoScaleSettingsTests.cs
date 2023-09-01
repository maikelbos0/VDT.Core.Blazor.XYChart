using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class AutoScaleSettingsTests {
    [Theory]
    [MemberData(nameof(HaveParametersChanged_Data))]
    public void HaveParametersChanged(bool isEnabled, int requestedGridLineCount, bool includeZero, decimal clearancePercentage, bool expectedResult) {
        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(AutoScaleSettings.IsEnabled), isEnabled },
            { nameof(AutoScaleSettings.RequestedGridLineCount), requestedGridLineCount },
            { nameof(AutoScaleSettings.IncludeZero), includeZero },
            { nameof(AutoScaleSettings.ClearancePercentage), clearancePercentage }
        });

        var subject = new AutoScaleSettings {
            IsEnabled = true,
            RequestedGridLineCount = 10,
            IncludeZero = true,
            ClearancePercentage = 10M
        };

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<bool, int, bool, decimal, bool> HaveParametersChanged_Data() => new() {
        { true, 10, true, 10M, false },
        { false, 10, true, 10M, true },
        { true, 15, true, 10M, true },
        { true, 10, false, 10M, true },
        { true, 10, true, 5M, true },
    };
}
