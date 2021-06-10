using System;

namespace CommonsLibrary.Exceptions
{
    [Serializable]
    public class NoSuitableTokenException : Exception
    {
        public NoSuitableTokenException(string substring) : base($"No suitable token exists for substring {substring}") { }
    }
}
