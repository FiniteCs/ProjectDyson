using Dyson.CodeAnalysis.Syntax.Expressions;

namespace Dyson.CodeAnalysis.Syntax.Statements
{
    internal sealed class VariableDeclarationStatementSyntax
        : StatementSyntax
    {
        public VariableDeclarationStatementSyntax(SyntaxToken typeKeyword, 
                                                  AssignmentExpressionSyntax variableAssignment)
        {
            TypeKeyword = typeKeyword;
            VariableAssignment = variableAssignment;
        }

        public override SyntaxKind Kind => SyntaxKind.VariableDeclarationStatement;

        public SyntaxToken TypeKeyword { get; }
        public AssignmentExpressionSyntax VariableAssignment { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return TypeKeyword;
            yield return VariableAssignment;
        }
    }
}
