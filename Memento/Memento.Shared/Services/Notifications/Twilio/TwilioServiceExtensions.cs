using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Memento.Shared.Services.Notifications.Twilio
{
	/// <summary>
	/// Implements the necessary methods to add the <see cref="TwilioService"/> to the ASP.NET Core Dependency Injection.
	/// </summary>
	[UsedImplicitly]
	public static class TwilioServiceExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the <see cref="TwilioService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Uses the specified <seealso cref="TwilioOptions"/>
		/// </summary>
		///
		/// <param name="services">The service collection.</param>
		/// <param name="options">The options.</param>
		[UsedImplicitly]
		public static IServiceCollection AddTwilioService(this IServiceCollection services, TwilioOptions options)
		{
			// Validate the options
			if (options == null)
			{
				throw new ArgumentException($"The {nameof(options)} are invalid.");
			}

			// Validate the api key
			if (string.IsNullOrWhiteSpace(options.ApiKey))
			{
				throw new ArgumentException($"The {nameof(options.ApiKey)} parameter is invalid.");
			}

			// Validate the api secret
			if (string.IsNullOrWhiteSpace(options.ApiSecret))
			{
				throw new ArgumentException($"The {nameof(options.ApiSecret)} parameter is invalid.");
			}

			// Validate the sender phone number
			if (string.IsNullOrWhiteSpace(options.Sender?.PhoneNumber))
			{
				throw new ArgumentException($"The {nameof(options.Sender)}.{nameof(options.Sender.PhoneNumber)} parameter is invalid.");
			}

			// Register the service
			services.AddScoped<ITextMessageService, TwilioService>();

			// Configure the options
			services.AddSingleton(options);

			return services;
		}

		/// <summary>
		/// Registers the <see cref="TwilioService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{TwilioOptions}"/>
		/// </summary>
		///
		/// <param name="services">The service collection.</param>
		/// <param name="action">The action that configures the <seealso cref="TwilioOptions"/>.</param>
		[UsedImplicitly]
		public static IServiceCollection AddTwilioService(this IServiceCollection services, Action<TwilioOptions> action)
		{
			// Create the options
			var options = new TwilioOptions();
			// Configure the options
			action?.Invoke(options);

			// Register the service
			services.AddTwilioService(options);

			return services;
		}
		#endregion
	}
}