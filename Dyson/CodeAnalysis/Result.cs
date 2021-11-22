namespace Dyson.CodeAnalysis
{
    public sealed class Result
    {
        public Result(IEnumerable<string> diagnostics)
        {
            Diagnostics = diagnostics;
        }

        public IEnumerable<string> Diagnostics { get; }
    }
}
