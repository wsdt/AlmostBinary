using Serilog.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlmostBinary_Compiler.Global
{
    /// <summary>
    /// Global constants calculated at runtime. Prefer using appsettings.json
    /// </summary>
    public interface IGlobalConstants
    {
        // File Paths
        public static readonly string PROJECT_ROOT_PATH = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "..");
        public static readonly string OUTPUT_PATH = Path.Combine(PROJECT_ROOT_PATH, "compiled");

        // Release/Debug files
#if DEBUG
        public static readonly string APP_SETTINGS_FILE = "appsettings_DEV.json";
#else
        public static readonly string APP_SETTINGS_FILE = "appsettings_MAIN.json";
#endif
    }
}
