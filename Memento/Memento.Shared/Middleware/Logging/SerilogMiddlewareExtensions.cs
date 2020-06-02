using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Memento.Shared.Middleware.Logging
{
	/// <summary>
	/// Implements the necessary methods to add the Serilog middleware to the ASP.NET Core Pipeline.
	/// </summary>
	public static class SerilogMiddlewareExtensions
	{
		#region [Constants]
		/// <summary>
		/// The message template.
		/// </summary>
		private const string MESSAGE_TEMPLATE =
			"HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {ElapsedTime:0.000} ms";
		#endregion

		#region [Extensions]
		/// <summary>
		/// Registers the Serilog middleware in the pipeline of the specified <seealso cref="IServiceCollection"/>.
		/// </summary>
		///
		/// <param name="services">The service collection.</param>
		public static IServiceCollection AddSerilogMiddleware(this IServiceCollection services)
		{
			services
				.AddSession(options =>
				{
					options.Cookie.HttpOnly = false;
					options.Cookie.IsEssential = false;
				});
			services
				.AddDistributedMemoryCache();

			return services;
		}

		/// <summary>
		/// Registers the Serilog middleware in the pipeline of the specified <seealso cref="IApplicationBuilder"/>.
		/// Configures the options using specified <seealso cref="Action{SerilogMiddlewareOptions}"/>
		/// </summary>
		///
		/// <param name="builder">The application builder.</param>
		/// <param name="action">The action that configures the <seealso cref="SerilogMiddlewareOptions"/>.</param>
		public static IApplicationBuilder UseSerilogMiddleware(this IApplicationBuilder builder, Action<SerilogMiddlewareOptions> action = null)
		{
			// Create the options
			var options = new SerilogMiddlewareOptions
			{
				MessageTemplate = MESSAGE_TEMPLATE
			};
			// Configure the options
			action?.Invoke(options);

			// Validate the options
			if (string.IsNullOrWhiteSpace(options.MessageTemplate))
			{
				throw new ArgumentException($"The {nameof(options.MessageTemplate)} parameter is invalid.");
			}

			// Register the middleware
			builder.UseMiddleware<SerilogMiddleware>(options);

			return builder;
		}
		#endregion
	}
}