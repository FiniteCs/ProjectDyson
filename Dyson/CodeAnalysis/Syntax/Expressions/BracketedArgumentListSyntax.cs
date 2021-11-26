namespace Dyson.CodeAnalysis.Syntax.Expressions
{
    internal sealed class BracketedArgumentListSyntax
        : ExpressionSyntax
    {
        public BracketedArgumentListSyntax(SyntaxToken openBracketToken, 
                                           ExpressionSyntax indexerExpression, 
                                           SyntaxToken closeBracketToken)
        {
            OpenBracketToken = openBracketToken;
            IndexerExpression = indexerExpression;
            CloseBracketToken = closeBracketToken;
        }

        public override SyntaxKind Kind => SyntaxKind.BracketedArgumentList;

        public SyntaxToken OpenBracketToken { get; }
        public ExpressionSyntax IndexerExpression { get; }
        public SyntaxToken CloseBracketToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenBracketToken;
            yield return IndexerExpression;
            yield return CloseBracketToken;
        }
    }
}
