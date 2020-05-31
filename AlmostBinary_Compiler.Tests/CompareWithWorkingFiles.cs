using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using AlmostBinary_Compiler.Tests.utils;
using AlmostBinary_GlobalConstants.Test;

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
            fileName += $".{IGlobalTestConstants.COMPILED_FILE_TYPE}";
            Assert.AreEqual(
                ReadFile(Path.Combine(IGlobalTestConstants.COMPILED_PATH, fileName)),
                ReadFile(Path.Combine(IGlobalTestConstants.WORKING_PATH, fileName)));
        }


        /// <summary>
        /// Files are compiled again to avoid any dependency on other test-files. 
        /// Possible alternative if performance is a concern: Add tests to playlist to ensure correct execution order.
        /// </summary>
        [ClassInitialize()]
        public static void CompileAllFiles(TestContext _) => HelperMethods.CompileAll();

        [TestMethod]
        public void CompareRepeat() => Compare(IGlobalTestConstants.REPEAT);

        [TestMethod]
        public void CompareCall() => Compare(IGlobalTestConstants.CALL);

        [TestMethod]
        public void CompareHelloWorld() => Compare(IGlobalTestConstants.HELLO_WORLD);

        [TestMethod]
        public void CompareIf() => Compare(IGlobalTestConstants.IF);

        [TestMethod]
        public void CompareInput() => Compare(IGlobalTestConstants.INPUT);

        [TestMethod]
        public void CompareVariable() => Compare(IGlobalTestConstants.VARIABLE);
    }
}
