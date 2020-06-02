using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;

namespace Memento.Shared.Middleware.Logging
{
	/// <summary>
	/// Implements the necessary methods to add the Serilog logger to the ASP.NET Core App.
	/// </summary>
	public static class SerilogExtensions
	{
		#region [Constants]
		/// <summary>
		/// The log template.
		/// </summary>
		private const string LOG_TEMPLATE =
			"[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u4}] {SourceContext}{NewLine}" +
			"{Message:j}{NewLine}" +
			"{Properties:j}{NewLine}" +
			"{Exception}{NewLine}";
		#endregion

		#region [Methods]
		/// <summary>
		/// Registers the Serilog logger in the specified <seealso cref="IWebHostBuilder"/>.
		/// Configures the options using specified <seealso cref="Action{SerilogOptions}"/>
		/// </summary>
		/// 
		/// <param name="builder">The web host builder.</param>
		/// <param name="action">The action.</param>
		public static IWebHostBuilder UseSerilogLogging(this IWebHostBuilder builder, Action<SerilogOptions> action)
		{
			// Configure the options
			var options = new SerilogOptions();
			action.Invoke(options);

			// Validate the options
			if (Enum.TryParse(options.Level, out LogEventLevel level) == false)
			{
				throw new ArgumentOutOfRangeException(nameof(options.Level), options.Level);
			}

			// Create the theme (based on AnsiConsoleTheme.Literate)
			var theme = new AnsiConsoleTheme(new Dictionary<ConsoleThemeStyle, string>
			{
				[ConsoleThemeStyle.Text] = "\x001B[38;5;0015m",
				[ConsoleThemeStyle.SecondaryText] = "\x001B[38;5;0222m",
				[ConsoleThemeStyle.TertiaryText] = "\x001B[38;5;0015m",
				[ConsoleThemeStyle.Invalid] = "\x001B[38;5;0011m",
				[ConsoleThemeStyle.Null] = "\x001B[38;5;0027m",
				[ConsoleThemeStyle.Name] = "\x001B[38;5;0136m",
				[ConsoleThemeStyle.String] = "\x001B[38;5;0075m",
				[ConsoleThemeStyle.Number] = "\x001B[38;5;0208m",
				[ConsoleThemeStyle.Boolean] = "\x001B[38;5;0027m",
				[ConsoleThemeStyle.Scalar] = "\x001B[38;5;0085m",
				[ConsoleThemeStyle.LevelVerbose] = "\x001B[38;5;0231m",
				[ConsoleThemeStyle.LevelDebug] = "\x001B[38;5;0224m",
				[ConsoleThemeStyle.LevelInformation] = "\x001B[38;5;0076m",
				[ConsoleThemeStyle.LevelWarning] = "\x001B[38;5;0220m",
				[ConsoleThemeStyle.LevelError] = "\x001B[38;5;0015m\x001B[48;5;0160m",
				[ConsoleThemeStyle.LevelFatal] = "\x001B[38;5;0015m\x001B[48;5;0088m"
			});

			// Create the logger
			Log.Logger = new LoggerConfiguration()
				// MinimumLevel
				.MinimumLevel.Is(level)
				// MinimumLevel: Hosting
				.MinimumLevel.Override("Microsoft.Hosting.Diagnostics", LogEventLevel.Warning)
				// MinimumLevel: Http
				.MinimumLevel.Override("System.Net.Http.HttpClient", LogEventLevel.Warning)
				// MinimumLevel: AspNetCore
				.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.AspNetCore.StaticFiles.StaticFileMiddleware", LogEventLevel.Warning)
				// MinimumLevel: EntityFramework
				.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Connection", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Infrastructure", LogEventLevel.Warning)
				.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Query", LogEventLevel.Warning)
				// Enrich
				.Enrich.FromLogContext()
				// Sinks
				.WriteTo.Async(sink => sink.Console(outputTemplate: LOG_TEMPLATE, theme: theme))
				// Build
				.CreateLogger();

			// Configure self logging
			Serilog.Debugging.SelfLog.Enable(Console.Out);

			// Log the configured options
			Log.Logger.ForContext(typeof(SerilogExtensions)).Information("Running Serilog (Minimum Log Level: {LogLevel})", options.Level);

			return builder;
		}
		#endregion
	}
}