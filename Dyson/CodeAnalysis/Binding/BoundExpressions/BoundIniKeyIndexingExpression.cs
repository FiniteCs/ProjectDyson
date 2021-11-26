using Dyson.CodeAnalysis.Binding.Types;

namespace Dyson.CodeAnalysis.Binding.BoundExpressions
{
    internal sealed class BoundIniKeyIndexingExpression
        : BoundExpression
    {
        public BoundIniKeyIndexingExpression(BoundExpression boundExpression)
        {
            BoundExpression = boundExpression;
        }

        public override Type Type => typeof(Key);
        public override BoundNodeKind Kind => BoundNodeKind.IniKeyIndexing;
        public static Type ExpectedIndexType => typeof(string);
        public Type IndexType => BoundExpression.Type;
        public BoundExpression BoundExpression { get; }
    }
}
