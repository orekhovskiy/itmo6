using NUnit.Framework;
using SyntaxAnalysisLibray.Lexer;
using ifmo.compilers;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;

namespace ifmo.compilers.Tests.PostfixMakerTests
{
    [TestFixture]
    class MakePostfixTests
    {
        [Test]
        public void MakePostfixTest1()
        {
            var code = "(a+b)";
            var tokenizedCode = Lexer.Tokenize(code);
            var postfixizedCode = PrefixMaker.MakePostfix(tokenizedCode);

        }

        [Test]
        public void MakePostfixTest2()
        {
            var file = "pseudocode.txt";
            var code = IOManager.ReadFile(file);
            var splited = code.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            var tokenizedCode = Lexer.Tokenize(code);
            var postfixizedCode = PrefixMaker.MakePostfix(tokenizedCode);

        }
    }
}
