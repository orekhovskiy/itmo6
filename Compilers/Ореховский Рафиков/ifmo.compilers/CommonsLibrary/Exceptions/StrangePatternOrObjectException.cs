using System;

namespace CommonsLibrary.Exceptions
{
    [Serializable]
    public class StrangePatternOrObjectException : Exception
    {
        private static readonly string s_message = "Unexpected pattern or object";

        public StrangePatternOrObjectException() : base(s_message) { }
    }
}
