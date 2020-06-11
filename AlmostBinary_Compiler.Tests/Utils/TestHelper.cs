using AlmostBinary_GlobalConstants.Tests;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlmostBinary_Compiler.Tests.Utils
{
    public class TestHelper
    {
        private static readonly RegexOptions regOptions = RegexOptions.None;
        private static readonly Regex multipleSpacesRegex = new Regex("[ ]{2,}", regOptions);

        /// <summary>
        /// Bug #4, try to call directly for better testing experience (e.g. exceptions when file not found, etc.)
        /// </summary>
        /// <param name="fileName"></param>
        public static Task Compile(string uncompiledFileName, Action onCompilationFinished = null)
        {
            CreateCompiledDirectory();
            Task compilationTask =  Task.Run(() =>
            {
                AlmostBinary_Compiler.Program.Main(new string[] { 
                    "-v",
                    "-f", Path.Combine(IGlobalTestConstants.EXAMPLES_PATH, $"{uncompiledFileName}.{IGlobalTestConstants.UNCOMPILED_FILE_TYPE}"), 
                    "-o", IGlobalTestConstants.COMPILED_PATH });
                onCompilationFinished?.Invoke();
            });
            compilationTask.ConfigureAwait(true);
            return compilationTask;
        }

        public static void Compile_bak(string uncompiledFileName, Action onCompilationFinished = null)
        {
            Process compiler = Process.Start(IGlobalTestConstants.COMPILER_EXE_PATH,
               Path.Combine(IGlobalTestConstants.EXAMPLES_PATH, $"{uncompiledFileName}.{IGlobalTestConstants.UNCOMPILED_FILE_TYPE}"));
            compiler.CloseMainWindow();
            compiler.Close();
            onCompilationFinished();
        }

        /// <summary>
        /// Simple wrapper method for readability
        /// </summary>
        private static void CreateDirectoryIfNecessary(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static void CreateCompiledDirectory() => CreateDirectoryIfNecessary(IGlobalTestConstants.COMPILED_PATH);

        /// <summary>
        /// Compiles specified example file
        /// </summary>
        /// <param name="fileName">File name without file-extension.</param>
        public static void Compile_bak(string fileName)
        {
            Console.WriteLine("##### " + IGlobalTestConstants.COMPILER_EXE_PATH + " / " + File.Exists(IGlobalTestConstants.COMPILER_EXE_PATH) + " / "
                + Path.Combine(IGlobalTestConstants.EXAMPLES_PATH, $"{fileName}.{IGlobalTestConstants.UNCOMPILED_FILE_TYPE}") + " / " + File.Exists(Path.Combine(IGlobalTestConstants.EXAMPLES_PATH, $"{fileName}.{IGlobalTestConstants.UNCOMPILED_FILE_TYPE}")));

            Process compiler = Process.Start(IGlobalTestConstants.COMPILER_EXE_PATH,
                Path.Combine(IGlobalTestConstants.EXAMPLES_PATH, $"{fileName}.{IGlobalTestConstants.UNCOMPILED_FILE_TYPE}"));
            compiler.CloseMainWindow();
            compiler.Close();
        }
        public static string TrimReplaceAll(string code) => multipleSpacesRegex.Replace(code.Replace("\n", " ").Replace("\r", " "), " ");

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

        public static string ReadFile(string file)
        {
            FileStream fs = new FileStream(file, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            return br.ReadString();
        }
    }
}
