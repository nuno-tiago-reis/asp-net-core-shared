using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Memento.Shared.Middleware.DataProtection
{
	/// <summary>
	/// Implements the necessary methods to add the FileSystemDataProtection middleware to the ASP.NET Core Pipeline.
	/// </summary>
	public static class FileSystemDataProtectionExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the FileSystemDataProtection middleware in the pipeline of the specified <seealso cref="IServiceCollection"/>.
		/// Uses the specified <seealso cref="FileSystemDataProtectionOptions"/>
		/// </summary>
		/// 
		/// <param name="options">The options.</param>
		public static IServiceCollection AddFileSystemDataProtection(this IServiceCollection instance, FileSystemDataProtectionOptions options)
		{
			#region [Validation]
			// Validate the options
			if (options == null)
			{
				throw new ArgumentException($"The {nameof(options)} are invalid.");
			}

			// Validate the certificate filename
			if (!string.IsNullOrWhiteSpace(options.CertificateFileName))
			{
				throw new ArgumentException($"The {nameof(options.CertificateFileName)} parameter is invalid.");
			}

			// Validate the certificate password
			if (!string.IsNullOrWhiteSpace(options.CertificatePassword))
			{
				throw new ArgumentException($"The {nameof(options.CertificatePassword)} parameter is invalid.");
			}

			// Validate the folder
			if (!string.IsNullOrWhiteSpace(options.Folder))
			{
				throw new ArgumentException($"The {nameof(options.Folder)} parameter is invalid.");
			}
			#endregion

			instance
				.AddDataProtection()
				.PersistKeysToFileSystem(new DirectoryInfo(options.Folder))
				.ProtectKeysWithCertificate(new X509Certificate2(options.CertificateFileName, options.CertificatePassword));

			return instance;
		}

		/// <summary>
		/// Registers the FileSystemDataProtection middleware in the pipeline of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{FileSystemDataProtectionOptions}"/>
		/// </summary>
		/// 
		/// <param name="action">The action that configures the <seealso cref="FileSystemDataProtectionOptions"/>.</param>
		public static IServiceCollection AddFileSystemDataProtection(this IServiceCollection services, Action<FileSystemDataProtectionOptions> action)
		{
			// Create the options
			var options = new FileSystemDataProtectionOptions();
			// Configure the options
			action?.Invoke(options);

			// Register the service
			services.AddFileSystemDataProtection(options);

			return services;
		}
		#endregion
	}
}