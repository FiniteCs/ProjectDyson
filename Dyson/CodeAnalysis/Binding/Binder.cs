using Dyson.CodeAnalysis.Binding.BoundExpressions;
using Dyson.CodeAnalysis.Binding.BoundOperators;
using Dyson.CodeAnalysis.Binding.BoundStatements;
using Dyson.CodeAnalysis.Syntax;
using Dyson.CodeAnalysis.Syntax.Expressions;
using Dyson.CodeAnalysis.Syntax.Statements;

namespace Dyson.CodeAnalysis.Binding
{
    internal sealed class Binder
    {
        private readonly List<string> diagnostics_;
        private readonly Dictionary<VariableSymbol, object> variables_;

        public Binder(Dictionary<VariableSymbol, object> variables)
        {
            diagnostics_ = new();
            variables_ = variables;
        }

        public IEnumerable<string> Diagnostics => diagnostics_;

        private static Type BindValueType(SyntaxToken valueType)
        {
            return valueType.Kind switch
            {
                SyntaxKind.LongKeyword => typeof(long),
                SyntaxKind.StringKeyword => typeof(string),
                SyntaxKind.BoolKeyword => typeof(bool),
                _ => null,
            };
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            return syntax.Kind switch
            {
                SyntaxKind.ParenthesizedExpression => BindParenthesizedExpression((ParenthesizedExpressionSyntax)syntax),
                SyntaxKind.LiteralExpression => BindLiteralExpression((LiteralExpressionSyntax)syntax),
                SyntaxKind.UnaryExpression => BindUnaryExpression((UnaryExpressionSyntax)syntax),
                SyntaxKind.BinaryExpression => BindBinaryExpression((BinaryExpressionSyntax)syntax),
                SyntaxKind.EqualsClause => BindEqualsClause((EqualsClauseSyntax)syntax),
                SyntaxKind.InvalidExpression => null,
                _ => throw new Exception($"Unexpected syntax {syntax.Kind}"),
            };
        }

        public BoundStatement BindStatement(StatementSyntax syntax)
        {
            return syntax.Kind switch
            {
                SyntaxKind.IniDefiningStatement => BindIniDefiningStatement((IniDefiningStatementSyntax)syntax),
                SyntaxKind.VariableDeclarationStatement => BindVariableDeclarationStatement((VariableDeclarationStatementSyntax)syntax),
                SyntaxKind.VariableReassignmentStatement => BindReassignmentStatement((VariableReassignmentStatementSyntax)syntax),
                SyntaxKind.InvalidStatement => null,
                _ => throw new Exception($"Unexpected syntax {syntax.Kind}"),
            };
        }

        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }

        private static BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            object value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            BoundExpression boundOperand = BindExpression(syntax.Operand);
            BoundUnaryOperator boundOperator = BoundUnaryOperator.Bind(syntax.UnaryOperator.Kind, boundOperand.Type);
            if (boundOperator == null)
            {
                diagnostics_.Add($"Unary operator '{syntax.UnaryOperator.Text}' is not defined for type {boundOperand.Type}.");
                return boundOperand;
            }
            return new BoundUnaryExpression(boundOperator, boundOperand);
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            BoundExpression boundLeft = BindExpression(syntax.Left);
            BoundExpression boundRight = BindExpression(syntax.Right);
            BoundBinaryOperator boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);
            if (boundOperator == null)
            {
                diagnostics_.Add($"Binary operator '{syntax.OperatorToken.Text}'" +
                                 $" is not defined for types {boundLeft.Type} and {boundRight.Type}");
                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }

        private BoundExpression BindEqualsClause(EqualsClauseSyntax syntax)
        {
            BoundExpression expression = BindExpression(syntax.Expression);
            return new BoundEqualsClause(expression);
        }

        private BoundStatement BindIniDefiningStatement(IniDefiningStatementSyntax syntax)
        {
            BoundExpression expression = BindExpression(syntax.EqualsClause);
            BoundIniDefiningStatement ini = new(expression);
            if (expression.Type != BoundIniDefiningStatement.IniType)
            {
                diagnostics_.Add($"Cannot convert from type {ini.ExpressionType} to type {BoundIniDefiningStatement.IniType}");
                return null;
            }

            return new BoundIniDefiningStatement(expression);
        }

        private BoundStatement BindVariableDeclarationStatement(VariableDeclarationStatementSyntax syntax)
        {
            string name = syntax.VariableAssignment.IdentifierToken.Text;
            BoundExpression assignment = BindExpression(syntax.VariableAssignment.EqualsClause.Expression);

            VariableSymbol existingVariable = variables_.Keys.FirstOrDefault(x => x.Name == name);
            if (existingVariable != null)
                diagnostics_.Add($"Variable '{name}' is already defined");


            Type type = BindValueType(syntax.TypeKeyword);
            VariableSymbol variable = new(name, type);
            variables_[variable] = null;
            if (assignment.Type != type)
                diagnostics_.Add($"Cannot convert from type {assignment.Type} to type {type}");

            return new BoundAssignmentExpression(variable, assignment);
        }

        private BoundStatement BindReassignmentStatement(VariableReassignmentStatementSyntax syntax)
        {
            string name = syntax.AssignmentExpression.IdentifierToken.Text;
            BoundExpression reassignment = BindExpression(syntax.AssignmentExpression.EqualsClause);

            VariableSymbol existingVariable = variables_.Keys.FirstOrDefault(x => x.Name == name);
            if (existingVariable == null)
                diagnostics_.Add($"Variable '{name}' does not exist");
            else
            {
                Type variableType = existingVariable.Type;
                Type reassignmentType = reassignment.Type;
                if (reassignmentType != variableType)
                    diagnostics_.Add($"Cannot convert from type {reassignmentType} to type {variableType}");

                return new BoundAssignmentExpression(existingVariable, reassignment);
            }

            return null;
        }
    }
}
