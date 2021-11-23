using Dyson.CodeAnalysis.Syntax.Expressions;

namespace Dyson.CodeAnalysis.Syntax.Statements
{
    public sealed class IniDefiningStatementSyntax
        : StatementSyntax
    {
        public IniDefiningStatementSyntax(SyntaxToken iniKeyword, EqualsClauseSyntax equalsClause)
        {
            IniKeyword = iniKeyword;
            EqualsClause = equalsClause;
        }

        public override SyntaxKind Kind => SyntaxKind.IniDefiningStatement;

        public SyntaxToken IniKeyword { get; }
        public EqualsClauseSyntax EqualsClause { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IniKeyword;
            yield return EqualsClause;
        }
    }
}
