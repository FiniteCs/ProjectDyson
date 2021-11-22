global using System;
global using System.Collections.Generic;
global using System.Linq;

using Dyson.CodeAnalysis.Syntax;

namespace Dyson
{
    internal static class Program
    {
        private static void Main()
        {
            while (true)
            {
                Console.ResetColor();
                Console.Write("> ");
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    break;

                if (line == "#cls")
                {
                    Console.Clear();
                    continue;
                }

                SyntaxTree syntaxTree = SyntaxTree.Parse(line);

                PrettyPrint(syntaxTree.Root);
                if (syntaxTree.Diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach (var diagnostic in syntaxTree.Diagnostics)
                        Console.WriteLine(diagnostic);

                    Console.ResetColor();
                }
            }
        }

        private static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(indent);
            Console.Write(marker);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(node.Kind);

            Console.ForegroundColor = ConsoleColor.Green;
            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Cyan;
            indent += isLast ? "   " : "│  ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                PrettyPrint(child, indent, child == lastChild);
        }
    }
}
