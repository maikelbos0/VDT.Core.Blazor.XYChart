using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class ChildComponentBaseTests {
    private class TestComponent : ChildComponentBase {
        public int Struct { get; set; }
        public string? String { get; set; }
        public List<int?> List { get; set; } = new();

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

    [Theory]
    [MemberData(nameof(HasParameterChanged_List_Data))]
    public void HasParameterChanged_List(List<int?> oldValue, List<int?>? newValue, bool expectedResult) {
        var subject = new TestComponent() {
            List = oldValue
        };

        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(TestComponent.String), newValue }
        });

        Assert.Equal(expectedResult, ChildComponentBase.HasParameterChanged(parameters, nameof(subject.List), subject.List));
    }

    public static TheoryData<List<int?>, List<int?>?, bool> HasParameterChanged_List_Data() => new() {
        { new List<int?>(), new List<int?>(), false },
        { new List<int?>() { 1, 2 }, new List<int?>() { 1, 2 }, false },
        { new List<int?>(), null, false },
        { new List<int?>() { 1, 2 }, new List<int?>() { 1, 3 }, false },
        { new List<int?>() { 1, 2, 3 }, new List<int?>() { 1, 2 }, false },
        { new List<int?>() { 1, 2 }, new List<int?>() { 1, 2, 3 }, false }
    };

    [Fact]
    public void HasParameterChanged_List_NotPresentInParameters() {
        var subject = new TestComponent();

        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>());

        Assert.False(ChildComponentBase.HasParameterChanged(parameters, nameof(subject.List), subject.List));
    }
}
