namespace Dyson.CodeAnalysis.Syntax.Statements
{
    internal class InvalidStatement
        : StatementSyntax
    {
        public InvalidStatement()
        {
        }

        public override SyntaxKind Kind => SyntaxKind.InvalidStatement;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}
