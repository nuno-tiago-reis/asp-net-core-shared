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
		/// Adds the <see cref="SendGridService"/> to the ASP.NET Core Dependency Injection to the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{SendGridOptions}"/>
		/// </summary>
		/// 
		/// <param name="action">The action that configures the <seealso cref="SendGridOptions"/>.</param>
		public static IServiceCollection AddSendGridService(this IServiceCollection instance, Action<SendGridOptions> action = null)
		{
			// Register the service
			instance.AddScoped<IEmailService, SendGridService>();

			// Configure the options
			instance.Configure<SendGridOptions>(options =>
			{
				action?.Invoke(options);

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
			});

			return instance;
		}
		#endregion
	}
}