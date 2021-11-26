namespace Dyson.CodeAnalysis.Syntax.Expressions
{
    internal sealed class IniKeyIndexingExpressionSyntax
        : ExpressionSyntax
    {
        public IniKeyIndexingExpressionSyntax(SyntaxToken iniKeyword, ExpressionSyntax bracketedArgumentList)
        {
            IniKeyword = iniKeyword;
            BracketedArgumentList = bracketedArgumentList;
        }

        public override SyntaxKind Kind => SyntaxKind.IniKeyIndexingExpression;

        public SyntaxToken IniKeyword { get; }
        public ExpressionSyntax BracketedArgumentList { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IniKeyword;
            yield return BracketedArgumentList;
        }
    }
}
