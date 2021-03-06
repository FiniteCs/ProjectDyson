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
                case SyntaxKind.BangToken:
                    return 6;

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
                    return 5;

                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 4;

                case SyntaxKind.EqualsEqualsToken:
                case SyntaxKind.BangEqualsToken:
                    return 3;

                case SyntaxKind.AmpersandAmpersandToken:
                    return 2;

                case SyntaxKind.PipePipeToken:
                    return 1;
                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeywordKind(string text)
        {
            return text switch
            {
                "ini" => SyntaxKind.IniKeyword,
                "long" => SyntaxKind.LongKeyword,
                "string" => SyntaxKind.StringKeyword,
                "true" => SyntaxKind.TrueKeyword,
                "false" => SyntaxKind.FalseKeyword,
                "bool" => SyntaxKind.BoolKeyword,
                "section" => SyntaxKind.SectionKeyword,
                "key" => SyntaxKind.KeyKeyword,
                _ => SyntaxKind.IdentifierToken,
            };
        }

        public static bool IsLiteral(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.NumericLiteralToken:
                case SyntaxKind.StringLiteralToken:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsValueType(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.LongKeyword:
                case SyntaxKind.StringKeyword:
                case SyntaxKind.BoolKeyword:
                case SyntaxKind.KeyKeyword:
                    return true;

                default:
                    return false;
            }
        }
    }
}
