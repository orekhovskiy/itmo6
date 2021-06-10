using System;
using System.Collections.Generic;
using System.Text;
using Terminal = SyntaxAnalysisLibray.Lexer.TokenType;

namespace SyntaxAnalysisLibray.Parser
{
    static class Grammar
    {
        public static Dictionary<NonTerminal, List<List<object>>> Rules { get; } = new Dictionary<NonTerminal, List<List<object>>>();

        static Grammar()
        {
            Rules.Add(NonTerminal.Root, new List<List<object>>
            {
                new List<object>{NonTerminal.VariableDeclaration, NonTerminal.ComputingDescription, Terminal.Dot, Terminal.EOF}
            });

            Rules.Add(NonTerminal.ComputingDescription, new List<List<object>>
            {
                new List<object>{NonTerminal.OperatorsList}
            });

            Rules.Add(NonTerminal.VariableDeclaration, new List<List<object>>
            {
                new List<object>{Terminal.Var, NonTerminal.VariablesList}
            });

            Rules.Add(NonTerminal.VariablesList, new List<List<object>>
            {
                new List<object>{Terminal.Ident, NonTerminal.VariablesContinuation}
            });

            Rules.Add(NonTerminal.VariablesContinuation, new List<List<object>>
            {
                new List<object>{Terminal.Comma, NonTerminal.VariablesList},
                new List<object>{}
            });

            Rules.Add(NonTerminal.OperatorsList, new List<List<object>>
            {
                new List<object>{NonTerminal.Operator, NonTerminal.OperatorsContinuation}
            });

            Rules.Add(NonTerminal.OperatorsContinuation, new List<List<object>>
            {
                new List<object>{NonTerminal.OperatorsList},
                new List<object>{}
            });

            Rules.Add(NonTerminal.Operator, new List<List<object>>
            {
                new List<object>{NonTerminal.Assignment},
                new List<object>{NonTerminal.ComplexOperator}
            });

            Rules.Add(NonTerminal.Assignment, new List<List<object>>
            {
                new List<object>{Terminal.Ident, Terminal.EqualSign, NonTerminal.Expression}
            });

            Rules.Add(NonTerminal.Expression, new List<List<object>>
            {
                new List<object>{Terminal.UnaryOperator, NonTerminal.Subexpression},
                new List<object>{NonTerminal.Subexpression}
            });

            Rules.Add(NonTerminal.Subexpression, new List<List<object>>
            {
                new List<object>{Terminal.LeftBracket, NonTerminal.Expression, Terminal.RightBracket},
                new List<object>{NonTerminal.BinaryOperatorSubexpression},
                new List<object>{NonTerminal.Operand}
            });

            Rules.Add(NonTerminal.BinaryOperatorSubexpression, new List<List<object>>
            {
                new List<object>{Terminal.BinaryOperator, NonTerminal.Subexpression, NonTerminal.Subexpression }
            });

            Rules.Add(NonTerminal.Operand, new List<List<object>>
            {
                new List<object>{Terminal.Ident},
                new List<object>{Terminal.Const}
            });

            Rules.Add(NonTerminal.ComplexOperator, new List<List<object>>
            {
                new List<object>{NonTerminal.CycleOperator},
                new List<object>{NonTerminal.CompoundOperator}
            });

            Rules.Add(NonTerminal.CycleOperator, new List<List<object>>
            {
                new List<object>{Terminal.While,NonTerminal.Expression, Terminal.Do, NonTerminal.Operator}
            });

            Rules.Add(NonTerminal.CompoundOperator, new List<List<object>>
            {
                new List<object>{Terminal.Begin, NonTerminal.OperatorsList, Terminal.End}
            });
        }
    }
}
