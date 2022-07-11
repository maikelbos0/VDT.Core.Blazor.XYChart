﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace BlazorPlayground.Graphics {
    public class ShapeRenderer : ComponentBase {

        [Parameter]
        public Shape? Shape { get; set; }

        [Parameter]
        public bool IsVirtual { get; set; } = false;

        [Parameter]
        public bool IsSelected { get; set; } = false;

        [Parameter]
        public EventCallback<MouseEventArgs> OnMouseDown { get; set; }

        [Parameter]
        public EventCallback<MouseEventArgs> OnMouseUp { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder) {
            if (Shape != null) {
                builder.OpenElement(1, GetElementName(Shape.RenderType));
                builder.SetKey(Shape);
                builder.AddAttribute(2, "onmousedown", OnMouseDown);
                builder.AddAttribute(4, "onmouseup", OnMouseUp);
                builder.AddAttribute(6, "class", $"shape {(IsSelected ? "shape-selected" : IsVirtual ? "shape-virtual" : "")}");
                builder.AddAttribute(7, "points", string.Join(" ", Shape.GetPoints().Select(p => FormattableString.Invariant($"{p.X},{p.Y}"))));
                builder.CloseElement();

                if (IsSelected) {
                    foreach (var anchor in Shape.GetAnchors()) {
                        var point = anchor.Get();

                        if (point != null) {
                            builder.OpenElement(1, "rect");
                            builder.SetKey(point);
                            builder.AddAttribute(3, "class", "shape-point");
                            builder.AddAttribute(4, "x", point.X - 5);
                            builder.AddAttribute(5, "y", point.Y - 5);
                            builder.AddAttribute(6, "width", 10);
                            builder.AddAttribute(7, "height", 10);
                            builder.CloseElement();
                        }
                    }
                }
            }
        }

        public string GetElementName(ShapeRenderType renderType)
            => renderType switch {
                ShapeRenderType.Polyline => "polyline",
                ShapeRenderType.Polygon => "polygon",
                _ => throw new NotImplementedException($"No implementation found for {nameof(ShapeRenderType)} '{renderType}'")
            };
    }
}
