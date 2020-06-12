using AlmostBinary_GlobalConstants;
using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmostBinary_Binarify
{
    public class CommandLineOptions : CommandLineOptionsBase
    {
        [Option('b', "to-binary", SetName = "BinaryConverter" , Required = true, HelpText = "Convert text to platform-independent binary.")]
        public IEnumerable<string> ToBinary { get; set; }

        [Option('s', "to-string", SetName = "StringConverter", Required = true, HelpText = "Convert binary back to the original string.")]
        public IEnumerable<string> ToString { get; set; }
    }
}
