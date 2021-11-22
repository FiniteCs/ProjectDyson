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

        // Clauses
        EqualsClause,

        // Expressions
        InvalidExpression,

        BinaryExpression,
        ParenthesizedExpression,
        LiteralExpression,

        // Statements
        IniDefiningStatement,
    }
}
