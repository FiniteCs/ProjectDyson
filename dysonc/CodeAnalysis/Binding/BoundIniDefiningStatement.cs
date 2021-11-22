namespace Dyson.CodeAnalysis.Binding
{
    internal sealed class BoundIniDefiningStatement
        : BoundExpression
    {
        public BoundIniDefiningStatement(BoundExpression expression)
        {
            ExpressionType = expression.Type;
        }

        public override Type Type => typeof(void);
        public override BoundNodeKind Kind => BoundNodeKind.IniDefiningStatement;
        public static Type IniType => typeof(string);
        public Type ExpressionType { get; }
    }
}
