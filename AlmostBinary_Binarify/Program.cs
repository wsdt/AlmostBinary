using AlmostBinary_Binarify.utils;
using AlmostBinary_GlobalConstants;
using CommandLine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using System.Reflection;

namespace AlmostBinary_Binarify
{
    public class Program
    {
        /// <summary>
        /// Used to bring at least some context to the Program class, otherwise most context fields in outputTemplate would be empty.
        /// Line-No, method name .. still empty.
        /// </summary>
        private static ILogger StartupLogger => Log.ForContext<Program>();
        /// <summary>
        /// Not outsourcable into GlobalConstants as Assembly etc. are not usable in that context.
        /// </summary>
        public static readonly string? PROGRAM_ENTRY_PATH = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        public static void Main(string[] args)
        {
            Console.WriteLine($"Starting Binarify. Received {args.Length} argument(s)."); // Logger not initialized yet

            try
            {
                // Parse command line args
                CommandLine.Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(o =>
                {
                    IServiceCollection services = ConfigureServices(o.Verbose);
                    ServiceProvider serviceProvider = services.BuildServiceProvider();

                    serviceProvider.GetService<Startup>().Run(o);
                });
            }
            catch (Exception ex)
            {
                StartupLogger.Here().Fatal(ex, $"Ooops, something went wrong with Binarify.");
            }

            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnShutdown!);
        }


        /// <summary>
        /// Is executed on proccess exit.
        /// </summary>
        private static void OnShutdown(object sender, EventArgs e)
        {
            StartupLogger.Information("Quitting Binarify.");
            Log.CloseAndFlush();
        }

        /// <summary>
        /// Configures global services.
        /// </summary>
        private static IServiceCollection ConfigureServices(bool? setVerboseLogLevel)
        {
            IServiceCollection services = new ServiceCollection();
            IConfiguration configuration = BuildConfiguration();
            services.AddSingleton(configuration);

            // Required to run application
            services.AddTransient<Startup>();

            Log.Logger = CraftLogger(configuration, setVerboseLogLevel == true ? LogEventLevel.Verbose : configuration.GetValue<LogEventLevel>("Logging:LogLevel"));

            StartupLogger.Information("Attached and configured services.");
            return services;
        }

        /// <summary>
        /// Makes configuration accessible related to current build (debug/release).
        /// </summary>
        /// <returns>Global Configuration</returns>
        private static IConfiguration BuildConfiguration() => new ConfigurationBuilder()
                .SetBasePath(Path.Combine(PROGRAM_ENTRY_PATH, "Properties"))
                .AddJsonFile(IGlobalConstants.APP_SETTINGS_FILE, optional: false, reloadOnChange: true)
                .Build();

        /// <summary>
        /// Builds File- and Console logger
        /// </summary>
        /// <param name="configuration">Global configuration</param>
        /// <returns>Logger</returns>
        private static ILogger CraftLogger(IConfiguration configuration, LogEventLevel logLevel)
        {
            string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} - {SourceContext}:{MemberName}:{LineNumber}{NewLine}{Exception}";

            return new LoggerConfiguration()
            .Enrich.FromLogContext()
            .MinimumLevel.Verbose()
            .WriteTo.Async(f => f.File(
                Path.Combine(
                    IGlobalConstants.PROJECT_ROOT_PATH,
                    configuration.GetValue<string>("Logging:LogOutputPath")
                ),
                rollingInterval: RollingInterval.Day,
                buffered: true,
                restrictedToMinimumLevel: logLevel,
                outputTemplate: outputTemplate))
            .WriteTo.Console(
                outputTemplate: outputTemplate,
                restrictedToMinimumLevel: logLevel,
                theme: AnsiConsoleTheme.Code
                )
            .CreateLogger();
        }
    }
}
