using Dyson.CodeAnalysis.Binding;
using Dyson.CodeAnalysis.Syntax;

namespace Dyson.CodeAnalysis
{
    public sealed class Compilation
    {
        public Compilation(SyntaxTree syntaxTree)
        {
            Syntax = syntaxTree;
        }

        public SyntaxTree Syntax { get; }

        public Result GetResult(Dictionary<VariableSymbol, object> variables)
        {
            Binder binder = new(variables);
            binder.BindStatement(Syntax.Root);

            string[] diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToArray();
            return new Result(diagnostics);
        }
    }
}
