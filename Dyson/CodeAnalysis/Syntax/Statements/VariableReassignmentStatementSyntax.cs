using Dyson.CodeAnalysis.Syntax.Expressions;

namespace Dyson.CodeAnalysis.Syntax.Statements
{
    internal sealed class VariableReassignmentStatementSyntax
        : StatementSyntax
    {
        public VariableReassignmentStatementSyntax(AssignmentExpressionSyntax assignmentExpression)
        {
            AssignmentExpression = assignmentExpression;
        }

        public override SyntaxKind Kind => SyntaxKind.VariableReassignmentStatement;

        public AssignmentExpressionSyntax AssignmentExpression { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return AssignmentExpression;
        }
    }
}
