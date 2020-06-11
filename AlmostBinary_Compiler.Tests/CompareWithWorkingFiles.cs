using AlmostBinary_GlobalConstants.Tests;
using System.IO;
using System;
using System.Diagnostics;
using Xunit;
using System.Text.RegularExpressions;

namespace AlmostBinary_Compiler.Tests
{
    //WORKING LOCALLY, BUT CONVERT TO INTEGRATION TEST TO GET IT TO WORK IN AUTOMATED TESTS

    public class CompareWithWorkingFiles
    {
        private static RegexOptions regOptions = RegexOptions.None;
        private static Regex multipleSpacesRegex = new Regex("[ ]{2,}", regOptions);

        private string ReadFile(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            return br.ReadString();
        }

        private void Compare(string fileName)
        {
            CompilerTestHelper.Compile(fileName);
            fileName += $".{IGlobalTestConstants.COMPILED_FILE_TYPE}";
            Assert.Equal(
                TrimReplaceAll(ReadFile(Path.Combine(IGlobalTestConstants.COMPILED_PATH, fileName))),
                TrimReplaceAll(ReadFile(Path.Combine(IGlobalTestConstants.WORKING_PATH, fileName))));
        }

        public static string TrimReplaceAll(string code)
        {
            return multipleSpacesRegex.Replace(code.Replace("\n", " ").Replace("\r", " "), " ");
        }

        /// <summary>
        /// Files are compiled again to avoid any dependency on other test-files. 
        /// Possible alternative if performance is a concern: Add tests to playlist to ensure correct execution order.
        /// </summary>
        public static void CompileAllFiles()
        {
            CompilerTestHelper.createDirectoryIfNecessary(IGlobalTestConstants.COMPILED_PATH);
            CompilerTestHelper.CompileAll();
        }

        [Fact]
        public void CompareRepeat() => Compare(IGlobalTestConstants.REPEAT);

        [Fact]
        public void CompareCall() => Compare(IGlobalTestConstants.CALL);

        [Fact]
        public void CompareHelloWorld() => Compare(IGlobalTestConstants.HELLO_WORLD);

        [Fact]
        public void CompareIf() => Compare(IGlobalTestConstants.IF);

        [Fact]
        public void CompareInput() => Compare(IGlobalTestConstants.INPUT);

        [Fact]
        public void CompareVariable() => Compare(IGlobalTestConstants.VARIABLE);


        public static class CompilerTestHelper
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
                Console.WriteLine("##### " + IGlobalTestConstants.COMPILER_EXE_PATH + " / " + File.Exists(IGlobalTestConstants.COMPILER_EXE_PATH) + " / "
                    + Path.Combine(IGlobalTestConstants.EXAMPLES_PATH, $"{fileName}.{IGlobalTestConstants.UNCOMPILED_FILE_TYPE}") + " / " + File.Exists(Path.Combine(IGlobalTestConstants.EXAMPLES_PATH, $"{fileName}.{IGlobalTestConstants.UNCOMPILED_FILE_TYPE}")));

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
        }
    }
}