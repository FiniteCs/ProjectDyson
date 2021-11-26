namespace Dyson.CodeAnalysis.Syntax.Expressions
{
    public sealed class InvalidExpression
        : ExpressionSyntax
    {
        public InvalidExpression()
        {

        }

        public override SyntaxKind Kind => SyntaxKind.InvalidExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}
