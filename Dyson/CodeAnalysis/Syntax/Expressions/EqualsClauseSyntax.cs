namespace Dyson.CodeAnalysis.Syntax.Expressions
{
    public sealed class EqualsClauseSyntax
        : ExpressionSyntax
    {
        public EqualsClauseSyntax(SyntaxToken equalsToken, ExpressionSyntax expression)
        {
            EqualsToken = equalsToken;
            Expression = expression;
        }

        public override SyntaxKind Kind => SyntaxKind.EqualsClause;

        public SyntaxToken EqualsToken { get; }
        public ExpressionSyntax Expression { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return EqualsToken;
            yield return Expression;
        }
    }
}
