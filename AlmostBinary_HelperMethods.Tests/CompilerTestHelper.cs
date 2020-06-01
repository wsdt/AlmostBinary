using AlmostBinary_GlobalConstants.Test;
using System.Diagnostics;
using System.IO;

namespace AlmostBinary_HelperMethods.Tests
{
    public sealed class CompilerTestHelper
    {
        /// <summary>
        /// Bug #4, try to call directly for better testing experience (e.g. exceptions when file not found, etc.)
        /// </summary>
        /// <param name="fileName"></param>
        public static void Compile_bak(string fileName) => AlmostBinary_Compiler.Program.Main(new string[] { Path.Combine(IGlobalTestConstants.EXAMPLES_PATH, $"{fileName}.{IGlobalTestConstants.UNCOMPILED_FILE_TYPE}") });

        /// <summary>
        /// Compiles specified example file
        /// </summary>
        /// <param name="fileName">File name without file-extension.</param>
        public static void Compile(string fileName)
        {
            Process compiler = Process.Start(IGlobalTestConstants.COMPILER_EXE_PATH,
                Path.Combine(IGlobalTestConstants.EXAMPLES_PATH, $"{fileName}.{IGlobalTestConstants.UNCOMPILED_FILE_TYPE}"));
            compiler.CloseMainWindow();
            compiler.Close();
        }

        /// <summary>
        /// Compiles all example files
        /// </summary>
        public static void CompileAll()
        {
            foreach (string fileName in IGlobalTestConstants.ALL_FILENAMES)
            {
                Compile(fileName);
            }
        }
    }
}
