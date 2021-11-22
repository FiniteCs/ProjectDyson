namespace Dyson.CodeAnalysis.Syntax.Expressions
{
    internal sealed class UnaryExpressionSyntax
        : ExpressionSyntax
    {
        public UnaryExpressionSyntax(SyntaxToken unaryOperator, ExpressionSyntax operand)
        {
            UnaryOperator = unaryOperator;
            Operand = operand;
        }

        public override SyntaxKind Kind => SyntaxKind.UnaryExpression;
        public SyntaxToken UnaryOperator { get; }
        public ExpressionSyntax Operand { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return UnaryOperator;
            yield return Operand;
        }
    }
}
