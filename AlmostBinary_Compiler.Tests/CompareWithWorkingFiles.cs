
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Reflection.Metadata;
using AlmostBinary_Compiler.Tests;
using AlmostBinary_Compiler.Tests.utils;

namespace AlmostBinary_Compiler.Tests
{
    [TestClass]
    public class CompareWithWorkingFiles
    {
        private string ReadFile(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            return br.ReadString();
        }

        private void Compare(string fileName)
        {
            fileName += $".{IGlobalConstants.COMPILED_FILE_TYPE}";
            Assert.AreEqual(
                ReadFile(Path.Combine(IGlobalConstants.COMPILED_PATH, fileName)),
                ReadFile(Path.Combine(IGlobalConstants.WORKING_PATH, fileName)));
        }


        /// <summary>
        /// Files are compiled again to avoid any dependency on other test-files. 
        /// Possible alternative if performance is a concern: Add tests to playlist to ensure correct execution order.
        /// </summary>
        [ClassInitialize()]
        public static void CompileAllFiles(TestContext _) => HelperMethods.CompileAll();

        [TestMethod]
        public void CompareRepeat() => Compare(IGlobalConstants.REPEAT);

        [TestMethod]
        public void CompareCall() => Compare(IGlobalConstants.CALL);

        [TestMethod]
        public void CompareHelloWorld() => Compare(IGlobalConstants.HELLO_WORLD);

        [TestMethod]
        public void CompareIf() => Compare(IGlobalConstants.IF);

        [TestMethod]
        public void CompareInput() => Compare(IGlobalConstants.INPUT);

        [TestMethod]
        public void CompareVariable() => Compare(IGlobalConstants.VARIABLE);
    }
}
