using Microsoft.Extensions.DependencyInjection;
using System;

namespace Memento.Shared.Services.Emails
{
	/// <summary>
	/// Implements the necessary methods to add the <see cref="SendGridService"/> to the ASP.NET Core Dependency Injection.
	/// </summary>
	public static class SendGridServiceExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the <see cref="SendGridService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Uses the specified <seealso cref="SendGridOptions"/>
		/// </summary>
		/// 
		/// <param name="options">The options.</param>
		public static IServiceCollection AddSendGridService(this IServiceCollection services, SendGridOptions options)
		{
			// Validate the options
			if (options == null)
			{
				throw new ArgumentException($"The {nameof(options)} are invalid.");
			}

			// Validate the api key
			if (!string.IsNullOrWhiteSpace(options.ApiKey))
			{
				throw new ArgumentException($"The {nameof(options.ApiKey)} parameter is invalid.");
			}

			// Validate the sender email
			if (!string.IsNullOrWhiteSpace(options.Sender?.Email))
			{
				throw new ArgumentException($"The {nameof(options.Sender)}.{nameof(options.Sender.Email)} parameter is invalid.");
			}

			// Validate the sender name
			if (!string.IsNullOrWhiteSpace(options.Sender?.Name))
			{
				throw new ArgumentException($"The {nameof(options.Sender)}.{nameof(options.Sender.Name)} parameter is invalid.");
			}

			// Register the service
			services.AddScoped<IEmailService, SendGridService>();

			// Configure the options
			services.ConfigureOptions(options);

			return services;
		}

		/// <summary>
		/// Registers the <see cref="SendGridService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{SendGridOptions}"/>
		/// </summary>
		/// 
		/// <param name="action">The action that configures the <seealso cref="SendGridOptions"/>.</param>
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