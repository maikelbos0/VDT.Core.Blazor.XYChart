﻿using System.Xml.Linq;

namespace BlazorPlayground.Graphics {
    public class Canvas {
        private int width = 800;
        private int height = 800;
        private int gridSize = 50;

        public int Width {
            get => width;
            set => width = Math.Max(value, 1);
        }
        public int Height {
            get => height;
            set => height = Math.Max(value, 1);
        }
        public int GridSize {
            get => gridSize;
            set => gridSize = Math.Max(value, 1);
        }
        public bool ShowGrid { get; set; } = false;
        public bool SnapToGrid { get; set; } = false;
        public List<Shape> Shapes { get; set; } = new List<Shape>();
        public Point? StartPoint { get; set; }
        public Point? SnappedStartPoint => Snap(StartPoint);
        public Point? EndPoint { get; set; }
        public Point? SnappedEndPoint => Snap(EndPoint);
        public bool IsDragging => StartPoint != null && EndPoint != null; // TODO consider removing this if code analysis can't figure out this causes start- and endpoint to be not null
        public Point? Delta => StartPoint != null && EndPoint != null ? EndPoint - StartPoint : null;
        public DrawSettings DrawSettings { get; } = new DrawSettings();
        public ShapeDefinition CurrentShapeDefinition { get; set; } = ShapeDefinition.Values.First();
        public Shape? SelectedShape { get; set; }

        private Point? Snap(Point? point) {
            if (!SnapToGrid || point == null) {
                return point;
            }

            return point.SnapToGrid(GridSize);
        }

        // TODO switch to/from draw mode in canvas instead of returning it here
        public bool AddShape() {
            var shape = CreateShape();

            if (shape != null) {
                Shapes.Add(shape);

                if (CurrentShapeDefinition.AutoSelect) {
                    SelectedShape = shape;
                    return true;
                }
            }

            return false;
        }

        public Shape? CreateShape() {
            if (SnappedStartPoint == null || SnappedEndPoint == null) {
                return null;
            }

            var shape = CurrentShapeDefinition.Construct(SnappedStartPoint, SnappedEndPoint, DrawSettings);

            shape.Fill = DrawSettings.FillPaintManager.Server;
            shape.Stroke = DrawSettings.StrokePaintManager.Server;
            shape.StrokeWidth = DrawSettings.StrokeWidth;
            shape.StrokeLinecap = DrawSettings.StrokeLinecap;
            shape.StrokeLinejoin = DrawSettings.StrokeLinejoin;

            return shape;
        }

        public XElement ExportSvg() => new(
            "svg",
            new XAttribute("viewBox", $"0 0 {Width} {Height}"),
            new XAttribute("width", Width),
            new XAttribute("height", Height),
            Shapes.Select(s => s.CreateElement())
        );
    }
}
