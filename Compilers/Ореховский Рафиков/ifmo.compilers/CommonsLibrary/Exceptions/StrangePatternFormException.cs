using System;

namespace CommonsLibrary.Exceptions
{
    [Serializable]
    public class StrangePatternFormException : Exception
    {
        public StrangePatternFormException() : base("Wrong pattern composition") { }
    }
}
