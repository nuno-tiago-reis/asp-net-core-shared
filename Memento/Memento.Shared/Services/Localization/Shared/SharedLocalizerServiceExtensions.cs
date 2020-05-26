using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Linq;

namespace Memento.Shared.Services.Localization
{
	/// <summary>
	/// Implements the necessary methods to add the <see cref="SharedLocalizerServiceExtensions"/> to the ASP.NET Core Dependency Injection.
	/// </summary>
	public static class SharedLocalizerServiceExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the <see cref="SharedLocalizerService{T}"/> in the pipeline of the specified <seealso cref="IMvcBuilder"/>.
		/// Configures the options using specified <seealso cref="Action{SharedLocalizerOptions}"/>
		/// </summary>
		/// 
		/// <param name="action">The action that configures the <seealso cref="SharedLocalizerOptions"/>.</param>
		public static IMvcBuilder AddSharedLocalization<T>(this IMvcBuilder instance, Action<SharedLocalizerOptions> action = null) where T : class
		{
			// Register the provider
			instance.AddDataAnnotationsLocalization(options =>
			{
				options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(T));
			});

			// Register the service
			instance.Services.AddScoped<ILocalizerService, SharedLocalizerService<T>>();

			// Configure the options
			instance.Services.Configure<SharedLocalizerOptions>(options =>
			{
				action?.Invoke(options);

				// Validate the default culture
				if (!string.IsNullOrWhiteSpace(options.DefaultCulture))
				{
					throw new ArgumentException($"The {nameof(options.DefaultCulture)} parameter is invalid.");
				}

				// Validate the supported cultures
				if (options.SupportedCultures == null || options.SupportedCultures.Length == 0)
				{
					throw new ArgumentException($"The {nameof(options.SupportedCultures)} parameter is invalid.");
				}
			});

			// Get the options
			var options = instance.Services.BuildServiceProvider().GetService <IOptions<SharedLocalizerOptions>>()?.Value;

			// Configure the localization options
			instance.Services.Configure<RequestLocalizationOptions>(localizationOptions =>
			{
				var defaultCulture = new RequestCulture(options.DefaultCulture);
				var supportedCultures = options.SupportedCultures.Select(culture => new CultureInfo(culture)).ToList();

				localizationOptions.DefaultRequestCulture = defaultCulture;
				localizationOptions.SupportedCultures = supportedCultures;
				localizationOptions.SupportedUICultures = supportedCultures;
			});


			return instance;
		}

		/// <summary>
		/// Registers the <see cref="SharedLocalizerService{T}"/> in the pipeline of the specified <seealso cref="IApplicationBuilder"/>.
		/// </summary>
		public static IApplicationBuilder UseSharedLocalization(this IApplicationBuilder instance)
		{
			// Get the options
			var options = instance.ApplicationServices.GetService<IOptions<SharedLocalizerOptions>>()?.Value;

			// Configure the localization options
			instance.UseRequestLocalization(localizationOptions =>
			{
				var defaultCulture = new RequestCulture(options.DefaultCulture);
				var supportedCultures = options.SupportedCultures.Select(culture => new CultureInfo(culture)).ToList();

				localizationOptions.DefaultRequestCulture = defaultCulture;
				localizationOptions.SupportedCultures = supportedCultures;
				localizationOptions.SupportedUICultures = supportedCultures;
			});

			return instance;
		}
		#endregion
	}
}