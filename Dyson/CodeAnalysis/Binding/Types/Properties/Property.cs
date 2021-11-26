namespace Dyson.CodeAnalysis.Binding.Types.Properties
{
    internal sealed class Property
    {
        public Property(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        public Type Type { get; }
        public string Name { get; }
    }
}
