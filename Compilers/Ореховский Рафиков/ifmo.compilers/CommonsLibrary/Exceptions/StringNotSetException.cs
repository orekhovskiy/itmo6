using System;

namespace CommonsLibrary.Exceptions
{
    [Serializable]
    public class StringNotSetException : Exception
    {
        private static readonly string s_message = "No defined string exists yet";

        public StringNotSetException() : base(s_message) { }
    }
}
