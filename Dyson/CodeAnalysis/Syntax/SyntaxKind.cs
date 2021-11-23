namespace Dyson.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        EndOfFileToken,
        BadToken,

        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        EqualsToken,

        WhitespaceToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        IdentifierToken,

        // Literals
        NumberToken,
        StringToken,

        // Keywords
        IniKeyword,
        LongKeyword,

        // Clauses
        EqualsClause,

        // Expressions
        InvalidExpression,

        ParenthesizedExpression,
        LiteralExpression,
        VariableAssignmentExpression,
        UnaryExpression,
        BinaryExpression,

        // Statements
        IniDefiningStatement,
        VariableDeclarationStatement,
        InvalidStatement,
        VariableReassignmentStatement,
        StringKeyword,
    }
}
