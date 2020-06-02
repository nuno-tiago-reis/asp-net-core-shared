using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;

namespace Memento.Shared.Services.Localization.Shared
{
	/// <summary>
	/// Implements the necessary methods to add the <see cref="SharedLocalizerServiceExtensions"/> to the ASP.NET Core Dependency Injection.
	/// </summary>
	[UsedImplicitly]
	public static class SharedLocalizerServiceExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the <see cref="SharedLocalizerService{T}"/> in the pipeline of the specified <seealso cref="IMvcBuilder"/>.
		/// Uses the specified <seealso cref="SharedLocalizerOptions"/>
		/// </summary>
		///
		/// <typeparam name="T">The shared resources type.</typeparam>
		///
		/// <param name="builder">The mvc builder.</param>
		/// <param name="options">The options.</param>
		[UsedImplicitly]
		public static IMvcBuilder AddSharedLocalization<T>(this IMvcBuilder builder, SharedLocalizerOptions options) where T : class
		{
			// Validate the options
			if (options == null)
			{
				throw new ArgumentException($"The {nameof(options)} are invalid.");
			}

			// Validate the default culture
			if (string.IsNullOrWhiteSpace(options.DefaultCulture))
			{
				throw new ArgumentException($"The {nameof(options.DefaultCulture)} parameter is invalid.");
			}

			// Validate the supported cultures
			if (options.SupportedCultures == null || options.SupportedCultures.Length == 0)
			{
				throw new ArgumentException($"The {nameof(options.SupportedCultures)} parameter is invalid.");
			}

			// Register the default service
			builder.Services.AddLocalization();

			// Register the service
			builder.Services.AddScoped<ILocalizerService, SharedLocalizerService<T>>();

			// Configure the options
			builder.Services.AddSingleton(options);

			// Register the data annotations provider
			builder.AddDataAnnotationsLocalization(annotationOptions =>
			{
				annotationOptions.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(T));
			});

			// Configure the localization options
			builder.Services.Configure<RequestLocalizationOptions>(localizationOptions =>
			{
				var defaultCulture = new RequestCulture(options.DefaultCulture);
				var supportedCultures = options.SupportedCultures.Select(culture => new CultureInfo(culture)).ToList();

				localizationOptions.DefaultRequestCulture = defaultCulture;
				localizationOptions.SupportedCultures = supportedCultures;
				localizationOptions.SupportedUICultures = supportedCultures;
			});

