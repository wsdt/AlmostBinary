﻿using System.IO;

namespace AlmostBinary_GlobalConstants.Tests
{
    // ALTHOUGH NOT NEEDED AT THE MOMENT, BUT KEEP FOR LATER INTEGRATION TESTS!


    /// <summary>
    /// Global test constants
    /// </summary>
    public interface IGlobalTestConstants
    {
        public const int TIMEOUT_IN_MS = 3000;

        /* Paths */
        public readonly static string ROOT_PATH = Directory.GetCurrentDirectory();
        public readonly static string EXAMPLES_PATH = Path.Combine(ROOT_PATH, "examples");
        public readonly static string COMPILED_PATH = Path.Combine(ROOT_PATH, "compiled");
        public readonly static string WORKING_PATH = Path.Combine(COMPILED_PATH, "working");
        public readonly static string COMPILER_EXE_PATH = Path.Combine(ROOT_PATH, "AlmostBinary_Compiler.exe");

        /* File types */
        public const string UNCOMPILED_FILE_TYPE = "abin";
        public const string COMPILED_FILE_TYPE = "wsdt";

        /* File-Names */
        public readonly static string[] ALL_FILENAMES = new string[]
        {
            CALL, HELLO_WORLD, IF, INPUT, REPEAT, VARIABLE
        };
        public const string CALL = "call";
        public const string HELLO_WORLD = "helloworld";
        public const string IF = "if";
        public const string INPUT = "input";
        public const string REPEAT = "repeat";
        public const string VARIABLE = "variable";
    }
}
