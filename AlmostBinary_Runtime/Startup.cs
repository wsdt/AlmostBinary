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
            //StreamReader sr = new StreamReader(args[0]);
            FileStream fs = new FileStream(args[0], FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            string code = br.ReadString();
            //string code = sr.ReadToEnd();
            Runtime runtime = new Runtime(code);
        }
        #endregion
    }
}
