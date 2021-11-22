namespace Dyson.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        // Clauses
        BoundEqualsClause,

        // Expressions
        BinaryExpression,
        LiteralExpression,

        // Statements
        IniDefiningStatement,
    }
}
