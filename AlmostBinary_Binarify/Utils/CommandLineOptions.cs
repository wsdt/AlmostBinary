using AlmostBinary_GlobalConstants;
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlmostBinary_Binarify
{
    public class CommandLineOptions : CommandLineOptionsBase
    {
        public const BitArch DEFAULT_BIT_ARCH = BitArch.x256;

        /// <summary>
        /// Defines the word-length of each keyword, ...
        /// </summary>
        public enum BitArch { x256 = 256, x128 = 128, x64 = 64, x32 = 32, x16 = 16, x8 = 8 }

        [Option('b', "to-binary", SetName = "BinaryConverter" , Required = true, HelpText = "Convert text to platform-independent binary.")]
        public IEnumerable<string> ToBinary { get; set; }

        [Option('s', "to-string", SetName = "StringConverter", Required = true, HelpText = "Convert binary back to the original string.")]
        public IEnumerable<string> ToString { get; set; }

        [Option('a', "arch", Required = false, Default = BitArch.x64, HelpText = "Defines internal word size for compilation, interpretation and Binarify.")]
        public BitArch BitArchitecture { get; set; }
    }
}
