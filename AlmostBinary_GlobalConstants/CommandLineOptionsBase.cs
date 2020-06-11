using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmostBinary_GlobalConstants
{
    /// <summary>
    /// Base class for command line options for all projects
    /// </summary>
    public class CommandLineOptionsBase
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }
    }
}
