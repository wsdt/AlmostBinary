using AlmostBinary_GlobalConstants.Tests;
using System.Threading.Tasks;
using Xunit;

namespace AlmostBinary_Runtime.Tests
{
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
            Assert.False(task.Wait(IGlobalTestConstants.TIMEOUT));
        }

        [Fact]
        public void RunRepeat() => Run(ICompiledFileConstants.REPEAT);

        [Fact]
        public void RunCall() => Run(ICompiledFileConstants.CALL);

        [Fact]
        public void RunHelloWorld() => Run(ICompiledFileConstants.HELLO_WORLD);

        [Fact]
        public void RunIf() => Run(ICompiledFileConstants.IF);

        [Fact]
        public void RunInput() => Run(ICompiledFileConstants.INPUT);

        [Fact]
        public void RunVariable() => Run(ICompiledFileConstants.VARIABLE);

    }
}
