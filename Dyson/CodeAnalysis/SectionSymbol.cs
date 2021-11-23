namespace Dyson.CodeAnalysis
{
    public sealed class SectionSymbol
        : Symbol
    {
        public SectionSymbol(string sectionName)
        {
            SectionName = sectionName;
        }

        public string SectionName { get; }
    }
}
