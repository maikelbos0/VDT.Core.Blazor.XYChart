using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class ChildComponentBaseTests {
    private class TestComponent : ChildComponentBase {
        public int Struct { get; set; }
        public string? String { get; set; }

        public override bool HaveParametersChanged(ParameterView parameters) {
            throw new System.NotImplementedException();
        }
    }

    [Theory]
    [InlineData(1, 1, false)]
    [InlineData(1, null, false)]
    [InlineData(1, 2, true)]
    public void HasParameterChanged_Struct(int oldValue, int? newValue, bool expectedResult) {
        var subject = new TestComponent() {
            Struct = oldValue
        };

        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(TestComponent.Struct), newValue }
        });

        Assert.Equal(expectedResult, ChildComponentBase.HasParameterChanged(parameters, nameof(subject.Struct), subject.Struct));
    }

    [Fact]
    public void HasParameterChanged_Struct_NotPresentInParameters() {
        var subject = new TestComponent();

        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>());

        Assert.False(ChildComponentBase.HasParameterChanged(parameters, nameof(subject.Struct), subject.Struct));
    }

    [Theory]
    [InlineData(null, null, false)]
    [InlineData("foo", "foo", false)]
    [InlineData("foo", "bar", true)]
    [InlineData("foo", null, true)]
    [InlineData(null, "foo", true)]
    public void HasParameterChanged_String(string? oldValue, string? newValue, bool expectedResult) {
        var subject = new TestComponent() {
            String = oldValue
        };

        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(TestComponent.String), newValue }
        });

        Assert.Equal(expectedResult, ChildComponentBase.HasParameterChanged(parameters, nameof(subject.String), subject.String));
    }

    [Fact]
    public void HasParameterChanged_String_NotPresentInParameters() {
        var subject = new TestComponent();

        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>());

        Assert.False(ChildComponentBase.HasParameterChanged(parameters, nameof(subject.String), subject.String));
    }
}
