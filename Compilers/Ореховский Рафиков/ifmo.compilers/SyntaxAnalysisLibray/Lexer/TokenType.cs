using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalysisLibray.Lexer
{
    public enum TokenType
    {
        Comma,
        LeftBracket,
        RightBracket,
        Const,
        Ident,
        Var,
        EqualSign,
        UnaryOperator,
        BinaryOperator,
        While,
        Do,
        Begin, 
        End,
        Dot,
        Space,
        Tab,
        LineBreak,
        EOF
    }
}
