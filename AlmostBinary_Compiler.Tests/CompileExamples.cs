using AlmostBinary_HelperMethods.Tests;
using AlmostBinary_GlobalConstants.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using AlmostBinary_Runtime.Tests;

namespace AlmostBinary_Compiler.Tests
{
    [TestClass]
    public class CompileExamples
    {
        /// <summary>
        /// Bug #4, try to call directly for better testing experience (e.g. exceptions when file not found, etc.)
        /// </summary>
        /// <param name="fileName"></param>
        public static void Compile(string uncompiledCode) => AlmostBinary_Compiler.Program.Main(new string[] { "--inline-code", uncompiledCode });

        [TestMethod]
        public void CompileRepeat() => Compile(IUncompiledFileConstants.REPEAT);

        [TestMethod]
        public void CompileCall() => Compile(IUncompiledFileConstants.CALL);

        [TestMethod]
        public void CompileHelloWorld() => Compile(IUncompiledFileConstants.HELLO_WORLD);

        [TestMethod]
        public void CompileIf() => Compile(IUncompiledFileConstants.IF);

        [TestMethod]
        public void CompileInput() => Compile(IUncompiledFileConstants.INPUT);

        [TestMethod]
        public void CompileVariable() => Compile(IUncompiledFileConstants.VARIABLE);
    }
}
