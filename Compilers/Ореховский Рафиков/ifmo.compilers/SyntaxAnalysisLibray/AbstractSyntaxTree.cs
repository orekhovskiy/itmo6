using SyntaxAnalysisLibray.Lexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SyntaxAnalysisLibray
{
    public class AbstractSyntaxTree
    {
        public AbstractSyntaxTree(string value)
        {
            Value = value;
            ChildNodes = new List<AbstractSyntaxTree>();
        }

        public AbstractSyntaxTree(string value, List<AbstractSyntaxTree> childNodes)
        {
            Value = value;
            ChildNodes = childNodes;
        }

        public string Value { get; set; }
        public List<AbstractSyntaxTree> ChildNodes { get; set; }
    }
}
