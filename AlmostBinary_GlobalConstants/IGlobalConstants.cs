using System;
using System.IO;

namespace AlmostBinary_GlobalConstants
{
    /// <summary>
    /// Global constants calculated at runtime. Prefer using appsettings.json
    /// </summary>
    public interface IGlobalConstants
    {
        // File Paths
        public static readonly string PROJECT_ROOT_PATH = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "..");

        // Release/Debug files
#if DEBUG
        public static readonly string APP_SETTINGS_FILE = "appsettings_DEV.json";
        public static readonly string OUTPUT_PATH = Path.Combine(PROJECT_ROOT_PATH, "..", "compiled");
#else
        public static readonly string APP_SETTINGS_FILE = "appsettings_MAIN.json";
        public static readonly string OUTPUT_PATH = Directory.GetCurrentDirectory(); // output to current dir if release
#endif
    }
}