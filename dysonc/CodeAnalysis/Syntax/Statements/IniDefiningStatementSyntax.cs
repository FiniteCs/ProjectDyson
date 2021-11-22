using Dyson.CodeAnalysis.Syntax.Expressions;

namespace Dyson.CodeAnalysis.Syntax.Statements
{
    internal sealed class IniDefiningStatementSyntax
        : StatementSyntax
    {
        public IniDefiningStatementSyntax(SyntaxToken iniKeyword, ExpressionSyntax equalsClause)
        {
            IniKeyword = iniKeyword;
            EqualsClause = equalsClause;
        }

        public override SyntaxKind Kind => SyntaxKind.IniDefiningStatement;

        public SyntaxToken IniKeyword { get; }
        public ExpressionSyntax EqualsClause { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IniKeyword;
            yield return EqualsClause;
        }
    }
}
