namespace Dyson.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        // Clauses
        BoundEqualsClause,

        // Expressions
        BinaryExpression,
        LiteralExpression,
        UnaryExpression,

        // Statements
        IniDefiningStatement,
        VariableAssignment,
        VariableDeclarationStatement,
        SectionStatement,
    }
}
