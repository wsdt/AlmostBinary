using AlmostBinary_GlobalConstants.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;

namespace AlmostBinary_HelperMethods.Tests
{
    public sealed class RuntimeTestHelper
    {
        /// <summary>
        /// Executes compiled programs async as they are blocking. Most runtime exceptions are thrown within a few seconds. Thus, this
        /// task waits and if no runtime exception is thrown, the test succeeds. Helps to reduce compilation errors, runtime errors might not be caught this way.
        /// </summary>
        /// <param name="fileName"></param>
        public static void Run(string fileName)
        {
            var task = Task.Run(() =>
            {
                string file = Path.Combine(IGlobalTestConstants.COMPILED_PATH, $"{fileName}.{IGlobalTestConstants.COMPILED_FILE_TYPE}");
                if (!File.Exists(file)) {
                    CompilerTestHelper.Compile(Path.GetFileNameWithoutExtension(file));
                }

                AlmostBinary_Runtime.Program.Main(new string[] { file });
            });
            Assert.IsFalse(task.Wait(IGlobalTestConstants.TIMEOUT));
        }
    }
}
