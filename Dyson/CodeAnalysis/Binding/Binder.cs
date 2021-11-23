using Dyson.CodeAnalysis.Syntax.Expressions;
using Dyson.CodeAnalysis.Syntax;
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
            switch (valueType.Kind)
            {
                case SyntaxKind.LongKeyword:
                    return typeof(long);
                case SyntaxKind.StringKeyword:
                    return typeof(string);
                default:
                    return null;
            }
        }

        private BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.ParenthesizedExpression:
                    return BindParenthesizedExpression((ParenthesizedExpressionSyntax)syntax);
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionSyntax)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntax)syntax);
                case SyntaxKind.EqualsClause:
                    return BindEqualsClause((EqualsClauseSyntax)syntax);
                case SyntaxKind.InvalidExpression:
                    return null;
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        public BoundExpression BindStatement(StatementSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.IniDefiningStatement:
                    return BindIniDefiningStatement((IniDefiningStatementSyntax)syntax);
                case SyntaxKind.VariableDeclarationStatement:
                    return BindVariableDeclarationStatement((VariableDeclarationStatementSyntax)syntax);
                case SyntaxKind.VariableReassignmentStatement:
                    return BindReassignmentStatement((VariableReassignmentStatementSyntax)syntax);
                case SyntaxKind.InvalidStatement:
                    return null;
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }

        private static BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            object value = syntax.LiteralToken.Value ?? 0;
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

        private BoundExpression BindIniDefiningStatement(IniDefiningStatementSyntax syntax)
        {
            BoundExpression expression = BindExpression(syntax.EqualsClause);
            BoundIniDefiningStatement ini = new(expression);
            if (expression.Type != BoundIniDefiningStatement.IniType)
            {
                diagnostics_.Add($"Cannot convert from type {ini.ExpressionType} to type {BoundIniDefiningStatement.IniType}");
                return expression;
            }

            return new BoundIniDefiningStatement(expression);
        }

        private BoundExpression BindVariableDeclarationStatement(VariableDeclarationStatementSyntax syntax)
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

        private BoundExpression BindReassignmentStatement(VariableReassignmentStatementSyntax syntax)
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
