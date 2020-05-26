using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Memento.Shared.Services.Templates
{
	/// <summary>
	/// Implements the necessary methods to add the <see cref="RazorTemplateService"/> to the ASP.NET Core Dependency Injection.
	/// </summary>
	public static class RazorTemplateServiceExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Adds the <see cref="RazorTemplateService"/> to the ASP.NET Core Dependency Injection to the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{RazorTemplateOptions}"/>
		/// </summary>
		/// 
		/// <param name="action">The action that configures the <seealso cref="RazorTemplateOptions"/>.</param>
		public static IServiceCollection AddRazorTemplateService(this IServiceCollection instance, Action<RazorTemplateOptions> action = null)
		{
			// Register the service
			instance.AddScoped<ITemplateService, RazorTemplateService>();

			// Configure the options
			instance.Configure<RazorViewEngineOptions>(options =>
			{
				// Create the options
				var templateOptions = new RazorTemplateOptions();
				// Configure the options
				action?.Invoke(templateOptions);

				foreach (var provider in templateOptions.FileProviders)
				{
					options.FileProviders.Add(provider);
				}
				foreach (var expander in templateOptions.ViewLocationExpanders)
				{
					options.ViewLocationExpanders.Add(expander);
				}
				foreach (var format in templateOptions.AreaViewLocationFormats)
				{
					options.AreaViewLocationFormats.Add(format);
				}
				foreach (var format in templateOptions.ViewLocationFormats)
				{
					options.ViewLocationFormats.Add(format);
				}
				foreach (var format in templateOptions.AreaPageViewLocationFormats)
				{
					options.AreaPageViewLocationFormats.Add(format);
				}
				foreach (var format in templateOptions.PageViewLocationFormats)
				{
					options.PageViewLocationFormats.Add(format);
				}

				options.AllowRecompilingViewsOnFileChange = templateOptions.AllowRecompilingViewsOnFileChange;
			});

			return instance;
		}
		#endregion
	}
}