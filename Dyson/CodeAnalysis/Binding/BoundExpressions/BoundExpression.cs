namespace Dyson.CodeAnalysis.Binding.BoundExpressions
{
    internal abstract class BoundExpression
        : BoundNode
    {
        public abstract Type Type { get; }
    }
}
