using AlmostBinary_HelperMethods.Tests;
using AlmostBinary_GlobalConstants.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AlmostBinary_Compiler.Tests
{
    [TestClass]
    public class CompileExamples
    {
        [ClassInitialize()]
        public static void CreateCompiledDirectory(TestContext _) => CompilerTestHelper.createDirectoryIfNecessary(IGlobalTestConstants.COMPILED_PATH);

        [TestMethod]
        public void CompileRepeat() => CompilerTestHelper.Compile(IGlobalTestConstants.REPEAT);

        [TestMethod]
        public void CompileCall() => CompilerTestHelper.Compile(IGlobalTestConstants.CALL);

        [TestMethod]
        public void CompileHelloWorld() => CompilerTestHelper.Compile(IGlobalTestConstants.HELLO_WORLD);

        [TestMethod]
        public void CompileIf() => CompilerTestHelper.Compile(IGlobalTestConstants.IF);

        [TestMethod]
        public void CompileInput() => CompilerTestHelper.Compile(IGlobalTestConstants.INPUT);

        [TestMethod]
        public void CompileVariable() => CompilerTestHelper.Compile(IGlobalTestConstants.VARIABLE);
    }
}
