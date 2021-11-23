namespace Dyson.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        // Tokens
        EndOfFileToken,
        BadToken,

        WhitespaceToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        IdentifierToken,

        // Binary Operators
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        EqualsToken,
        EqualsEqualsToken,
        BangEqualsToken,
        AmpersandAmpersandToken,
        PipePipeToken,

        // Unary
        BangToken,

        // Literals
        NumberToken,
        StringToken,

        // Keywords
        IniKeyword,
        LongKeyword,
        StringKeyword,
        BoolKeyword,
        TrueKeyword,
        FalseKeyword,

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
    }
}
