namespace Dyson.CodeAnalysis.Binding.BoundExpressions
{
    internal sealed class BoundBracketedArgumentList
        : BoundExpression
    {
        public BoundBracketedArgumentList(BoundExpression boundExpression)
        {
            BoundExpression = boundExpression;
        }

        public override Type Type => BoundExpression.Type;

        public override BoundNodeKind Kind => BoundNodeKind.BracketedArgumentList;

        public BoundExpression BoundExpression { get; }
    }
}
