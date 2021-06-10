using System;

namespace CommonsLibrary.Exceptions
{
    [Serializable]
    public class StrangeCharacterException : Exception
    {
        public StrangeCharacterException(char character) : base($"Non-alphabetical character '{character}' met") { }
    }
}
