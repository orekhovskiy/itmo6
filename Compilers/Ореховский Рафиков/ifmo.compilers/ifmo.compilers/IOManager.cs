using System;
using System.Collections.Generic;
using System.Text;

namespace ifmo.compilers
{
    public static class IOManager
    {
        public static string ReadFile(string fileName)
            => System.IO.File.ReadAllText(fileName);
    }
}
