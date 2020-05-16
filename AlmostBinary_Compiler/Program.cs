using AlmostBinary_Compiler.Global;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Diagnostics.Tracing;
using System.IO;

namespace AlmostBinary_Compiler
{
    public class Program
    {
        /// <summary>
        /// Used to bring at least some context to the Program class, otherwise most context fields in outputTemplate would be empty.
        /// Line-No, method name .. still empty.
        /// </summary>
        private static ILogger StartupLogger => Serilog.Log.ForContext<Program>();

        #region methods
        static void Main(string[] args)
        {
            Console.WriteLine($"Starting compiler. Received {args.Length} argument(s)."); // Logger not initialized yet
            IServiceCollection services = ConfigureServices();
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<Startup>().Run(args);

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnShutdown!);
        }

        /// <summary>
        /// Is executed on proccess exit.
        /// </summary>
        private static void OnShutdown(object sender, EventArgs e)
        {
            StartupLogger.Information("Program:OnShutdown: Quitting compiler.");
            Log.CloseAndFlush();
        }

        /// <summary>
        /// Configures global services.
        /// </summary>
        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            IConfiguration logConfiguration = BuildConfiguration();
            services.AddSingleton(logConfiguration);

            // Required to run application
            services.AddTransient<Startup>();

            Log.Logger = CraftLogger(logConfiguration);

            StartupLogger.Information("Attached and configured services.");
            return services;
        }

        /// <summary>
        /// Makes configuration accessible related to current build (debug/release).
        /// </summary>
        /// <returns>Global Configuration</returns>
        private static IConfiguration BuildConfiguration() => new ConfigurationBuilder()
                .SetBasePath(Path.Combine(IGlobalConstants.PROJECT_ROOT_PATH, "Properties"))
                .AddJsonFile(IGlobalConstants.APP_SETTINGS_FILE, optional: false, reloadOnChange: true)
                .Build();

        /// <summary>
        /// Builds File- and Console logger
        /// </summary>
        /// <param name="configuration">Global configuration</param>
        /// <returns>Logger</returns>
        private static ILogger CraftLogger(IConfiguration configuration)
        {
            string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} - {SourceContext}:{MemberName}:{LineNumber}{NewLine}{Exception}";
            LogEventLevel logLevel = configuration.GetValue<LogEventLevel>("Runtime:Logging:LogLevel");

            return new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Async(f => f.File(
                Path.Combine(
                    IGlobalConstants.PROJECT_ROOT_PATH,
                    configuration.GetValue<string>("Runtime:Logging:LogOutputPath")
                ),
                restrictedToMinimumLevel: logLevel,
                rollingInterval: RollingInterval.Day,
                buffered: true,
                outputTemplate: outputTemplate))
            .WriteTo.Console(
                outputTemplate: outputTemplate,
                restrictedToMinimumLevel: logLevel,
                theme: AnsiConsoleTheme.Code
                )
            .CreateLogger();
        }
    }
    #endregion
}
