using Dyson.CodeAnalysis.Syntax.Expressions;
using Dyson.CodeAnalysis.Syntax;
using Dyson.CodeAnalysis.Syntax.Statements;

namespace Dyson.CodeAnalysis.Binding
{
    internal sealed class Binder
    {
        private readonly List<string> diagnostics_;

        public Binder()
        {
            diagnostics_ = new();
        }

        public IEnumerable<string> Diagnostics => diagnostics_;

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

        private BoundExpression BindEqualsClause(EqualsClauseSyntax equalsClauseSyntax)
        {
            BoundExpression expression = BindExpression(equalsClauseSyntax.Expression);
            return new BoundEqualsClause(expression);
        }

        private BoundExpression BindIniDefiningStatement(IniDefiningStatementSyntax iniDefiningStatement)
        {
            BoundExpression expression = BindExpression(iniDefiningStatement.EqualsClause);
            BoundIniDefiningStatement ini = new(expression);
            if (expression.Type != BoundIniDefiningStatement.IniType)
            {
                diagnostics_.Add($"Cannot convert from type {ini.ExpressionType} to type {BoundIniDefiningStatement.IniType}");
                return expression;
            }

            return new BoundIniDefiningStatement(expression);
        }
    }
}