			return builder;
		}

		/// <summary>
		/// Registers the <see cref="SharedLocalizerService{T}"/> in the pipeline of the specified <seealso cref="IMvcBuilder"/>.
		/// Configures the options using specified <seealso cref="Action{SharedLocalizerOptions}"/>
		/// </summary>
		///
		/// <typeparam name="T">The shared resources type.</typeparam>
		///
		/// <param name="builder">The mvc builder.</param>
		/// <param name="action">The action that configures the <seealso cref="SharedLocalizerOptions"/>.</param>
		[UsedImplicitly]
		public static IMvcBuilder AddSharedLocalization<T>(this IMvcBuilder builder, Action<SharedLocalizerOptions> action) where T : class
		{
			// Create the options
			var options = new SharedLocalizerOptions();
			// Configure the options
			action?.Invoke(options);

			// Register the service
			builder.AddSharedLocalization<T>(options);

			return builder;
		}

		/// <summary>
		/// Registers the <see cref="SharedLocalizerService{T}"/> in the pipeline of the specified <seealso cref="IServiceCollection"/>.
		/// Uses the specified <seealso cref="SharedLocalizerOptions"/>
		/// </summary>
		///
		/// <typeparam name="T">The shared resources type.</typeparam>
		///
		/// <param name="services">The service collection.</param>
		/// <param name="options">The options.</param>
		[UsedImplicitly]
		public static IServiceCollection AddSharedLocalization<T>(this IServiceCollection services, SharedLocalizerOptions options) where T : class
		{
			// Validate the options
			if (options == null)
			{
				throw new ArgumentException($"The {nameof(options)} are invalid.");
			}

			// Validate the default culture
			if (string.IsNullOrWhiteSpace(options.DefaultCulture))
			{
				throw new ArgumentException($"The {nameof(options.DefaultCulture)} parameter is invalid.");
			}

			// Validate the supported cultures
			if (options.SupportedCultures == null || options.SupportedCultures.Length == 0)
			{
				throw new ArgumentException($"The {nameof(options.SupportedCultures)} parameter is invalid.");
			}

			// Register the default service
			services.AddLocalization();

			// Register the service
			services.AddScoped<ILocalizerService, SharedLocalizerService<T>>();

			// Configure the options
			services.AddSingleton(options);

			// Configure the localization options
			services.Configure<RequestLocalizationOptions>(localizationOptions =>
			{
				var defaultCulture = new RequestCulture(options.DefaultCulture);
				var supportedCultures = options.SupportedCultures.Select(culture => new CultureInfo(culture)).ToList();

				localizationOptions.DefaultRequestCulture = defaultCulture;
				localizationOptions.SupportedCultures = supportedCultures;
				localizationOptions.SupportedUICultures = supportedCultures;
			});

			return services;
		}

		/// <summary>
		/// Registers the <see cref="SharedLocalizerService{T}"/> in the pipeline of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{SharedLocalizerOptions}"/>
		/// </summary>
		///
		/// <typeparam name="T">The shared resources type.</typeparam>
		///
		/// <param name="services">The service collection.</param>
		/// <param name="action">The action that configures the <seealso cref="SharedLocalizerOptions"/>.</param>
		[UsedImplicitly]
		public static IServiceCollection AddSharedLocalization<T>(this IServiceCollection services, Action<SharedLocalizerOptions> action) where T : class
		{
			// Create the options
			var options = new SharedLocalizerOptions();
			// Configure the options
			action?.Invoke(options);

			// Register the service
			services.AddSharedLocalization<T>(options);

			return services;
		}

		/// <summary>
		/// Registers the <see cref="SharedLocalizerService{T}"/> in the pipeline of the specified <seealso cref="IApplicationBuilder"/>.
		/// Uses the specified <seealso cref="SharedLocalizerOptions"/>
		/// </summary>
		///
		/// <param name="builder">The application builder.</param>
		/// <param name="options">The options.</param>
		[UsedImplicitly]
		public static IApplicationBuilder UseSharedLocalization(this IApplicationBuilder builder, SharedLocalizerOptions options)
		{
			// Configure the localization options
			builder.UseRequestLocalization(localizationOptions =>
			{
				var defaultCulture = new RequestCulture(options.DefaultCulture);
				var supportedCultures = options.SupportedCultures.Select(culture => new CultureInfo(culture)).ToList();

				localizationOptions.DefaultRequestCulture = defaultCulture;
				localizationOptions.SupportedCultures = supportedCultures;
				localizationOptions.SupportedUICultures = supportedCultures;
			});

			return builder;
		}

		/// <summary>
		/// Registers the <see cref="SharedLocalizerService{T}"/> in the pipeline of the specified <seealso cref="IApplicationBuilder"/>.
		/// Configures the options using specified <seealso cref="Action{SharedLocalizerOptions}"/>
		/// </summary>
		///
		/// <param name="builder">The application builder.</param>
		/// <param name="action">The action that configures the <seealso cref="SharedLocalizerOptions"/>.</param>
		[UsedImplicitly]
		public static IApplicationBuilder UseSharedLocalization(this IApplicationBuilder builder, Action<SharedLocalizerOptions> action)
		{
			// Create the options
			var options = new SharedLocalizerOptions();
			// Configure the options
			action?.Invoke(options);

			// Register the service
			builder.UseSharedLocalization(options);

			return builder;
		}
		#endregion
	}
}