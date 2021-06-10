using CommonsLibrary;
using CommonsLibrary.Exceptions;
using SyntaxAnalysisLibray.Lexer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows.Markup;
using Terminal = SyntaxAnalysisLibray.Lexer.TokenType;

namespace SyntaxAnalysisLibray.Parser
{
    public static class Parser
    {
        private static List<Token> s_tokens;
        private static Token s_token;
        private static int s_tokensIterator;
        private static Stack<int> s_tokensIteratorStack;

        public static AbstractSyntaxTree Parse(List<Token> tokens)
        {
            PrepareToRead(tokens);
            var result = GetSymbol(NonTerminal.Root);
            if (result.Success)
            {
                var element = (AbstractSyntaxTree)result.Value;
                return element;
            }
            else
            {
                throw new NoSuitableParseTreeException();
            }
        }

        private static Result GetSymbol(object symbol)
        {
            Result result = default;
            switch (symbol)
            {
                case Terminal terminal:
                    NextToken();
                    result = GetTerminal(terminal);
                    break;
                case NonTerminal nonTerminal:
                    result = GetNonTerminal(nonTerminal);
                    break;
            }
            string value = default;
            List<AbstractSyntaxTree> childNodes = default;
            switch (result.Success, symbol)
            {
                case (false, _):
                    return result;

                case (_, Terminal.Ident):
                case (_, Terminal.Const):
                    return new Result(true, new AbstractSyntaxTree(result.Value.ToString()));

                case (_, NonTerminal.Root):
                    value = "Program";
                    childNodes = ((List<AbstractSyntaxTree>)result.Value);
                    return new Result(true, new AbstractSyntaxTree(value, childNodes));

                case (_, NonTerminal.VariableDeclaration):
                    value = "Variable declaration";
                    childNodes = ((List<AbstractSyntaxTree>)result.Value).Where((val, ident) => ident % 2 != 0).ToList();
                    return new Result(true, new AbstractSyntaxTree(value, childNodes));

                case (_, NonTerminal.Assignment):
                    value = "Assignment";
                    childNodes = new List<AbstractSyntaxTree>
                    {
                        ((List<AbstractSyntaxTree>)result.Value)[0]
                    };
                    childNodes.AddRange(((List<AbstractSyntaxTree>)result.Value).Skip(2).ToList());
                    return new Result(true, new AbstractSyntaxTree(value, childNodes));

                case (_, NonTerminal.CycleOperator):
                    value = "While";
                    childNodes = new List<AbstractSyntaxTree>
                    {
                        ((List<AbstractSyntaxTree>)result.Value)[1],
                        ((List<AbstractSyntaxTree>)result.Value)[3]
                    };
                    return new Result(true, new AbstractSyntaxTree(value, childNodes));

                case (_, NonTerminal.CompoundOperator):
                    value = "Compound operator";
                    childNodes = ((List<AbstractSyntaxTree>)result.Value).Skip(1).ToList();
                    childNodes.RemoveAt(childNodes.Count - 1);
                    return new Result(true, new AbstractSyntaxTree(value, childNodes));

                case (_, NonTerminal.BinaryOperatorSubexpression):
                    value = ((List<AbstractSyntaxTree>)result.Value).First().Value;
                    childNodes = ((List<AbstractSyntaxTree>)result.Value).Skip(1).ToList();
                    return new Result(true, new AbstractSyntaxTree(value, childNodes));

                default:
                    return result;
            }
        }

        private static Result GetTerminal(Terminal requestedSymbol)
        {
            if (requestedSymbol == s_token.Type)
            {
                return new Result(true, s_token.Content);
            }
            else
            {
                return new Result(false);
            }
        }

        private static Result GetNonTerminal(NonTerminal requestedSymbol)
        {
            var productions = Grammar.Rules[requestedSymbol];
            foreach (var production in productions)
            {
                SavePosition();
                var productionMatches = true;
                var elements = new List<AbstractSyntaxTree>();
                foreach (var symbol in production)
                {
                    var result = GetSymbol(symbol);
                    if (!result.Success)
                    {
                        productionMatches = false;
                        break;
                    }
                    switch (result.Value)
                    {
                        case "":
                        case ".":
                            continue;
                        case List<AbstractSyntaxTree> list:
                            elements.AddRange(list);
                            break;
                        case AbstractSyntaxTree node:
                            elements.Add(node);
                            break;
                        case string tokenContent:
                            elements.Add(new AbstractSyntaxTree(tokenContent));
                            break;
                        default:
                            throw new Exception("Unexpected tree node");
                    }
                }
                if (productionMatches)
                {
                    return new Result(true, elements);
                }
                RestorePosition();
            }
            return new Result(false);
        }

        private static void PrepareToRead(List<Token> tokens)
        {
            s_tokens = tokens;
            s_tokensIterator = 0;
            s_tokensIteratorStack = new Stack<int>();
        }

        private static void SavePosition()
        {
            s_tokensIteratorStack.Push(s_tokensIterator);
        }

        private static void RestorePosition()
        {
            s_tokensIterator = s_tokensIteratorStack.Pop();
            s_token = s_tokens[s_tokensIterator];
        }

        private static void NextToken()
        {
            s_token = s_tokens[s_tokensIterator];
            s_tokensIterator++;
        }
    }
}
