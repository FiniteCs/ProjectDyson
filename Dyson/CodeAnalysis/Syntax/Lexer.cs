namespace Dyson.CodeAnalysis.Syntax
{
    internal sealed class Lexer
    {
        private readonly string text_;
        private int position_;
        private readonly List<string> diagnostics_;
        public Lexer(string text)
        {
            text_ = text;
            position_ = 0;
            diagnostics_ = new();
        }

        public IEnumerable<string> Diagnostics => diagnostics_;

        private char PeekChar(int charOffset)
        {
            int index = position_ + charOffset;
            if (index >= text_.Length)
                return '\0'; // End of file

            return text_[index];
        }

        private char Current => PeekChar(0);

        private char Lookahead => PeekChar(1);

        public SyntaxToken Lex()
        {
            if (Current == '\0')
                return new SyntaxToken(SyntaxKind.EndOfFileToken, position_, "\0", null);

            int start = position_;

            switch (Current)
            {
                case '+':
                    return new SyntaxToken(SyntaxKind.PlusToken, position_++, "+", null);
                case '-':
                    return new SyntaxToken(SyntaxKind.MinusToken, position_++, "-", null);
                case '*':
                    return new SyntaxToken(SyntaxKind.StarToken, position_++, "*", null);
                case '/':
                    return new SyntaxToken(SyntaxKind.SlashToken, position_++, "/", null);
                case '(':
                    return new SyntaxToken(SyntaxKind.OpenParenthesisToken, position_++, "(", null);
                case ')':
                    return new SyntaxToken(SyntaxKind.CloseParenthesisToken, position_++, ")", null);
                case '&':
                    if (Lookahead == '&')
                    {
                        position_ += 2;
                        return new SyntaxToken(SyntaxKind.AmpersandAmpersandToken, start, "&&", null);
                    }
                    break;
                case '|':
                    if (Lookahead == '|')
                    {
                        position_ += 2;
                        return new SyntaxToken(SyntaxKind.PipePipeToken, start, "||", null);
                    }
                    break;
                case '=':
                    if (Lookahead == '=')
                    {
                        position_ += 2;
                        return new SyntaxToken(SyntaxKind.EqualsEqualsToken, start, "==", null);
                    }
                    else
                    {
                        position_ += 1;
                        return new SyntaxToken(SyntaxKind.EqualsToken, start, "=", null);
                    }
                case '!':
                    if (Lookahead == '=')
                    {
                        position_ += 2;
                        return new SyntaxToken(SyntaxKind.BangEqualsToken, start, "!=", null);
                    }
                    else
                    {
                        position_ += 1;
                        return new SyntaxToken(SyntaxKind.BangToken, start, "!", null);
                    }

                default:
                    break;
            }

            if (Current == '\"')
            {
                position_++;
                int hStart = position_;
                while (char.IsAscii(Current) &&
                       Current != '\"')
                    position_++;

                int length = position_ - hStart;
                string text = text_.Substring(hStart, length);
                position_++;
                return new SyntaxToken(SyntaxKind.StringToken, hStart, text, text);
            }

            if (char.IsWhiteSpace(Current))
            {
                while (char.IsWhiteSpace(Current))
                    position_++;

                int length = position_ - start;
                string text = text_.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            if (char.IsLetter(Current))
            {
                while (char.IsLetter(Current))
                    position_++;

                int length = position_ - start;
                string text = text_.Substring(start, length);
                SyntaxKind kind = SyntaxFacts.GetKeywordKind(text);
                return new SyntaxToken(kind, start, text, null);
            }

            if (char.IsDigit(Current))
            {
                while (char.IsDigit(Current))
                    position_++;

                int length = position_ - start;
                string text = text_.Substring(start, length);
                if (!long.TryParse(text, out long value))
                    diagnostics_.Add($"{text} is not a valid Int64");

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            diagnostics_.Add($"Unexpected character '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, position_++, text_.Substring(position_ - 1, 1), null);
        }
    }
}
