using System;
using System.Collections.Generic;
using System.Text;
using static CommonsLibrary.EnumUtilizer;

namespace SyntaxAnalysisLibray.Lexer
{
    static class Grammar
    {
        private static readonly Dictionary<string, string> s_rules = new Dictionary<string, string>();

        public static Dictionary<TokenType, string> TokenDefinitions { get; } = new Dictionary<TokenType, string>();

        static Grammar()
        {
            // Правила грамматики
            s_rules.Add("Comma", $"(,)");
            s_rules.Add("Space", $"( )");
            s_rules.Add("LeftBracket", $"(\\()");
            s_rules.Add("RightBracket", $"(\\))");
            s_rules.Add("Digit", $"(0|1|2|3|4|5|6|7|8|9)");
            s_rules.Add("Letter", $"(a|b|c|d|e|f|g|h|i|j|k|l|m|n|o|p|q|r|s|t|u|v|w|x|y|z)");
            s_rules.Add("Const", $"({s_rules["Digit"]}+)");
            s_rules.Add("Ident", $"({s_rules["Letter"]}({s_rules["Letter"]}|{s_rules["Digit"]})*)");
            s_rules.Add("Var", $"(Var)");
            s_rules.Add("EqualSign", $"(=)");
            s_rules.Add("UnaryOperator", $"(not|-)");
            s_rules.Add("BinaryOperator", $"(-|\\+|\\*|\\/|<|>|==)");
            s_rules.Add("While", $"(WHILE)");
            s_rules.Add("Do", $"(DO)");
            s_rules.Add("Begin", $"(Begin)");
            s_rules.Add("End", $"(End)");
            s_rules.Add("LineBreak", $"(\r\n)");
            s_rules.Add("Tab", $"(\t)");
            s_rules.Add("Dot", $"(\\.)");

            // Соответствие между правилами и определениями токенов
            foreach (TokenType tokenType in Enum.GetValues(typeof(TokenType)))
            {
                if (tokenType != TokenType.EOF)
                {
                    TokenDefinitions.Add(tokenType, "^" + s_rules[UniformEnumToString(tokenType, Capitalization.AsListed)]);
                }
            }
        }
    }
}
