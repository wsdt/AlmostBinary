using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection.Metadata;

namespace AlmostBinary_Compiler.Tests
{
    [TestClass]
    public class CompileExamples
    {
        private readonly string EXAMPLES_PATH = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "examples");
        private const string INPUT_FILE_TYPE = "abin";

        [TestMethod]
        public void CompileRepeat() => Program.Main(new string[] { Path.Combine(EXAMPLES_PATH, $"repeat.{INPUT_FILE_TYPE}")});

        [TestMethod]
        public void CompileCall() => Program.Main(new string[] { Path.Combine(EXAMPLES_PATH, $"call.{INPUT_FILE_TYPE}") });

        [TestMethod]
        public void CompileHelloWorld() => Program.Main(new string[] { Path.Combine(EXAMPLES_PATH, $"hello world.{INPUT_FILE_TYPE}") });

        [TestMethod]
        public void CompileIf() => Program.Main(new string[] { Path.Combine(EXAMPLES_PATH, $"if.{INPUT_FILE_TYPE}") });

        [TestMethod]
        public void CompileInput() => Program.Main(new string[] { Path.Combine(EXAMPLES_PATH, $"input.{INPUT_FILE_TYPE}") });

        [TestMethod]
        public void CompileVariable() => Program.Main(new string[] { Path.Combine(EXAMPLES_PATH, $"variable.{INPUT_FILE_TYPE}") });
    }
}
