using AlmostBinary_Compiler;
using AlmostBinary_Compiler.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AlmostBinary_Runtime.Tests
{
    [TestClass]
    public class RunExamples
    {
        private readonly static string COMPILED_PATH = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "compiled");
        private const string INPUT_FILE_TYPE = "wsdt";
        private const int TIMEOUT = 3000;

        /// <summary>
        /// Executes compiled programs async as they are blocking. Most runtime exceptions are thrown within a few seconds. Thus, this
        /// task waits and if no runtime exception is thrown, the test succeeds. Helps to reduce compilation errors, runtime errors might not be caught this way.
        /// </summary>
        /// <param name="fileName"></param>
        private void Run(string fileName)
        {
            var task = Task.Run(() =>
            {
                Program.Main(new string[] { Path.Combine(COMPILED_PATH, $"{fileName}.{INPUT_FILE_TYPE}") });
            });
            Assert.IsFalse(task.Wait(TIMEOUT));
        }

        [TestMethod]
        public void RunRepeat() => Run(IGlobalConstants.REPEAT);

        [TestMethod]
        public void RunCall() => Run(IGlobalConstants.CALL);

        [TestMethod]
        public void RunHelloWorld() => Run(IGlobalConstants.HELLO_WORLD);

        [TestMethod]
        public void RunIf() => Run(IGlobalConstants.IF);

        [TestMethod]
        public void RunInput() => Run(IGlobalConstants.INPUT);

        [TestMethod]
        public void RunVariable() => Run(IGlobalConstants.VARIABLE);

    }
}
