using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalysisLibray.Lexer
{
    public struct Token
    {
        public TokenType Type { get; }
        public string Content { get; }

        public Token(TokenType type, string content)
        {
            Type = type;
            Content = content;
        }

        public override string ToString()
        {
            return $"{Type}: {Content}";
        }
    }
}
