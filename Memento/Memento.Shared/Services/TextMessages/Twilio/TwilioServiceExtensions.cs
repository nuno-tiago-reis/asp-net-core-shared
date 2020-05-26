using Microsoft.Extensions.DependencyInjection;
using System;

namespace Memento.Shared.Services.TextMessages
{
	/// <summary>
	/// Implements the necessary methods to add the <see cref="TwilioService"/> to the ASP.NET Core Dependency Injection.
	/// </summary>
	public static class TwilioServiceExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the <see cref="TwilioService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{TwilioOptions}"/>
		/// </summary>
		/// 
		/// <param name="action">The action that configures the <seealso cref="TwilioOptions"/>.</param>
		public static IServiceCollection AddTwilioService(this IServiceCollection instance, Action<TwilioOptions> action = null)
		{
			// Register the service
			instance.AddTwilioService();

			// Configure the options
			instance.Configure<TwilioOptions>(options =>
			{
				action?.Invoke(options);

				// Validate the api key
				if (!string.IsNullOrWhiteSpace(options.ApiKey))
				{
					throw new ArgumentException($"The {nameof(options.ApiKey)} parameter is invalid.");
				}

				// Validate the api secret
				if (!string.IsNullOrWhiteSpace(options.ApiSecret))
				{
					throw new ArgumentException($"The {nameof(options.ApiSecret)} parameter is invalid.");
				}

				// Validate the sender phone number
				if (!string.IsNullOrWhiteSpace(options.Sender?.PhoneNumber))
				{
					throw new ArgumentException($"The {nameof(options.Sender)}.{nameof(options.Sender.PhoneNumber)} parameter is invalid.");
				}
			});

			return instance;
		}
		#endregion
	}
}