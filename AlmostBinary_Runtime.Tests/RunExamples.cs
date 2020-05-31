using AlmostBinary_Compiler;
using AlmostBinary_GlobalConstants.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace AlmostBinary_Runtime.Tests
{
    [TestClass]
    public class RunExamples
    {
        /// <summary>
        /// Executes compiled programs async as they are blocking. Most runtime exceptions are thrown within a few seconds. Thus, this
        /// task waits and if no runtime exception is thrown, the test succeeds. Helps to reduce compilation errors, runtime errors might not be caught this way.
        /// </summary>
        /// <param name="fileName"></param>
        private void Run(string fileName)
        {
            var task = Task.Run(() =>
            {
                Program.Main(new string[] { Path.Combine(IGlobalTestConstants.COMPILED_PATH, $"{fileName}.{IGlobalTestConstants.COMPILED_FILE_TYPE}") });
            });
            Assert.IsFalse(task.Wait(IGlobalTestConstants.TIMEOUT));
        }

        [TestMethod]
        public void RunRepeat() => Run(IGlobalTestConstants.REPEAT);

        [TestMethod]
        public void RunCall() => Run(IGlobalTestConstants.CALL);

        [TestMethod]
        public void RunHelloWorld() => Run(IGlobalTestConstants.HELLO_WORLD);

        [TestMethod]
        public void RunIf() => Run(IGlobalTestConstants.IF);

        [TestMethod]
        public void RunInput() => Run(IGlobalTestConstants.INPUT);

        [TestMethod]
        public void RunVariable() => Run(IGlobalTestConstants.VARIABLE);

    }
}
