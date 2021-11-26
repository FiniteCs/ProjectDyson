using Dyson.CodeAnalysis.Syntax.Expressions;
using Dyson.CodeAnalysis.Syntax.Statements;

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
            if (index >= tokens_.Length)
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
            StatementSyntax statement = ParsePrimaryStatement();
            SyntaxToken endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(diagnostics_, statement, endOfFileToken);
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParseIndexing();
        }

        private ExpressionSyntax ParseIndexing()
        {
            if (Current.Kind == SyntaxKind.OpenBracketToken)
            {
                SyntaxToken openBracket = NextToken();
                ExpressionSyntax expression = ParseExpression();
                SyntaxToken closeBracket = MatchToken(SyntaxKind.CloseBracketToken);
                return new BracketedArgumentListSyntax(openBracket, expression, closeBracket);
            }

            return ParseMemberAccess();
        }

        private ExpressionSyntax ParseMemberAccess()
        {
            ExpressionSyntax expression = ParseAssignmentExpression();

            while (Current.Kind == SyntaxKind.DotToken)
            {
                List<MemberAccessExpressionSyntax> memberAccesses = new();
                SyntaxToken dotToken = NextToken();
                ExpressionSyntax ex = ParseExpression();
                if (ex is MemberAccessExpressionSyntax m)
                {
                    memberAccesses.Add(m);
                    continue;
                }
                else if (ex.Kind != SyntaxKind.NameExpression)
                    return new InvalidExpression();
                return new MemberAccessExpressionSyntax(expression, dotToken, (NameExpressionSyntax)ex, memberAccesses);
            }

            return expression;
        }

        private ExpressionSyntax ParseAssignmentExpression()
        {
            if (Current.Kind == SyntaxKind.IdentifierToken &&
                PeekToken(1).Kind == SyntaxKind.EqualsToken)
            {
                SyntaxToken identifierToken = NextToken();
                ExpressionSyntax equalsClause = ParseExpression();
                if (equalsClause.Kind != SyntaxKind.EqualsClause)
                    return new InvalidExpression();

                return new AssignmentExpressionSyntax(identifierToken, (EqualsClauseSyntax)equalsClause);
            }

            return ParseBinaryExpression();
        }

        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            int unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
            if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                SyntaxToken operatorToken = NextToken();
                ExpressionSyntax operand = ParseBinaryExpression(unaryOperatorPrecedence);
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }
            else
            {
                left = ParsePrimaryExpression();
            }

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
            if (Current.Kind.IsLiteral())
            {
                SyntaxToken literalToken = NextToken();
                return new LiteralExpressionSyntax(literalToken);
            }

            switch (Current.Kind)
            {
                case SyntaxKind.OpenParenthesisToken:
                    {
                        SyntaxToken left = NextToken();
                        ExpressionSyntax expression = ParseExpression();
                        SyntaxToken right = MatchToken(SyntaxKind.CloseParenthesisToken);
                        return new ParenthesizedExpressionSyntax(left, expression, right);
                    }

                case SyntaxKind.FalseKeyword:
                case SyntaxKind.TrueKeyword:
                    {
                        SyntaxToken keywordToken = NextToken();
                        bool value = keywordToken.Kind == SyntaxKind.TrueKeyword;
                        return new LiteralExpressionSyntax(keywordToken, value);
                    }

                case SyntaxKind.IniKeyword:
                    {
                        SyntaxToken iniKeyword = NextToken();
                        ExpressionSyntax indexingExpression = ParseExpression();
                        return new IniKeyIndexingExpressionSyntax(iniKeyword, indexingExpression);
                    }

                case SyntaxKind.IdentifierToken:
                    {
                        SyntaxToken identifier = NextToken();
                        return new NameExpressionSyntax(identifier);
                    }

                case SyntaxKind.EqualsToken:
                    {
                        SyntaxToken equalsToken = NextToken();
                        ExpressionSyntax expression = ParseExpression();
                        return new EqualsClauseSyntax(equalsToken, expression);
                    }
                default:
                    {
                        return new InvalidExpression();
                    }
            }
        }

        private StatementSyntax ParsePrimaryStatement()
        {
            if (Current.Kind.IsValueType())
            {
                SyntaxToken typeKeyword = NextToken();
                ExpressionSyntax variableAssignment = ParseExpression();
                if (variableAssignment.Kind != SyntaxKind.VariableAssignmentExpression)
                    return new InvalidStatement();

                return new VariableDeclarationStatementSyntax(typeKeyword, (AssignmentExpressionSyntax)variableAssignment);
            }

            switch (Current.Kind)
            {
                case SyntaxKind.IniKeyword:
                    {
                        SyntaxToken iniKeyword = NextToken();
                        ExpressionSyntax equalsClause = ParseExpression();
                        if (equalsClause.Kind != SyntaxKind.EqualsClause)
                            return new InvalidStatement();

                        return new IniDefiningStatementSyntax(iniKeyword, (EqualsClauseSyntax)equalsClause);
                    }
                case SyntaxKind.SectionKeyword:
                    {
                        SyntaxToken sectionKeyword = NextToken();
                        ExpressionSyntax sectionName = ParseExpression();
                        return new SectionStatementSyntax(sectionKeyword, sectionName);
                    }
                case SyntaxKind.IdentifierToken when PeekToken(1).Kind == SyntaxKind.EqualsToken:
                    {
                        ExpressionSyntax variableAssignment = ParseExpression();
                        if (variableAssignment.Kind == SyntaxKind.VariableAssignmentExpression)
                            return new InvalidStatement();

                        return new VariableReassignmentStatementSyntax((AssignmentExpressionSyntax)variableAssignment);
                    }
                default:
                    return new InvalidStatement();
            }
        }
    }
}
