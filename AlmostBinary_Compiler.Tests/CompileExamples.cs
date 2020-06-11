using AlmostBinary_GlobalConstants.Tests;
using System.IO;
using AlmostBinary_Runtime.Tests;
using System.Threading.Tasks;
using Xunit;

namespace AlmostBinary_Compiler.Tests
{
    public class CompileExamples
    {
        /// <summary>
        /// Bug #4, try to call directly for better testing experience (e.g. exceptions when file not found, etc.)
        /// </summary>
        /// <param name="fileName"></param>
        public static void Compile(string uncompiledFile)
        {
            CreateCompiledDirectory();
            var task = Task.Run(() =>
            {
                AlmostBinary_Compiler.Program.Main(new string[] { uncompiledFile });
            });
        }

        /// <summary>
        /// Simple wrapper method for readability
        /// </summary>
        public static void createDirectoryIfNecessary(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void CreateCompiledDirectory() => createDirectoryIfNecessary(IGlobalTestConstants.COMPILED_PATH);

        [Fact]
        public void CompileRepeat() => Compile(IGlobalTestConstants.REPEAT);

        [Fact]
        public void CompileCall() => Compile(IGlobalTestConstants.CALL);

        [Fact]
        public void CompileHelloWorld() => Compile(IGlobalTestConstants.HELLO_WORLD);

        [Fact]
        public void CompileIf() => Compile(IGlobalTestConstants.IF);

        [Fact]
        public void CompileInput() => Compile(IGlobalTestConstants.INPUT);

        [Fact]
        public void CompileVariable() => Compile(IGlobalTestConstants.VARIABLE);
    }
}
