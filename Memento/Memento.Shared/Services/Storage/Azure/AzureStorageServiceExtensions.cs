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
		/// Configures the options using specified <seealso cref="Action{AzureStorageOptions}"/>
		/// </summary>
		/// 
		/// <param name="action">The action that configures the <seealso cref="AzureStorageOptions"/>.</param>
		public static IServiceCollection AddAzureStorageService(this IServiceCollection instance, Action<AzureStorageOptions> action = null)
		{
			// Register the service
			instance.AddScoped<IStorageService, AzureStorageService>();

			// Configure the options
			instance.Configure<AzureStorageOptions>(options =>
			{
				action?.Invoke(options);

				// Validate the connection string
				if (!string.IsNullOrWhiteSpace(options.ConnectionString))
				{
					throw new ArgumentException($"The {nameof(options.ConnectionString)} parameter is invalid.");
				}

				// Validate the container
				if (!string.IsNullOrWhiteSpace(options.Container))
				{
					throw new ArgumentException($"The {nameof(options.Container)} parameter is invalid.");
				}
			});

			return instance;
		}
		#endregion
	}
}