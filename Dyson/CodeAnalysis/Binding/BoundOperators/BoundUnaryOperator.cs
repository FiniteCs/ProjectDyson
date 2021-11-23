using Dyson.CodeAnalysis.Syntax;

namespace Dyson.CodeAnalysis.Binding.BoundOperators
{
    internal sealed class BoundUnaryOperator
    {
        private BoundUnaryOperator(SyntaxKind synatxKind, BoundUnaryOperatorKind kind, Type operandType)
            : this(synatxKind, kind, operandType, operandType)
        {
        }

        private BoundUnaryOperator(SyntaxKind synatxKind, BoundUnaryOperatorKind kind, Type operandType, Type resultType)
        {
            SynatxKind = synatxKind;
            Kind = kind;
            OperandType = operandType;
            Type = resultType;
        }

        public SyntaxKind SynatxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OperandType { get; }
        public Type Type { get; }

        private static readonly BoundUnaryOperator[] _operators =
        {
            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(long)),
            new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(long)),
        };

        public static BoundUnaryOperator Bind(SyntaxKind syntaxKind, Type operandType)
        {
            foreach (BoundUnaryOperator op in _operators)
            {
                if (op.SynatxKind == syntaxKind && op.OperandType == operandType)
                    return op;
            }

            return null;
        }
    }
}
