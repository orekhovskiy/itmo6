using System;
using System.Collections.Generic;
using System.Text;

namespace CommonsLibrary
{
    public struct Result
    {
        public bool Success { get; }
        public object Value { get; }

        public Result(bool success)
        {
            Success = success;
            Value = null;
        }

        public Result(bool success, object value)
        {
            Success = success;
            Value = value;
        }
    }
}
