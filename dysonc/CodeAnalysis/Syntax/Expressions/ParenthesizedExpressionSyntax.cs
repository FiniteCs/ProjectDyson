namespace Dyson.CodeAnalysis.Syntax.Expressions
{
    internal sealed class ParenthesizedExpressionSyntax
        : ExpressionSyntax
    {
        public ParenthesizedExpressionSyntax(SyntaxToken openParen, ExpressionSyntax expression, SyntaxToken closeParen)
        {
            OpenParen = openParen;
            Expression = expression;
            CloseParen = closeParen;
        }

        public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;

        public SyntaxToken OpenParen { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken CloseParen { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParen;
            yield return Expression;
            yield return CloseParen;
        }
    }
}
