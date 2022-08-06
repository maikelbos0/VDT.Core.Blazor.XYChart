﻿namespace BlazorPlayground.Graphics {
    public class ShapeDefinition {
        public delegate Shape Constructor(Point startPoint, Point endPoint);

        private readonly static Dictionary<Type, ShapeDefinition> definitions = new() {
            { typeof(Line), new("Line", (startPoint, endPoint) => new Line(startPoint, endPoint), useStrokeLinecap: true) },
            { typeof(Rectangle), new("Rectangle", (startPoint, endPoint) => new Rectangle(startPoint, endPoint), useFill: true, useStrokeLinejoin: true) },
            { typeof(Circle), new("Circle", (startPoint, endPoint) => new Circle(startPoint, endPoint), useFill: true) },
            { typeof(Ellipse), new("Ellipse", (startPoint, endPoint) => new Ellipse(startPoint, endPoint), useFill: true) },
            { typeof(RegularPolygon), new("Regular polygon", (startPoint, endPoint) => new RegularPolygon(startPoint, endPoint), useFill: true, useStrokeLinejoin: true, useSides: true) },
            { typeof(QuadraticBezier), new("Quadratic bezier", (startPoint, endPoint) => new QuadraticBezier(startPoint, endPoint), useFill: true, useStrokeLinecap: true, autoSelect: true) },
            { typeof(CubicBezier), new("Cubic bezier", (startPoint, endPoint) => new CubicBezier(startPoint, endPoint), useFill: true, useStrokeLinecap: true, autoSelect: true) }
        };

        public static IEnumerable<ShapeDefinition> Values => definitions.Values;

        public static ShapeDefinition Get(Shape shape) => Get(shape.GetType());

        public static ShapeDefinition Get(Type type) => definitions[type];

        public string Name { get; }
        public Constructor Construct { get; }
        public bool UseFill { get; }
        public bool UseStrokeLinecap { get; }
        public bool UseStrokeLinejoin { get; }
        public bool UseSides { get; }
        public bool AutoSelect { get; }

        private ShapeDefinition(string name, Constructor construct, bool useFill = false, bool useStrokeLinecap = false, bool useStrokeLinejoin = false, bool useSides = false, bool autoSelect = false) {
            Name = name;
            Construct = construct;
            UseFill = useFill;
            UseStrokeLinecap = useStrokeLinecap;
            UseStrokeLinejoin = useStrokeLinejoin;
            UseSides = useSides;
            AutoSelect = autoSelect;
        }
    }
}
