namespace Dyson.CodeAnalysis.Syntax
{
    internal enum SyntaxKind
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

        // Expressions
        BinaryExpression,
        ParenthesizedExpression,
        LiteralExpression,
        EqualsClause,
        InvalidExpression,

        // Statements
        IniDefiningStatement,
    }
}
