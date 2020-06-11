using AlmostBinary_GlobalConstants.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AlmostBinary_Runtime.Tests.utils
{
    public class TestHelper
    {
        /// <summary>
        /// Executes compiled programs async as they are blocking. Most runtime exceptions are thrown within a few seconds. Thus, this
        /// task waits and if no runtime exception is thrown, the test succeeds. Helps to reduce compilation errors, runtime errors might not be caught this way.
        /// </summary>
        /// <param name="fileName">Already compiled code (= content of .wsdt file)</param>
        //public static void Run(string fileName)
        //{
        //    Task task = Task.Run(async () =>
        //    {
        //        await AlmostBinary_Compiler.Tests.Utils.TestHelper.Compile(fileName).ContinueWith((compilationTask) =>
        //        {
        //            AlmostBinary_Runtime.Program.Main(new string[] { "-f", Path.Combine(IGlobalTestConstants.COMPILED_PATH, $"{fileName}.{IGlobalTestConstants.COMPILED_FILE_TYPE}") });
        //        });
        //    });
        //    Assert.False(task.Wait(IGlobalTestConstants.TIMEOUT_IN_MS));
        //}

        public static void Run(string fileName)
        {
            Task task = Task.Run(async () =>
            {
                await AlmostBinary_Compiler.Tests.Utils.TestHelper.Compile(fileName, () => {
                    AlmostBinary_Runtime.Program.Main(new string[] { "-f", Path.Combine(IGlobalTestConstants.COMPILED_PATH, $"{fileName}.{IGlobalTestConstants.COMPILED_FILE_TYPE}") });
                }); ;
            });
            Assert.False(task.Wait(IGlobalTestConstants.TIMEOUT_IN_MS));
        }
    }
}
