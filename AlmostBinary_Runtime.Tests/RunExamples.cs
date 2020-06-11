using AlmostBinary_GlobalConstants.Tests;
using System;
using System.IO;
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
        /// <param name="fileName">Already compiled code (= content of .wsdt file)</param>
        private void Run(string fileName)
        {
            AssertAsync.CompletesIn(IGlobalTestConstants.TIMEOUT_IN_SECONDS, () =>
            {
                string path = Path.Combine(IGlobalTestConstants.EXAMPLES_PATH, $"{fileName}.{IGlobalTestConstants.COMPILED_FILE_TYPE}");
                AlmostBinary_Runtime.Program.Main(new string[] { Path.Combine(IGlobalTestConstants.COMPILED_PATH, $"{fileName}.{IGlobalTestConstants.COMPILED_FILE_TYPE}") });
            });

            //var task = Task.Run(() =>
            //{
            //    AlmostBinary_Runtime.Program.Main(new string[] { "--inline-code", compiledCode });
            //});
            //Assert.False(task.Wait(IGlobalTestConstants.TIMEOUT));
        }

        [Fact]
        public void RunRepeat() => Run(IGlobalTestConstants.REPEAT);

        [Fact]
        public void RunCall() => Run(IGlobalTestConstants.CALL);

        [Fact]
        public void RunHelloWorld() => Run(IGlobalTestConstants.HELLO_WORLD);

        [Fact]
        public void RunIf() => Run(IGlobalTestConstants.IF);

        [Fact]
        public void RunInput() => Run(IGlobalTestConstants.INPUT);

        [Fact]
        public void RunVariable() => Run(IGlobalTestConstants.VARIABLE);

    }

    public static class AssertAsync
    {
        public static void CompletesIn(int timeout, Action action)
        {
            var task = Task.Run(action);
            var completedInTime = Task.WaitAll(new[] { task }, TimeSpan.FromSeconds(timeout));

            if (task.Exception != null)
            {
                if (task.Exception.InnerExceptions.Count == 1)
                {
                    throw task.Exception.InnerExceptions[0];
                }

                throw task.Exception;
            }

            if (completedInTime)
            {
                throw new TimeoutException($"Task did complete in {timeout} seconds. Abin-files should block.");
            }
        }
    }
}
