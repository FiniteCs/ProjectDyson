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
        private readonly List<SectionSymbol> sections_;

        public Binder(Dictionary<VariableSymbol, object> variables, List<SectionSymbol> sections)
        {
            diagnostics_ = new();
            variables_ = variables;
            sections_ = sections;
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
                SyntaxKind.SectionStatement => BindSectionStatement((SectionStatementSyntax)syntax),
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
                diagnostics_.Add($"Unary operator '{syntax.UnaryOperator.Text}'" +
                                 $" is not defined for type '{boundOperand.Type.S()}'");
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
                                 $" is not defined for types '{boundLeft.Type.S()}' and '{boundRight.Type.S()}'");
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
            if (expression.Type != BoundIniDefiningStatement.ExpectedType)
            {
                ReportIncompatibleTypes(ini.ExpressionType, BoundIniDefiningStatement.ExpectedType);
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
                ReportIncompatibleTypes(assignment.Type, type);

            return new BoundAssignmentStatement(variable, assignment);
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
                    ReportIncompatibleTypes(reassignmentType, variableType);

                return new BoundAssignmentStatement(existingVariable, reassignment);
            }

            return null;
        }

        private BoundStatement BindSectionStatement(SectionStatementSyntax syntax)
        {
            BoundLiteralExpression expression = (BoundLiteralExpression)BindExpression(syntax.SectionName);
            if (expression.Type != BoundSectionStatementSyntax.ExpectedType)
                ReportIncompatibleTypes(expression.Type, BoundSectionStatementSyntax.ExpectedType);

            string name = expression.Value.ToString();
            SectionSymbol section = new(name);
            sections_.Add(section);
            return new BoundSectionStatementSyntax(section, expression);
        }

        private void ReportIncompatibleTypes(Type left, Type right) => diagnostics_.Add($"Cannot convert type '{left.S()}' to '{right.S()}'");
    }
}
