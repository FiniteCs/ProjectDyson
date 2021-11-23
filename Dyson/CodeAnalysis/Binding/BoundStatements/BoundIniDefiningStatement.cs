using Dyson.CodeAnalysis.Binding.BoundExpressions;

namespace Dyson.CodeAnalysis.Binding.BoundStatements
{
    internal sealed class BoundIniDefiningStatement
        : BoundStatement
    {
        public BoundIniDefiningStatement(BoundExpression expression)
        {
            ExpressionType = expression.Type;
        }

        public override Type Type => typeof(void);
        public override BoundNodeKind Kind => BoundNodeKind.IniDefiningStatement;
        public static Type ExpectedType => typeof(string);
        public Type ExpressionType { get; }
    }
}
