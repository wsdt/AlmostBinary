using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using AlmostBinary_Runtime.Global;
using Serilog;
using Microsoft.Extensions.Configuration;
using AlmostBinary_Runtime.utils;
using System.Security.Cryptography;
using System.Text.Json;

namespace AlmostBinary_Runtime
{
    public class Startup
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Startup>();
        private readonly IConfiguration _configuration;
        private static List<string> _imports = new List<string>();
        #endregion

        #region properties
        public static List<string> Imports { get => _imports; set => _imports = value; }
        #endregion

        #region ctor
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region methods
        public void Run(string[] args)
        {
            try
            {
                FileStream fs = new FileStream(args[0], FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                string code = br.ReadString();
                Log.Here().Verbose($"Input-File: '{code}'");
                Log.Here().Information("Starting Runtime.");
                Runtime runtime = new Runtime(code);
            } catch(Exception ex)
            {
                // Top-level logging for uncaught/propagated exceptions
                Log.Here().Fatal(ex, "Code Interpretation failed.");
                throw;
            }
        }
        #endregion
    }
}
