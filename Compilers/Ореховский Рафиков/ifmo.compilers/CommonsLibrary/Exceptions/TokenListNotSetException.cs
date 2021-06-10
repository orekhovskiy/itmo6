using System;

namespace CommonsLibrary.Exceptions
{
    [Serializable]
    public class TokenListNotSetException : Exception
    {
        private static readonly string s_message = "No token list exists yet";

        public TokenListNotSetException() : base(s_message) { }
    }
}
