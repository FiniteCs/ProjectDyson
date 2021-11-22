using Dyson.CodeAnalysis.Syntax.Statements;

namespace Dyson.CodeAnalysis.Syntax
{
    internal sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<string> diagnostics, StatementSyntax root, SyntaxToken endOfFileToken)
        {
            Diagnostics = diagnostics;
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public IEnumerable<string> Diagnostics { get; }
        public StatementSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public static SyntaxTree Parse(string text)
        {
            Parser parser = new(text);
            return parser.Parse();
        }
    }
}
