using Dyson.CodeAnalysis.Binding.Types;
using Dyson.CodeAnalysis.Binding.Types.Properties;

namespace Dyson.CodeAnalysis.Binding.BoundExpressions
{
    internal sealed class BoundMemberAccessExpression
        : BoundExpression
    {
        public BoundMemberAccessExpression(StaticType staticType, Property property)
        {
            StaticType = staticType;
            Property = property;
        }

        public override Type Type => Property.Type;
        public override BoundNodeKind Kind => BoundNodeKind.MemberAccess;
        public StaticType StaticType { get; }
        public Property Property { get; }
    }
}
