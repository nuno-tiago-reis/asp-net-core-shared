using Microsoft.Extensions.DependencyInjection;
using System;

namespace Memento.Shared.Services.Storage
{
	/// <summary>
	/// Implements the necessary methods to add the <see cref="FileSystemStorageService"/> to the ASP.NET Core Dependency Injection.
	/// </summary>
	public static class FileSystemStorageServiceExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the <see cref="FileSystemStorageService"/> in the dependency injection mechanism of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{FileSystemStorageOptions}"/>
		/// </summary>
		/// 
		/// <param name="action">The action that configures the <seealso cref="FileSystemStorageOptions"/>.</param>
		public static IServiceCollection AddFileSystemStorageService(this IServiceCollection instance, Action<FileSystemStorageOptions> action = null)
		{
			// Register the service
			instance.AddScoped<IStorageService, FileSystemStorageService>();

			// Configure the options
			instance.Configure<FileSystemStorageOptions>(options =>
			{
				action?.Invoke(options);

				// Validate the folder
				if (!string.IsNullOrWhiteSpace(options.Folder))
				{
					throw new ArgumentException($"The {nameof(options.Folder)} parameter is invalid.");
				}
			});

			return instance;
		}
		#endregion
	}
}