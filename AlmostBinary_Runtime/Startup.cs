using AlmostBinary_Runtime.utils;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;

namespace AlmostBinary_Runtime
{
    public class Startup
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Startup>();
        #endregion

        #region methods
        /// <summary>
        /// Runs provided .wsdt-file.
        /// </summary>
        /// <param name="args"></param>
        public void Run(string[] args)
        {
            try
            {
                FileStream fs = new FileStream(args[0], FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                string code = br.ReadString();
                RunInline(code);
            }
            catch (Exception ex)
            {
                // Top-level logging for uncaught/propagated exceptions
                Log.Here().Fatal(ex, "Code Interpretation failed.");
                throw;
            }
        }

        /// <summary>
        /// Provide .wsdt-code directly as parameter.
        /// </summary>
        /// <param name="code">Compiled .abin code</param>
        public void RunInline(string code)
        {
            Log.Here().Verbose($"Input-File: '{code}'");
            Log.Here().Information("Starting Runtime.");
            new Runtime(code);
        }
        #endregion
    }
}
