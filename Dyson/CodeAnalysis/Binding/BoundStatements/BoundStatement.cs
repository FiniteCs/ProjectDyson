namespace Dyson.CodeAnalysis.Binding.BoundStatements
{
    internal abstract class BoundStatement
        : BoundNode
    {
        public abstract Type Type { get; }
    }
}
