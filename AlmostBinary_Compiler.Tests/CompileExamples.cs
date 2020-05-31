using Microsoft.VisualStudio.TestTools.UnitTesting;
using AlmostBinary_Compiler.Tests.utils;
using AlmostBinary_GlobalConstants.Test;

namespace AlmostBinary_Compiler.Tests
{
    [TestClass]
    public class CompileExamples
    {
        [TestMethod]
        public void CompileRepeat() => HelperMethods.Compile(IGlobalTestConstants.REPEAT);

        [TestMethod]
        public void CompileCall() => HelperMethods.Compile(IGlobalTestConstants.CALL);

        [TestMethod]
        public void CompileHelloWorld() => HelperMethods.Compile(IGlobalTestConstants.HELLO_WORLD);

        [TestMethod]
        public void CompileIf() => HelperMethods.Compile(IGlobalTestConstants.IF);

        [TestMethod]
        public void CompileInput() => HelperMethods.Compile(IGlobalTestConstants.INPUT);

        [TestMethod]
        public void CompileVariable() => HelperMethods.Compile(IGlobalTestConstants.VARIABLE);
    }
}
