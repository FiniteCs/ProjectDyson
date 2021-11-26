using Dyson.CodeAnalysis.Binding.Types.Properties;

namespace Dyson.CodeAnalysis.Binding.Types
{
    internal abstract class StaticType
    {
        public abstract Property[] Properties { get; }

        public abstract string Name { get; }

        private readonly static StaticType[] types_ =
        {
            new Key(),
        };

        public static StaticType GetStaticType(Type type)
        {
            foreach (var t in types_)
                if (type == t.GetType())
                    return t;

            return null;
        }

        public Property GetProperty(string name)
        {
            foreach (var p in Properties)
                if (p.Name == name)
                    return p;

            return null;
        }
    }
}
