﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorPlayground.Chart;

public class Canvas2 : ComponentBase {
    [CascadingParameter] internal XYChart2 Chart { get; set; } = null!;

    protected override void OnInitialized() {
        Chart.Canvas = this;
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder) => builder.AddContent(1, nameof(Canvas2));
}
