using AlmostBinary_Binarify.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using static AlmostBinary_Binarify.BinaryConverter;

namespace AlmostBinary_Binarify
{
    public class Startup
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Startup>();
        public const string ARG_TOBINARY = "--to-binary";
        public const string ARG_TOSTRING = "--to-string";
        #endregion

        #region methods
        /// <summary>
        /// Converts input to binary.
        /// </summary>
        /// <param name="args"></param>
        public void Run(string[] args)
        {
            if (args.Length < 2)
            {
                throw new Exception("Binarify expects at least two parameters.");
            }
            switch (args[0])
            {
                case ARG_TOBINARY: ConvertAllToBinary(args); break;
                case ARG_TOSTRING: ConvertAllToString(args); break;
                default: throw new Exception($"Unknown parameter: {args[0]}");
            }
            Log.Here().Verbose($"Converted {args.Length - 1} values.");
        }

        /// <summary>Converts all provided arguments to binary (except the first which is reserved for configuration).</summary>
        private void ConvertAllToBinary(string[] args)
        {
            Log.Here().Verbose("Converting values to Binary.");
            for (int i = 1; i < args.Length; i++)
            {
                Binary b = args[i].ToBinary();
                Log.Here().Information($"Argument {i}: '{args[i]}' -> '{b.BinaryString}'");
            }
        }

        /// <summary>Converts all provided arguments back to the orginal string (except the first which is reserved for configuration).</summary>
        private void ConvertAllToString(string[] args)
        {
            Log.Here().Verbose("Converting values to Strings");
            for (int i = 1; i < args.Length; i++)
            {
                Binary b = new Binary() { BinaryString = args[i] };
                Log.Here().Information($"Argument {i}: '{args[i]}' -> {b}");
            }
            #endregion
        }
    }
}
