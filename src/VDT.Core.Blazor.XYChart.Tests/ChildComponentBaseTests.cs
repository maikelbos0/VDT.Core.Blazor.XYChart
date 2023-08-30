using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class ChildComponentBaseTests {
    private class TestComponent : ChildComponentBase {
        public bool? Value { get; set; }

        public override bool HaveParametersChanged(ParameterView parameters) {
            throw new System.NotImplementedException();
        }
    }

    [Theory]
    [InlineData(null, null, false)]
    [InlineData(false, false, false)]
    [InlineData(true, true, false)]
    [InlineData(null, false, true)]
    [InlineData(null, true, true)]
    [InlineData(false, null, true)]
    [InlineData(true, null, true)]
    [InlineData(true, false, true)]
    [InlineData(false, true, true)]
    public void HasParameterChanged(bool? oldValue, bool? newValue, bool expectedResult) {
        var subject = new TestComponent() {
            Value = oldValue
        };

        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(TestComponent.Value), newValue }
        });

        Assert.Equal(expectedResult, subject.HasParameterChanged(parameters, nameof(subject.Value), subject.Value));
    }

    [Fact]
    public void HasParameterChanged_NotPresentInParameters() {
        var subject = new TestComponent();

        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>());

        Assert.False(subject.HasParameterChanged(parameters, nameof(subject.Value), subject.Value));
    }
}
