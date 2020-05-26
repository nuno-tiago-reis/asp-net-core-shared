using Microsoft.Extensions.DependencyInjection;
using Sotsera.Blazor.Toaster;
using System;

namespace Memento.Shared.Services.Toaster
{
	/// <summary>
	/// Implements the necessary methods to add the <see cref="IToaster"/> service to the ASP.NET Core Pipeline.
	/// </summary>
	public static class ToasterExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the <see cref="IToaster"/> service in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Uses the specified <seealso cref="ToasterOptions"/>
		/// </summary>
		/// 
		/// <param name="options">The options.</param>
		public static IServiceCollection AddToasterService(this IServiceCollection services, ToasterOptions options = null)
		{
			// Register the service
			services.AddToaster(options ?? new ToasterOptions());

			return services;
		}

		/// <summary>
		/// Registers the <see cref="IToaster"/> service in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{ToasterOptions}"/>
		/// </summary>
		/// 
		/// <param name="action">The action that configures the <seealso cref="ToasterOptions"/>.</param>
		public static IServiceCollection AddToasterService(this IServiceCollection services, Action<ToasterOptions> action)
		{
			// Create the options
			var options = new ToasterOptions();
			// Configure the options
			action?.Invoke(options);

			// Register the service
			services.AddToaster(options);

			return services;
		}
		#endregion
	}
}