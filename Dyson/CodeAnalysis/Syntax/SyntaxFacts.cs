namespace Dyson.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts
    {
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 3;

                default:
                    return 0;
            }
        }

        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.StarToken:
                case SyntaxKind.SlashToken:
                    return 2;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 1;

                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeywordKind(string text)
        {
            switch (text)
            {
                case "ini":
                    return SyntaxKind.IniKeyword;
                case "long":
                    return SyntaxKind.LongKeyword;
                case "string":
                    return SyntaxKind.StringKeyword;
                default:
                    return SyntaxKind.IdentifierToken;
            }
        }

        public static bool IsValueType(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.LongKeyword:
                case SyntaxKind.StringKeyword:
                    return true;

                default:
                    return false;
            }
        }
    }
}
