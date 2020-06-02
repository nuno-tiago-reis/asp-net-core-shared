using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Memento.Shared.Services.Notifications
{
	/// <summary>
	/// Implements the necessary methods to add the <see cref="SendGridService"/> to the ASP.NET Core Dependency Injection.
	/// </summary>
	[UsedImplicitly]
	public static class SendGridServiceExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the <see cref="SendGridService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Uses the specified <seealso cref="SendGridOptions"/>
		/// </summary>
		///
		/// <param name="services">The service collection.</param>
		/// <param name="options">The options.</param>
		[UsedImplicitly]
		public static IServiceCollection AddSendGridService(this IServiceCollection services, SendGridOptions options)
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

			// Validate the sender email
			if (string.IsNullOrWhiteSpace(options.Sender?.Email))
			{
				throw new ArgumentException($"The {nameof(options.Sender)}.{nameof(options.Sender.Email)} parameter is invalid.");
			}

			// Validate the sender name
			if (string.IsNullOrWhiteSpace(options.Sender?.Name))
			{
				throw new ArgumentException($"The {nameof(options.Sender)}.{nameof(options.Sender.Name)} parameter is invalid.");
			}

			// Register the service
			services.AddScoped<IEmailService, SendGridService>();

			// Configure the options
			services.AddSingleton(options);

			return services;
		}

		/// <summary>
		/// Registers the <see cref="SendGridService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{SendGridOptions}"/>
		/// </summary>
		///
		/// <param name="services">The service collection.</param>
		/// <param name="action">The action that configures the <seealso cref="SendGridOptions"/>.</param>
		[UsedImplicitly]
		public static IServiceCollection AddSendGridService(this IServiceCollection services, Action<SendGridOptions> action)
		{
			// Create the options
			var options = new SendGridOptions();
			// Configure the options
			action?.Invoke(options);

			// Register the service
			services.AddSendGridService(options);

			return services;
		}
		#endregion
	}
}