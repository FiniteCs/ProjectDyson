namespace Dyson.CodeAnalysis.Syntax.Expressions
{
    internal sealed class AssignmentExpressionSyntax
        : ExpressionSyntax
    {
        public AssignmentExpressionSyntax(SyntaxToken identifierToken, EqualsClauseSyntax equalsClause)
        {
            IdentifierToken = identifierToken;
            EqualsClause = equalsClause;
        }

        public override SyntaxKind Kind => SyntaxKind.VariableAssignmentExpression;

        public SyntaxToken IdentifierToken { get; }
        public EqualsClauseSyntax EqualsClause { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IdentifierToken;
            yield return EqualsClause;
        }
    }
}
