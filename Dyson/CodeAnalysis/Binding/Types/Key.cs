using Dyson.CodeAnalysis.Binding.Types.Properties;

namespace Dyson.CodeAnalysis.Binding.Types
{
    internal sealed class Key
        : StaticType
    {
        public override Property[] Properties 
        {
            get
            {
                Property[] properties = 
                { 
                    new Property(typeof(string), "Value"),
                    new Property(typeof(string), "Name")
                };
                return properties;
            }
        }

        public override string Name => "Key";
    }
}
