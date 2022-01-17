﻿namespace BlazorPlayground.Calculator {
    internal class MultiplicationOperator : Operator {
        public override OperatorPrecedence Precedence => OperatorPrecedence.High;

        public override decimal Invoke(decimal left, decimal right) => left * right;
    }
}
