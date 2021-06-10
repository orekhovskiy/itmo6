using System;

namespace CommonsLibrary.Exceptions
{
    [Serializable]
    public class NoSuitableParseTreeException : Exception
    {
        public NoSuitableParseTreeException() : base("No suitable parse tree exists") { }
    }
}
