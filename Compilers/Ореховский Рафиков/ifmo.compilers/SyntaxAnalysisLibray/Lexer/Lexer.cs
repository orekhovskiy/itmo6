using CommonsLibrary;
using CommonsLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SyntaxAnalysisLibray.Lexer
{
    public static class Lexer
    {
        public static List<Token> Tokenize(string str)
        {
            var tokens = new List<Token>();
            for (var i = 0; i < str.Length; i++)
            {
                var substring = str.Substring(i).Split((char[])null, StringSplitOptions.RemoveEmptyEntries)[0];
                var result = GetToken(substring);
                if (result.Success)
                {
                    var token = (Token)result.Value;
                    tokens.Add(token);
                    i += CompinsateEmptyEntries(str.Substring(i), token.Content);
                    i += token.Content.Length - 1;

                }
                else
                {
                    throw new NoSuitableTokenException(str.Substring(i));
                }
            }
            tokens.Add(new Token(TokenType.EOF, ""));
            return tokens;
        }
        
        private static Result GetToken(string str)
        {
            foreach (var (tokenType, tokenDefinition) in Grammar.TokenDefinitions)
            {
                var match = (new Regex(tokenDefinition)).Match(str);
                if (match.Success)
                {
                    return new Result(true, new Token(tokenType, match.Value));
                }
            }
            return new Result(false);
        }

        private static int CompinsateEmptyEntries(string str, string token)
        {
            var i = 0;
            while(str[i] == ' ' || str[i] == '\t')
            {
                i++;
            }
            if (str.Length >= i + token.Length + 2)
            {
                var substr = str.Substring(i + token.Length, 2);
                return substr == "\r\n" ? i + 2 : i;
            }
            else
            {
                return i;
            }
        }
    }
}
