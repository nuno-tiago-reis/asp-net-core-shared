using Microsoft.Extensions.DependencyInjection;
using System;

namespace Memento.Shared.Services.Storage
{
	/// <summary>
	/// Implements the necessary methods to add the <see cref="AzureStorageService"/> to the ASP.NET Core Dependency Injection.
	/// </summary>
	public static class AzureStorageServiceExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the <see cref="AzureStorageService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Uses the specified <seealso cref="AzureStorageOptions"/>
		/// </summary>
		/// 
		/// <param name="options">The options.</param>
		public static IServiceCollection AddAzureStorageService(this IServiceCollection services, AzureStorageOptions options)
		{
			// Validate the options
			if (options == null)
			{
				throw new ArgumentException($"The {nameof(options)} are invalid.");
			}

			// Validate the connection string
			if (string.IsNullOrWhiteSpace(options.ConnectionString))
			{
				throw new ArgumentException($"The {nameof(options.ConnectionString)} parameter is invalid.");
			}

			// Validate the container
			if (string.IsNullOrWhiteSpace(options.Container))
			{
				throw new ArgumentException($"The {nameof(options.Container)} parameter is invalid.");
			}

			// Register the service
			services.AddScoped<IStorageService, AzureStorageService>();

			// Configure the options
			services.AddSingleton(options);

			return services;
		}

		/// <summary>
		/// Registers the <see cref="AzureStorageService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{AzureStorageOptions}"/>
		/// </summary>
		/// 
		/// <param name="action">The action that configures the <seealso cref="AzureStorageOptions"/>.</param>
		public static IServiceCollection AddAzureStorageService(this IServiceCollection services, Action<AzureStorageOptions> action)
		{
			// Create the options
			var options = new AzureStorageOptions();
			// Configure the options
			action?.Invoke(options);

			// Register the service
			services.AddAzureStorageService(options);

			return services;
		}
		#endregion
	}
}