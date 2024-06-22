using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using Xunit;

namespace VDT.Core.Blazor.XYChart.Tests;

public class ParameterViewExtensionsTests {
    private class TestComponent : ChildComponentBase {
        public string? String { get; set; }
        public int Struct { get; set; }
        public DataMarkerDelegate Delegate { get; set; } = DefaultDataMarkerTypes.Square;
        public List<string> StringList { get; set; } = [];
        public List<int?> StructList { get; set; } = [];

        public override bool HaveParametersChanged(ParameterView parameters)
            => parameters.HasParameterChanged(Struct)
            || parameters.HasParameterChanged(String)
            || parameters.HasParameterChanged(StructList)
            || parameters.HasParameterChanged(Delegate)
            || parameters.HasParameterChanged(StringList);
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

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    [Theory]
    [InlineData(1, 1, false)]
    [InlineData(1, null, true)]
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
        { DefaultDataMarkerTypes.Square, null, true },
        { DefaultDataMarkerTypes.Square, DefaultDataMarkerTypes.Round, true }
    };

    [Theory]
    [MemberData(nameof(HasParameterChanged_StringList_Data))]
    public void HasParameterChanged_StringList(List<string> oldValue, List<string>? newValue, bool expectedResult) {
        var subject = new TestComponent() {
            StringList = oldValue
        };

        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(TestComponent.StringList), newValue }
        });

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<List<string>, List<string>?, bool> HasParameterChanged_StringList_Data() => new() {
        { new List<string>(), new List<string>(), false },
        { new List<string>() { "Foo", "Bar" }, new List<string>() { "Foo", "Bar" }, false },
        { new List<string>(), null, true },
        { new List<string>() { "Foo", "Bar" }, new List<string>() { "Foo", "Baz" }, true },
        { new List<string>() { "Foo", "Bar", "Baz" }, new List<string>() { "Foo", "Bar" }, true },
        { new List<string>() { "Foo", "Bar" }, new List<string>() { "Foo", "Bar", "Baz" }, true }
    };

    [Theory]
    [MemberData(nameof(HasParameterChanged_StructList_Data))]
    public void HasParameterChanged_StructList(List<int?> oldValue, List<int?>? newValue, bool expectedResult) {
        var subject = new TestComponent() {
            StructList = oldValue
        };

        var parameters = ParameterView.FromDictionary(new Dictionary<string, object?>() {
            { nameof(TestComponent.StructList), newValue }
        });

        Assert.Equal(expectedResult, subject.HaveParametersChanged(parameters));
    }

    public static TheoryData<List<int?>, List<int?>?, bool> HasParameterChanged_StructList_Data() => new() {
        { new List<int?>(), new List<int?>(), false },
        { new List<int?>() { 1, 2 }, new List<int?>() { 1, 2 }, false },
        { new List<int?>(), null, true },
        { new List<int?>() { 1, 2 }, new List<int?>() { 1, 3 }, true },
        { new List<int?>() { 1, 2, 3 }, new List<int?>() { 1, 2 }, true },
        { new List<int?>() { 1, 2 }, new List<int?>() { 1, 2, 3 }, true }
    };
}
