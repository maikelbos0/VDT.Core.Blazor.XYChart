using System;
using System.Linq;

namespace VDT.Core.Blazor.XYChart.Tests;

public static class Helpers {
    public static string Path(params FormattableString[] commands) => string.Join(" ", commands.Select(FormattableString.Invariant));
}
