using Microsoft.Extensions.DependencyInjection;
using System;

namespace Memento.Shared.Services.ReCaptcha
{
	/// <summary>
	/// Implements the necessary methods to add the <see cref="GoogleRecaptchaService"/> to the ASP.NET Core Dependency Injection.
	/// </summary>
	public static class GoogleRecaptchaServiceExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the <see cref="GoogleRecaptchaService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Uses the specified <seealso cref="GoogleReCaptchaOptions"/>
		/// </summary>
		/// 
		/// <param name="options">The options.</param>
		public static IServiceCollection AddGoogleRecaptchaService(this IServiceCollection services, GoogleReCaptchaOptions options)
		{
			// Validate the options
			if (options == null)
			{
				throw new ArgumentException($"The {nameof(options)} are invalid.");
			}

			// Validate the host
			if (string.IsNullOrWhiteSpace(options.Host))
			{
				throw new ArgumentException($"The {nameof(options.Host)} parameter is invalid.");
			}

			// Validate the site key
			if (string.IsNullOrWhiteSpace(options.SiteKey))
			{
				throw new ArgumentException($"The {nameof(options.SiteKey)} parameter is invalid.");
			}

			// Validate the site secret
			if (string.IsNullOrWhiteSpace(options.SiteSecret))
			{
				throw new ArgumentException($"The {nameof(options.SiteSecret)} parameter is invalid.");
			}

			// Register the service
			services.AddHttpClient<IRecaptchaService, GoogleRecaptchaService>();

			// Configure the options
			services.AddSingleton(options);

			return services;
		}

		/// <summary>
		/// Registers the <see cref="GoogleRecaptchaService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{GoogleReCaptchaOptions}"/>
		/// </summary>
		/// 
		/// <param name="action">The action that configures the <seealso cref="GoogleReCaptchaOptions"/>.</param>
		public static IServiceCollection AddGoogleRecaptchaService(this IServiceCollection services, Action<GoogleReCaptchaOptions> action)
		{
			// Create the options
			var options = new GoogleReCaptchaOptions();
			// Configure the options
			action?.Invoke(options);

			// Register the service
			services.AddGoogleRecaptchaService(options);

			return services;
		}
		#endregion
	}
}