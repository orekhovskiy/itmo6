using SyntaxAnalysisLibray;
using SyntaxAnalysisLibray.Lexer;
using SyntaxAnalysisLibray.Parser;
using System;

namespace ifmo.compilers
{
    class Program
    {
        public static void Main(params string[] args)
        {
            var fileName = args[0];
            var code = IOManager.ReadFile(fileName);
            var tokenizedCode = Lexer.Tokenize(code);
            var prefixCode = PrefixMaker.MakePrefix(tokenizedCode);
            var ast = Parser.Parse(prefixCode);
            var prinVersion = MakePrintVersion(ast, 0);
            Console.WriteLine(prinVersion);
        }

        private static string MakePrintVersion(AbstractSyntaxTree ast, int nestedLevel)
        {
            var str = $"{GetIndent(nestedLevel)}{ast.Value}\n";
            foreach(var node in ast.ChildNodes)
            {
                str += MakePrintVersion(node, nestedLevel + 1);
            }
            return str;
        }

        private static string GetIndent(int nestedLevel)
            => nestedLevel > 0 ? $"{new String('\t', nestedLevel)}>  " : ">  ";
    }
}
