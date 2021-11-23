using Dyson.CodeAnalysis.Binding.BoundExpressions;

namespace Dyson.CodeAnalysis.Binding.BoundStatements
{
    internal sealed class BoundSectionStatementSyntax
        : BoundStatement
    {
        public BoundSectionStatementSyntax(SectionSymbol section, BoundExpression literalExpression)
        {
            Section = section;
            LiteralExpression = literalExpression;
        }

        public override Type Type => typeof(void);
        public override BoundNodeKind Kind => BoundNodeKind.SectionStatement;
        public static Type ExpectedType => typeof(string);
        public SectionSymbol Section { get; }
        public BoundExpression LiteralExpression { get; }
    }
}
