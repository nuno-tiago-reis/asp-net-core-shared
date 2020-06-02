using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Memento.Shared.Services.Templates
{
	/// <summary>
	/// Implements the necessary methods to add the <see cref="RazorTemplateService"/> to the ASP.NET Core Dependency Injection.
	/// </summary>
	[UsedImplicitly]
	public static class RazorTemplateServiceExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the <see cref="RazorTemplateService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Uses the specified <seealso cref="RazorTemplateOptions"/>
		/// </summary>
		///
		/// <param name="services">The service collection.</param>
		/// <param name="options">The options.</param>
		[UsedImplicitly]
		public static IServiceCollection AddRazorTemplateService(this IServiceCollection services, RazorTemplateOptions options)
		{
			// Validate the options
			if (options == null)
			{
				throw new ArgumentException($"The {nameof(options)} are invalid.");
			}

			// Register the service
			services.AddScoped<ITemplateService, RazorTemplateService>();

			// Configure the options
			services.Configure<RazorViewEngineOptions>(templateOptions =>
			{
				foreach (var provider in options.FileProviders)
				{
					templateOptions.FileProviders.Add(provider);
				}
				foreach (var expander in options.ViewLocationExpanders)
				{
					templateOptions.ViewLocationExpanders.Add(expander);
				}
				foreach (var format in options.AreaViewLocationFormats)
				{
					templateOptions.AreaViewLocationFormats.Add(format);
				}
				foreach (var format in options.ViewLocationFormats)
				{
					templateOptions.ViewLocationFormats.Add(format);
				}
				foreach (var format in options.AreaPageViewLocationFormats)
				{
					templateOptions.AreaPageViewLocationFormats.Add(format);
				}
				foreach (var format in options.PageViewLocationFormats)
				{
					templateOptions.PageViewLocationFormats.Add(format);
				}

				templateOptions.AllowRecompilingViewsOnFileChange = options.AllowRecompilingViewsOnFileChange;
			});

			return services;
		}

		/// <summary>
		/// Registers the <see cref="RazorTemplateService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{RazorTemplateOptions}"/>
		/// </summary>
		///
		/// <param name="services">The service collection.</param>
		/// <param name="action">The action that configures the <seealso cref="RazorTemplateOptions"/>.</param>
		[UsedImplicitly]
		public static IServiceCollection AddRazorTemplateService(this IServiceCollection services, Action<RazorTemplateOptions> action)
		{
			// Create the options
			var options = new RazorTemplateOptions();
			// Configure the options
			action?.Invoke(options);

			// Register the service
			services.AddRazorTemplateService(options);

			return services;
		}
		#endregion
	}
}