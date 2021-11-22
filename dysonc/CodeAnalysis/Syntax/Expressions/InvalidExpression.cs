namespace Dyson.CodeAnalysis.Syntax.Expressions
{
    internal sealed class InvalidExpression
        : ExpressionSyntax
    {
        public InvalidExpression(SyntaxKind expressionKind)
        {
            Kind = expressionKind;
        }

        public override SyntaxKind Kind { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}
