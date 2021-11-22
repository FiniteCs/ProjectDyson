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

        public SyntaxToken Lex()
        {
            if (Current == '\0')
                return new SyntaxToken(SyntaxKind.EndOfFileToken, position_, "\0", null);

            int start = position_;

            if (char.IsWhiteSpace(Current))
            {
                while (char.IsWhiteSpace(Current))
                    position_++;

                int length = position_ - start;
                string text = text_.Substring(start, length);
                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            if (char.IsDigit(Current))
            {
                while (char.IsDigit(Current))
                    position_++;

                int length = position_ - start;
                string text = text_.Substring(start, length);
                if (!long.TryParse(text, out long value))
                    diagnostics_.Add($"{text} is not a valid Int32");

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            diagnostics_.Add($"Unexpected character '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, position_++, text_.Substring(position_ - 1, 1), null);
        }
    }
}
