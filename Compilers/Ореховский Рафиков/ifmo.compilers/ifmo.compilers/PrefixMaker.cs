using SyntaxAnalysisLibray.Lexer;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using CommonsLibrary.Exceptions;

namespace ifmo.compilers
{
    public static class PrefixMaker
    {
        public static List<Token> MakePrefix(List<Token> tokens)
        {
            var prefixTokens = new List<Token>();
            var expressionSubset = new List<Token>();
            foreach (var token in tokens)
            {
                if (IsExpressionPart(token))
                {
                    expressionSubset.Add(token);
                } 
                else
                {
                    // If token is <EqualSign> postfix form doesn't needed
                    // This separates expressions as well
                    if (token.Type == TokenType.EqualSign && expressionSubset.Count >= 1)
                    {
                        var preEqualToken = expressionSubset.ElementAt(expressionSubset.Count - 1);
                        expressionSubset.RemoveAt(expressionSubset.Count - 1);
                        prefixTokens.AddRange(MakePrefixExpression(expressionSubset));
                        prefixTokens.Add(preEqualToken);
                        expressionSubset = new List<Token>();
                        prefixTokens.Add(token);
                    }
                    else
                    {
                        prefixTokens.AddRange(MakePrefixExpression(expressionSubset));
                        expressionSubset = new List<Token>();
                        prefixTokens.Add(token);
                    }
                }
            }
            prefixTokens.AddRange(MakePrefixExpression(expressionSubset));
            return prefixTokens;
        }

        private static List<Token> MakePrefixExpression(List<Token> tokens)
        {
            // Just an <Operand>
            // ex: a
            if (tokens.Count < 2)
            {
                return tokens;
            }

            // Expression is <UnaryOperator> <Operand>
            // ex: -5
            if (tokens.Count == 2 && tokens[0].Type == TokenType.UnaryOperator && 
                (tokens[1].Type == TokenType.Ident || tokens[1].Type == TokenType.Const))
            {
                return tokens;
            }

            // Expression is complex
            // ex: a + b
            var prefixExpression = new List<Token>();
            var operationStack = new Stack<Token>();
            foreach (var token in tokens)
            {
                switch (token.Type)
                {
                    case TokenType.Const:
                    case TokenType.Ident:
                        prefixExpression.Add(token);
                        break;
                    case TokenType.LeftBracket:
                        operationStack.Push(token);
                        break;
                    case TokenType.RightBracket:
                        if (!operationStack.Any())
                        {
                            throw new NoSuitableParseTreeException();
                        }
                        while (operationStack.Peek().Type != TokenType.LeftBracket)
                        {
                            prefixExpression.Add(operationStack.Pop());
                        }
                        operationStack.Pop();
                        break;
                    default:
                        if (operationStack.Any() && operationStack.Peek().Type == TokenType.LeftBracket)
                        {
                            operationStack.Push(token);
                        }
                        else  if (operationStack.Any() && GetOperationPriority(operationStack.Peek()) >= GetOperationPriority(token))
                        {
                            prefixExpression.Add(operationStack.Pop());
                            operationStack.Push(token);
                        }
                        else
                        {
                            operationStack.Push(token);
                        }
                        break;
                }
            }
            while (operationStack.Any())
            {
                prefixExpression.Add(operationStack.Pop());
            }
            prefixExpression.Reverse();
            return prefixExpression;
        }

        private static bool IsExpressionPart(Token token)
            => token.Type == TokenType.Const || token.Type == TokenType.Ident || token.Type == TokenType.LeftBracket || 
               token.Type == TokenType.RightBracket || token.Type == TokenType.BinaryOperator || token.Type == TokenType.UnaryOperator;
        
        private static int GetOperationPriority(Token token)
        {
            switch(token.Content)
            {
                case "*":
                case "/":
                    return 3;
                case "+":
                case "-":
                    return 2;
                default:
                    return 1;
            }
        }
    }
}
