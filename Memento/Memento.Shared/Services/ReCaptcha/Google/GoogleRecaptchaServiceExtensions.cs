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
		/// Configures the options using specified <seealso cref="Action{GoogleReCaptchaOptions}"/>
		/// </summary>
		/// 
		/// <param name="action">The action that configures the <seealso cref="GoogleReCaptchaOptions"/>.</param>
		public static IServiceCollection AddGoogleRecaptchaService(this IServiceCollection instance, Action<GoogleReCaptchaOptions> action = null)
		{
			// Register the service
			instance.AddHttpClient<IRecaptchaService, GoogleRecaptchaService>();

			// Configure the options
			instance.Configure<GoogleReCaptchaOptions>(options =>
			{
				action?.Invoke(options);

				// Validate the host
				if (!string.IsNullOrWhiteSpace(options.Host))
				{
					throw new ArgumentException($"The {nameof(options.Host)} parameter is invalid.");
				}

				// Validate the site key
				if (!string.IsNullOrWhiteSpace(options.SiteKey))
				{
					throw new ArgumentException($"The {nameof(options.SiteKey)} parameter is invalid.");
				}

				// Validate the site secret
				if (!string.IsNullOrWhiteSpace(options.SiteSecret))
				{
					throw new ArgumentException($"The {nameof(options.SiteSecret)} parameter is invalid.");
				}
			});

			return instance;
		}
		#endregion
	}
}