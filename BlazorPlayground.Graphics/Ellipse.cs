﻿namespace BlazorPlayground.Graphics {
    public class Ellipse : DrawableShape {

        private readonly static Anchor[] anchors = new[] {
            new Anchor<Ellipse>(s => s.CenterPoint, (s, p) => s.CenterPoint = p),
            new Anchor<Ellipse>(s => s.RadiusPoint, (s, p) => s.RadiusPoint = p)
        };

        public override string ElementName => "ellipse";
        public override IReadOnlyList<Anchor> Anchors { get; } = Array.AsReadOnly(anchors);
        public Point CenterPoint { get; set; }
        public Point RadiusPoint { get; set; }

        private Ellipse() : this(new Point(0, 0), new Point(0, 0)) { }

        public Ellipse(Point centerPoint, Point radiusPoint) {
            CenterPoint = centerPoint;
            RadiusPoint = radiusPoint;
        }

        public override ShapeAttributeCollection GetAttributes() => new() {
            { "cx", CenterPoint.X },
            { "cy", CenterPoint.Y },
            { "rx", Math.Abs(RadiusPoint.X - CenterPoint.X) },
            { "ry", Math.Abs(RadiusPoint.Y - CenterPoint.Y) }
        };

        protected override Shape CreateClone() => new Ellipse(CenterPoint, RadiusPoint);
    }
}
