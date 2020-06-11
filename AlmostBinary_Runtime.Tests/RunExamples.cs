using AlmostBinary_GlobalConstants.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        /// <param name="compiledCode">Already compiled code (= content of .wsdt file)</param>
        private void Run(string compiledCode)
        {
            var task = Task.Run(() =>
            {
                AlmostBinary_Runtime.Program.Main(new string[] { "--inline-code", compiledCode });
            });
            Assert.IsFalse(task.Wait(IGlobalTestConstants.TIMEOUT));
        }

        [TestMethod]
        public void RunRepeat() => Run(ICompiledFileConstants.REPEAT);

        [TestMethod]
        public void RunCall() => Run(ICompiledFileConstants.CALL);

        [TestMethod]
        public void RunHelloWorld() => Run(ICompiledFileConstants.HELLO_WORLD);

        [TestMethod]
        public void RunIf() => Run(ICompiledFileConstants.IF);

        [TestMethod]
        public void RunInput() => Run(ICompiledFileConstants.INPUT);

        [TestMethod]
        public void RunVariable() => Run(ICompiledFileConstants.VARIABLE);

    }
}
