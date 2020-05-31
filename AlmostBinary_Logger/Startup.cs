using AlmostBinary_Logger.utils;
using Serilog;

namespace AlmostBinary_Logger
{
    public class Startup
    {
        #region fields
        private static ILogger Log => Serilog.Log.ForContext<Startup>();
        #endregion

        #region methods
        public void Run(string[] args)
        {
            Log.Here().Verbose("Started logger service.");
        }
        #endregion
    }
}
