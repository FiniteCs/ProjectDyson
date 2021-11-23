namespace Dyson.CodeAnalysis.Binding.BoundExpressions
{
    internal sealed class BoundEqualsClause
        : BoundExpression
    {
        public BoundEqualsClause(BoundExpression expression)
        {
            Expression = expression;
        }

        public override Type Type => Expression.Type;
        public override BoundNodeKind Kind => BoundNodeKind.BoundEqualsClause;
        public BoundExpression Expression { get; }
    }
}
