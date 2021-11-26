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
        OpenBracketToken,
        CloseBracketToken,
        IdentifierToken,
        DotToken,

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
        NumericLiteralToken,
        StringLiteralToken,

        // Keywords
        IniKeyword,
        LongKeyword,
        StringKeyword,
        BoolKeyword,
        TrueKeyword,
        FalseKeyword,
        SectionKeyword,
        KeyKeyword,

        // Clauses
        EqualsClause,

        // Expressions
        InvalidExpression,

        ParenthesizedExpression,
        LiteralExpression,
        VariableAssignmentExpression,
        UnaryExpression,
        BinaryExpression,
        BracketedArgumentList,
        NameExpression,
        IniKeyIndexingExpression,
        MemberAccessExpression,

        // Statements
        IniDefiningStatement,
        SectionStatement,
        VariableDeclarationStatement,
        InvalidStatement,
        VariableReassignmentStatement,
    }
}
