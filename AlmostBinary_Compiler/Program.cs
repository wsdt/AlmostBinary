﻿using AlmostBinary_Compiler.utils;
using AlmostBinary_GlobalConstants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using System.Reflection;

namespace AlmostBinary_Compiler
{
    public class Program
    {
        /// <summary>
        /// Used to bring at least some context to the Program class, otherwise most context fields in outputTemplate would be empty.
        /// Line-No, method name .. still empty.
        /// </summary>
        private static ILogger StartupLogger => Serilog.Log.ForContext<Program>();
        /// <summary>
        /// Not outsourcable into GlobalConstants as Assembly etc. are not usable in that context.
        /// </summary>
        public static readonly string? PROGRAM_ENTRY_PATH = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        #region methods
        public static void Main(string[] args)
        {
            Console.WriteLine($"Starting compiler. Received {args.Length} argument(s)."); // Logger not initialized yet

            try
            {
                Compile(args);
            }
            catch (Exception e)
            {
                StartupLogger.Fatal(e, $"Unexpected exception. Could not start application.");
            }
            finally
            {
                AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnShutdown!);
            }
        }

        public static string? Compile(string[] args)
        {
            IServiceCollection services = ConfigureServices();
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            if (args.Length <= 0) throw new ArgumentException("You have to provide at least one argument.");
            string? compiledCode = null;
            switch (args[0])
            {
                case "--inline-code":
                    if (args.Length <= 1) throw new ArgumentException("Parameter --inline-code expects 2 arguments.");
                    compiledCode = serviceProvider.GetService<Startup>().CompileInline(args[1]); 
                    break;
                default: 
                    serviceProvider.GetService<Startup>().Run(args); 
                    break;
            }
            return compiledCode;
        }

        /// <summary>
        /// Is executed on proccess exit.
        /// </summary>
        private static void OnShutdown(object sender, EventArgs e)
        {
            StartupLogger.Information("Quitting compiler.");
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
                .SetBasePath(Path.Combine(
                    Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), 
                    "Properties"))
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
            .MinimumLevel.Verbose()
            .WriteTo.Async(f => f.File(
                Path.Combine(
                    IGlobalConstants.PROJECT_ROOT_PATH,
                    configuration.GetValue<string>("Runtime:Logging:LogOutputPath")
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
    #endregion
}
