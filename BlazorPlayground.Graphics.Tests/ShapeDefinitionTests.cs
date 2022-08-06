﻿using System;
using Xunit;

namespace BlazorPlayground.Graphics.Tests {
    public class ShapeDefinitionTests {
        [Theory]
        [InlineData(typeof(Line), "Line")]
        [InlineData(typeof(Rectangle), "Rectangle")]
        [InlineData(typeof(Circle), "Circle")]
        [InlineData(typeof(Ellipse), "Ellipse")]
        [InlineData(typeof(RegularPolygon), "Regular polygon")]
        [InlineData(typeof(QuadraticBezier), "Quadratic bezier")]
        [InlineData(typeof(CubicBezier), "Cubic bezier")]
        public void Get(Type type, string expectedName) {
            var definition = ShapeDefinition.Get(type);

            Assert.Equal(expectedName, definition.Name);
        }

        [Fact]
        public void Get_By_Shape() {
            var shape = new Line(new Point(100, 200), new Point(150, 250));

            var definition = ShapeDefinition.Get(shape);

            Assert.Equal(ShapeDefinition.Get(typeof(Line)), definition);
        }

        [Fact]
        public void Line() {
            var definition = Assert.Single(ShapeDefinition.Values, d => d.Name == "Line");

            Assert.False(definition.UseFill);
            Assert.False(definition.UseStrokeLinejoin);
            Assert.True(definition.UseStrokeLinecap);
            Assert.False(definition.UseSides);
            Assert.False(definition.AutoSelect);

            var shape = Assert.IsType<Line>(definition.Construct(new Point(100, 200), new Point(150, 250)));

            PointAssert.Equal(new Point(100, 200), shape.StartPoint);
            PointAssert.Equal(new Point(150, 250), shape.EndPoint);
        }

        [Fact]
        public void Rectangle() {
            var definition = Assert.Single(ShapeDefinition.Values, d => d.Name == "Rectangle");

            Assert.True(definition.UseFill);
            Assert.True(definition.UseStrokeLinejoin);
            Assert.False(definition.UseStrokeLinecap);
            Assert.False(definition.UseSides);
            Assert.False(definition.AutoSelect);

            var shape = Assert.IsType<Rectangle>(definition.Construct(new Point(100, 200), new Point(150, 250)));

            PointAssert.Equal(new Point(100, 200), shape.StartPoint);
            PointAssert.Equal(new Point(150, 250), shape.EndPoint);
        }

        [Fact]
        public void Circle() {
            var definition = Assert.Single(ShapeDefinition.Values, d => d.Name == "Circle");

            Assert.True(definition.UseFill);
            Assert.False(definition.UseStrokeLinejoin);
            Assert.False(definition.UseStrokeLinecap);
            Assert.False(definition.UseSides);
            Assert.False(definition.AutoSelect);

            var shape = Assert.IsType<Circle>(definition.Construct(new Point(100, 200), new Point(150, 250)));

            PointAssert.Equal(new Point(100, 200), shape.CenterPoint);
            PointAssert.Equal(new Point(150, 250), shape.RadiusPoint);
        }

        [Fact]
        public void Ellipse() {
            var definition = Assert.Single(ShapeDefinition.Values, d => d.Name == "Ellipse");

            Assert.True(definition.UseFill);
            Assert.False(definition.UseStrokeLinejoin);
            Assert.False(definition.UseStrokeLinecap);
            Assert.False(definition.UseSides);
            Assert.False(definition.AutoSelect);

            var shape = Assert.IsType<Ellipse>(definition.Construct(new Point(100, 200), new Point(150, 250)));

            PointAssert.Equal(new Point(100, 200), shape.CenterPoint);
            PointAssert.Equal(new Point(150, 250), shape.RadiusPoint);
        }

        [Fact]
        public void RegularPolygon() {
            var definition = Assert.Single(ShapeDefinition.Values, d => d.Name == "Regular polygon");

            Assert.True(definition.UseFill);
            Assert.True(definition.UseStrokeLinejoin);
            Assert.False(definition.UseStrokeLinecap);
            Assert.True(definition.UseSides);
            Assert.False(definition.AutoSelect);

            var shape = Assert.IsType<RegularPolygon>(definition.Construct(new Point(100, 200), new Point(150, 250)));

            PointAssert.Equal(new Point(100, 200), shape.CenterPoint);
            PointAssert.Equal(new Point(150, 250), shape.RadiusPoint);
        }

        [Fact]
        public void QuadraticBezier() {
            var definition = Assert.Single(ShapeDefinition.Values, d => d.Name == "Quadratic bezier");

            Assert.True(definition.UseFill);
            Assert.False(definition.UseStrokeLinejoin);
            Assert.True(definition.UseStrokeLinecap);
            Assert.False(definition.UseSides);
            Assert.True(definition.AutoSelect);

            var shape = Assert.IsType<QuadraticBezier>(definition.Construct(new Point(100, 200), new Point(150, 250)));

            PointAssert.Equal(new Point(100, 200), shape.StartPoint);
            PointAssert.Equal(new Point(150, 250), shape.EndPoint);
        }

        [Fact]
        public void CubicBezier() {
            var definition = Assert.Single(ShapeDefinition.Values, d => d.Name == "Cubic bezier");

            Assert.True(definition.UseFill);
            Assert.False(definition.UseStrokeLinejoin);
            Assert.True(definition.UseStrokeLinecap);
            Assert.False(definition.UseSides);
            Assert.True(definition.AutoSelect);

            var shape = Assert.IsType<CubicBezier>(definition.Construct(new Point(100, 200), new Point(150, 250)));

            PointAssert.Equal(new Point(100, 200), shape.StartPoint);
            PointAssert.Equal(new Point(150, 250), shape.EndPoint);
        }
    }
}
