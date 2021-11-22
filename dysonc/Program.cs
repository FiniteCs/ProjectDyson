global using System;
global using System.Collections.Generic;

using Dyson.CodeAnalysis.Syntax;

namespace Dyson
{
    internal static class Program
    {
        private static void Main()
        {
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    break;

                Lexer lexer = new(line);
                while (true)
                {
                    SyntaxToken token = lexer.Lex();
                    if (token.Kind == SyntaxKind.EndOfFileToken)
                        break;
                    Console.WriteLine($"{token.Kind} : '{token.Text}' : {token.Value ?? "null"}");
                }
            }
        }
    }
}
