namespace Dyson.CodeAnalysis.Syntax
{
    internal enum SyntaxKind
    {
        // Tokens
        EndOfFileToken,
        BadToken,
        WhitespaceToken,
        NumberToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        BinaryExpression,
        ParenthesizedExpression,
        LiteralExpression,
    }
}
