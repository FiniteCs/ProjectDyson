namespace Dyson.CodeAnalysis.Syntax.Expressions
{
    internal class MemberAccessExpressionSyntax
        : ExpressionSyntax
    {
        public MemberAccessExpressionSyntax(ExpressionSyntax expression, 
                                            SyntaxToken dotToken, 
                                            NameExpressionSyntax nameExpression,
                                            List<MemberAccessExpressionSyntax> memberAccesses)
        {
            Expression = expression;
            DotToken = dotToken;
            NameExpression = nameExpression;
            MemberAccesses = memberAccesses;
        }

        public override SyntaxKind Kind => SyntaxKind.MemberAccessExpression;

        public ExpressionSyntax Expression { get; }
        public SyntaxToken DotToken { get; }
        public NameExpressionSyntax NameExpression { get; }
        public List<MemberAccessExpressionSyntax> MemberAccesses { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Expression;
            yield return DotToken;
            yield return NameExpression;
            foreach (var member in MemberAccesses)
                yield return member;
        }
    }
}
