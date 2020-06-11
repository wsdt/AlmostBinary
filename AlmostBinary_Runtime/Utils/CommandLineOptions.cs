using AlmostBinary_GlobalConstants;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmostBinary_Runtime.utils
{
    public class CommandLineOptions : CommandLineOptionsBase
    {
        [Option('f', "file", Required = true, HelpText = "Which file should be executed?")]
        public string CompiledFile { get; set; }
    }
}
