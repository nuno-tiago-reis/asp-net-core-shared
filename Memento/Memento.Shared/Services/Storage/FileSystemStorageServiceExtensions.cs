using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Memento.Shared.Services.Storage
{
	/// <summary>
	/// Implements the necessary methods to add the <see cref="FileSystemStorageService"/> to the ASP.NET Core Dependency Injection.
	/// </summary>
	[UsedImplicitly]
	public static class FileSystemStorageServiceExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the <see cref="FileSystemStorageService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Uses the specified <seealso cref="FileSystemStorageOptions"/>
		/// </summary>
		///
		/// <param name="services">The service collection.</param>
		/// <param name="options">The options.</param>
		[UsedImplicitly]
		public static IServiceCollection AddFileSystemStorageService(this IServiceCollection services, FileSystemStorageOptions options)
		{
			// Validate the options
			if (options == null)
			{
				throw new ArgumentException($"The {nameof(options)} are invalid.");
			}

			// Validate the folder
			if (string.IsNullOrWhiteSpace(options.Folder))
			{
				throw new ArgumentException($"The {nameof(options.Folder)} parameter is invalid.");
			}

			// Register the http context accessor
			services.AddHttpContextAccessor();

			// Register the service
			services.AddScoped<IStorageService, FileSystemStorageService>();

			// Configure the options
			services.AddSingleton(options);

			return services;
		}

		/// <summary>
		/// Registers the <see cref="FileSystemStorageService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{FileSystemStorageOptions}"/>
		/// </summary>
		///
		/// <param name="services">The service collection.</param>
		/// <param name="action">The action that configures the <seealso cref="FileSystemStorageOptions"/>.</param>
		[UsedImplicitly]
		public static IServiceCollection AddFileSystemStorageService(this IServiceCollection services, Action<FileSystemStorageOptions> action)
		{
			// Create the options
			var options = new FileSystemStorageOptions();
			// Configure the options
			action?.Invoke(options);

			// Register the service
			services.AddFileSystemStorageService(options);

			return services;
		}
		#endregion
	}
}