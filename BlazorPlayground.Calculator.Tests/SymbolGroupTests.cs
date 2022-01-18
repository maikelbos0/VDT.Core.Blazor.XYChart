﻿using System.Linq;
using Xunit;

namespace BlazorPlayground.Calculator.Tests {
    public class SymbolGroupTests {
        [Fact]
        public void SymbolGroup_Can_Append_EvaluatableSymbol_When_Empty() {
            var group = new SymbolGroup();
            var LiteralNumber = new LiteralNumber(0);

            Assert.True(group.Append(LiteralNumber));
            Assert.Equal(LiteralNumber, Assert.Single(group.Symbols));
        }

        [Fact]
        public void SymbolGroup_Can_Not_Append_Operator_When_Empty() {
            var group = new SymbolGroup();
            var op = new AdditionOperator('+');

            Assert.False(group.Append(op));
            Assert.Empty(group.Symbols);
        }

        [Fact]
        public void SymbolGroup_Can_Append_EvaluatableSymbol_When_Last_Symbol_Is_Operator() {
            var group = new SymbolGroup();
            var LiteralNumber = new LiteralNumber(0);

            group.Symbols.Add(new LiteralNumber(1));
            group.Symbols.Add(new AdditionOperator('+'));

            Assert.True(group.Append(LiteralNumber));
            Assert.Equal(3, group.Symbols.Count);
            Assert.Equal(LiteralNumber, group.Symbols.Last());
        }

        [Fact]
        public void SymbolGroup_Can_Not_Append_Operator_When_Last_Symbol_Is_Operator() {
            var group = new SymbolGroup();
            var op = new AdditionOperator('+');

            group.Symbols.Add(new LiteralNumber(1));
            group.Symbols.Add(new AdditionOperator('+'));

            Assert.False(group.Append(op));
            Assert.Equal(2, group.Symbols.Count);
        }

        [Fact]
        public void SymbolGroup_Can_Append_Operator_When_Last_Symbol_Is_EvaluatableSymbol() {
            var group = new SymbolGroup();
            var op = new AdditionOperator('+');

            group.Symbols.Add(new LiteralNumber(1));

            Assert.True(group.Append(op));
            Assert.Equal(2, group.Symbols.Count);
            Assert.Equal(op, group.Symbols.Last());
        }

        [Fact]
        public void SymbolGroup_Can_Not_Append_EvaluatableSymbol_When_Last_Symbol_Is_EvaluatableSymbol() {
            var group = new SymbolGroup();
            var LiteralNumber = new LiteralNumber(1);

            group.Symbols.Add(new LiteralNumber(1));

            Assert.False(group.Append(LiteralNumber));
            Assert.Single(group.Symbols);
        }

        [Fact]
        public void SymbolGroup_Close_Removes_Trailing_Operators() {
            var group = new SymbolGroup();

            group.Symbols.Add(new LiteralNumber(1));
            group.Symbols.Add(new AdditionOperator('+'));
            group.Symbols.Add(new LiteralNumber(1));
            group.Symbols.Add(new AdditionOperator('+'));
            group.Symbols.Add(new AdditionOperator('+'));

            group.Close();

            Assert.Equal(3, group.Symbols.Count);
        }

        [Fact]
        public void SymbolGroup_Close_Adds_Zero_When_Empty() {
            var group = new SymbolGroup();

            group.Close();

            Assert.Equal(0, Assert.IsType<LiteralNumber>(Assert.Single(group.Symbols)).Evaluate());
        }

        [Fact]
        public void SymbolGroup_Can_Evaluate_Empty_Group() {
            var group = new SymbolGroup();

            Assert.Equal(0M, group.Evaluate());
        }

        [Fact]
        public void SymbolGroup_Can_Evaluate_Simple_Expression() {
            var group = new SymbolGroup();

            group.Symbols.Add(new LiteralNumber(2.8M));
            group.Symbols.Add(new DivisionOperator('/'));
            group.Symbols.Add(new LiteralNumber(1.6M));

            Assert.Equal(1.75M, group.Evaluate());
        }

        [Fact]
        public void SymbolGroup_Can_Evaluate_Expression_With_Multiple_Same_Precedence_Operators() {
            var group = new SymbolGroup();

            group.Symbols.Add(new LiteralNumber(2.8M));
            group.Symbols.Add(new DivisionOperator('/'));
            group.Symbols.Add(new LiteralNumber(1.6M));
            group.Symbols.Add(new MultiplicationOperator('*'));
            group.Symbols.Add(new LiteralNumber(3.2M));

            Assert.Equal(5.6M, group.Evaluate());
        }

        [Fact]
        public void SymbolGroup_Can_Evaluate_Expression_With_Multiple_Different_Precedence_Operators() {
            var group = new SymbolGroup();

            group.Symbols.Add(new LiteralNumber(2.8M));
            group.Symbols.Add(new AdditionOperator('+'));
            group.Symbols.Add(new LiteralNumber(1.2M));
            group.Symbols.Add(new MultiplicationOperator('*'));
            group.Symbols.Add(new LiteralNumber(3.2M));
            group.Symbols.Add(new AdditionOperator('+'));
            group.Symbols.Add(new LiteralNumber(2.8M));

            Assert.Equal(9.44M, group.Evaluate());
        }
    }
}
