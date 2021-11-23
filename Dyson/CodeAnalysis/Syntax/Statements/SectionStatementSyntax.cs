using Dyson.CodeAnalysis.Syntax.Expressions;

namespace Dyson.CodeAnalysis.Syntax.Statements
{
    internal sealed class SectionStatementSyntax
        : StatementSyntax
    {
        public SectionStatementSyntax(SyntaxToken sectionKeyword, ExpressionSyntax sectionName)
        {
            SectionKeyword = sectionKeyword;
            SectionName = sectionName;
        }

        public override SyntaxKind Kind => SyntaxKind.SectionStatement;
        public SyntaxToken SectionKeyword { get; }
        public ExpressionSyntax SectionName { get; }
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return SectionKeyword;
            yield return SectionName;
        }
    }
}
