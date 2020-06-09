using AlmostBinary_Binarify.utils;
using Serilog;
using System;
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
                throw new ArgumentException("Binarify expects at least two parameters.");
            }
            switch (args[0])
            {
                case ARG_TOBINARY: ConvertAllToBinary(args); break;
                case ARG_TOSTRING: ConvertAllToString(args); break;
                default: throw new ArgumentException($"Unknown parameter: {args[0]}");
            }
            Log.Here().Verbose($"Converted {args.Length - 1} values.");
        }
        #endregion
    }
}
