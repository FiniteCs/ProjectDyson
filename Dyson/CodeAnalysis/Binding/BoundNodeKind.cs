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
        VariableExpression,
        BracketedArgumentList,
        IniKeyIndexing,

        // Statements
        IniDefiningStatement,
        VariableAssignment,
        VariableDeclarationStatement,
        SectionStatement,
        MemberAccess,
    }
}
