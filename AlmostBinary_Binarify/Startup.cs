using AlmostBinary_Binarify.utils;
using Serilog;
using System;
using System.Linq;
using static AlmostBinary_Binarify.BinaryConverter;

namespace AlmostBinary_Binarify
{
    public class Startup
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Startup>();
        #endregion

        #region methods
        /// <summary>
        /// Converts input to binary.
        /// </summary>
        /// <param name="args"></param>
        public void Run(CommandLineOptions args)
        {
            if (args.ToBinary != null) ConvertAllToBinary(args.ToBinary.ToArray());
            if (args.ToString != null) ConvertAllToString(args.ToString.ToArray());
        }
        #endregion
    }
}
