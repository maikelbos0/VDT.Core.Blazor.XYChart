using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class ParameterViewExtensionsTests {
    private class TestComponent : ChildComponentBase {
        public int Struct { get; set; }
        public string? String { get; set; }
        public List<int?> List { get; set; } = new();
        public DataMarkerDelegate Delegate { get; set; } = DefaultDataMarkerTypes.Square;

        public override bool HaveParametersChanged(ParameterView parameters)
            => parameters.HasParameterChanged(Struct)
            || parameters.HasParameterChanged(String)
            || parameters.HasParameterChanged(List)
            || parameters.HasParameterChanged(Delegate);
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

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    [Theory]
    [MemberData(nameof(HasParameterChanged_Delegate_Data))]
    public void HasParameterChanged_Delegate(DataMarkerDelegate oldValue, DataMarkerDelegate? newValue, bool expectedResult) {
        var subject = new TestComponent() {
            Delegate = oldValue
        };

        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(TestComponent.Delegate), newValue }
        });

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<DataMarkerDelegate, DataMarkerDelegate?, bool> HasParameterChanged_Delegate_Data() => new() {
        { DefaultDataMarkerTypes.Square, DefaultDataMarkerTypes.Square, false },
        { DefaultDataMarkerTypes.Square, null, false },
        { DefaultDataMarkerTypes.Square, DefaultDataMarkerTypes.Round, true }
    };

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

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    [Theory]
    [MemberData(nameof(HasParameterChanged_List_Data))]
    public void HasParameterChanged_List(List<int?> oldValue, List<int?>? newValue, bool expectedResult) {
        var subject = new TestComponent() {
            List = oldValue
        };

        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(TestComponent.List), newValue }
        });

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<List<int?>, List<int?>?, bool> HasParameterChanged_List_Data() => new() {
        { new List<int?>(), new List<int?>(), false },
        { new List<int?>() { 1, 2 }, new List<int?>() { 1, 2 }, false },
        { new List<int?>(), null, false },
        { new List<int?>() { 1, 2 }, new List<int?>() { 1, 3 }, true },
        { new List<int?>() { 1, 2, 3 }, new List<int?>() { 1, 2 }, true },
        { new List<int?>() { 1, 2 }, new List<int?>() { 1, 2, 3 }, true }
    };
}
