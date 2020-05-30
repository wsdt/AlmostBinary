using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Text;
using System.Diagnostics;

namespace AlmostBinary_Compiler.Tests.utils
{
    public sealed class HelperMethods
    {
        /// <summary>
        /// Bug #4, try to call directly for better testing experience (e.g. exceptions when file not found, etc.)
        /// </summary>
        /// <param name="fileName"></param>
        public static void Compile_bak(string fileName) => Program.Main(new string[] { Path.Combine(IGlobalConstants.EXAMPLES_PATH, $"{fileName}.{IGlobalConstants.UNCOMPILED_FILE_TYPE}") });

        /// <summary>
        /// Compiles specified example file
        /// </summary>
        /// <param name="fileName">File name without file-extension.</param>
        public static void Compile(string fileName)
        {
            Process compiler = Process.Start(IGlobalConstants.COMPILER_EXE_PATH,
                Path.Combine(IGlobalConstants.EXAMPLES_PATH, $"{fileName}.{IGlobalConstants.UNCOMPILED_FILE_TYPE}"));
            compiler.CloseMainWindow();
            compiler.Close();
        }

        /// <summary>
        /// Compiles all example files
        /// </summary>
        public static void CompileAll()
        {
            foreach (string fileName in IGlobalConstants.ALL_FILENAMES)
            {
                Compile(fileName);
            }
        }
    }
}
