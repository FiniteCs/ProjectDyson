using Dyson.CodeAnalysis.Binding.BoundExpressions;

namespace Dyson.CodeAnalysis.Binding
{
    internal class BoundLiteralExpression
        : BoundExpression
    {
        public BoundLiteralExpression(object value)
        {
            Value = value;
        }

        public override Type Type => Value.GetType();
        public override BoundNodeKind Kind => BoundNodeKind.LiteralExpression;
        public object Value { get; }
    }
}
