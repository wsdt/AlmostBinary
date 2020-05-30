using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection.Metadata;
using AlmostBinary_Compiler.Tests.utils;

namespace AlmostBinary_Compiler.Tests
{
    [TestClass]
    public class CompileExamples
    {
        [TestMethod]
        public void CompileRepeat() => HelperMethods.Compile(IGlobalConstants.REPEAT);

        [TestMethod]
        public void CompileCall() => HelperMethods.Compile(IGlobalConstants.CALL);

        [TestMethod]
        public void CompileHelloWorld() => HelperMethods.Compile(IGlobalConstants.HELLO_WORLD);

        [TestMethod]
        public void CompileIf() => HelperMethods.Compile(IGlobalConstants.IF);

        [TestMethod]
        public void CompileInput() => HelperMethods.Compile(IGlobalConstants.INPUT);

        [TestMethod]
        public void CompileVariable() => HelperMethods.Compile(IGlobalConstants.VARIABLE);
    }
}
