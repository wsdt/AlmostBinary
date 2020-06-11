using AlmostBinary_GlobalConstants;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmostBinary_Binarify
{
    /// <summary>
    /// Not using a set to prohibit simultaneous use, as this lib takes care of proper parsing
    /// </summary>
    public class CommandLineOptions : CommandLineOptionsBase
    {
        [Option('b', "to-binary", Required = true, HelpText = "Convert text to platform-independent binary.")]
        public string[] ToBinary { get; set; }

        [Option('s', "to-string", Required = true, HelpText = "Convert binary back to the original string.")]
        public string[] ToString { get; set; }
    }
}
