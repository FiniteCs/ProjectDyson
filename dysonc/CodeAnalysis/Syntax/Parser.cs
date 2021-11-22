namespace Dyson.CodeAnalysis.Syntax
{
    internal sealed class Parser
    {
        private readonly SyntaxToken[] tokens_;
        private int position_;
        private readonly List<string> diagnostics_;
        public Parser(string text)
        {
            List<SyntaxToken> tokens = new();
            Lexer lexer = new(text);
            SyntaxToken token;
            do
            {
                token = lexer.Lex();
                if (token.Kind != SyntaxKind.WhitespaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                    tokens.Add(token);
            } while (token.Kind != SyntaxKind.EndOfFileToken);

            tokens_ = tokens.ToArray();
            position_ = 0;
            diagnostics_ = new();
            diagnostics_.AddRange(lexer.Diagnostics);
        }

        private SyntaxToken PeekToken(int tokenOffset)
        {
            int index = position_ + tokenOffset;
            if (tokenOffset >= tokens_.Length)
                return tokens_[^1];

            return tokens_[index];
        }

        private SyntaxToken Current => PeekToken(0);

        private SyntaxToken NextToken()
        {
            SyntaxToken current = Current;
            position_++;
            return current;
        }

        private SyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();

            diagnostics_.Add($"Unexpected token '{Current.Kind}' expected '{kind}'");
            return new SyntaxToken(kind, Current.Position, null, null);
        }

        public SyntaxTree Parse()
        {
            ExpressionSyntax expression = ParseExpression();
            SyntaxToken endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(diagnostics_, expression, endOfFileToken);
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParseBinaryExpression();
        }

        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left = ParsePrimaryExpression();

            while (true)
            {
                int precedence = Current.Kind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence)
                    break;

                SyntaxToken operatorToken = NextToken();
                ExpressionSyntax right = ParseBinaryExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.Kind == SyntaxKind.OpenParenthesisToken)
            {
                SyntaxToken left = NextToken();
                ExpressionSyntax expression = ParseExpression();
                SyntaxToken right = MatchToken(SyntaxKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, expression, right);
            }

            SyntaxToken numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}
