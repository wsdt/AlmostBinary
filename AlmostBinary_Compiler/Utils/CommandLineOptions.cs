using AlmostBinary_GlobalConstants;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmostBinary_Compiler
{
    public class CommandLineOptions : CommandLineOptionsBase
    {
        [Option('f', "file", Required = true, HelpText = "Which file should be compiled?")]
        public string UnCompiledFile { get; set; }

        [Option('o', "ouput-path", Required = false, 
            HelpText = "Defines the output path of compiled files. If not defined, files will be compiled to the compiled-folder of the program directory.")]
        public string OutputPath { get; set; }
    }
}
