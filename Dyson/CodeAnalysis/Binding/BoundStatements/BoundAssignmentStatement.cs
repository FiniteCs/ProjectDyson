using Dyson.CodeAnalysis.Binding.BoundExpressions;

namespace Dyson.CodeAnalysis.Binding.BoundStatements
{
    internal sealed class BoundAssignmentStatement
        : BoundStatement
    {
        public BoundAssignmentStatement(VariableSymbol variable, BoundExpression expression)
        {
            Variable = variable;
            Expression = expression;
        }

        public override Type Type => Variable.Type;
        public override BoundNodeKind Kind => BoundNodeKind.VariableAssignment;

        public VariableSymbol Variable { get; }
        public BoundExpression Expression { get; }
    }
}
