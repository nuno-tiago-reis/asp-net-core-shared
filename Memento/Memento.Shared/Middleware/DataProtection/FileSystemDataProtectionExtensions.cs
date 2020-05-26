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
		/// Adds the FileSystemDataProtection middleware to the ASP.NET Core Pipeline to the specified <seealso cref="IServiceCollection"/>.
		/// </summary>
		///
		/// <param name="options">The options.</param>
		public static IServiceCollection AddFileSystemDataProtection(this IServiceCollection instance, FileSystemDataProtectionOptions options)
		{
			#region [Validation]
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
		#endregion
	}
}