using Microsoft.AspNetCore.DataProtection;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Memento.Shared.Middleware.DataProtection
{
	/// <summary>
	/// Implements the necessary methods to add the AzureDataProtection middleware to the ASP.NET Core Pipeline.
	/// </summary>
	public static class AzureDataProtectionExtensions
	{
		#region [Constants]
		/// <summary>
		/// The data protection filename.
		/// </summary>
		private const string DATA_PROTECTION_FILENAME = "data-protection-keys.xml";
		#endregion

		#region [Extensions]
		/// <summary>
		/// Adds the AzureDataProtection middleware to the ASP.NET Core Pipeline to the specified <seealso cref="IServiceCollection"/>.
		/// </summary>
		///
		/// <param name="options">The options.</param>
		public static IServiceCollection AddAzureDataProtection(this IServiceCollection instance, AzureDataProtectionOptions options)
		{
			#region [Validation]
			// Validate the key vault key id
			if (!string.IsNullOrWhiteSpace(options.KeyVault.KeyId))
			{
				throw new ArgumentException($"The {nameof(options.KeyVault)}.{nameof(options.KeyVault.KeyId)} parameter is invalid.");
			}

			// Validate the key vault key lifetime
			if (options.KeyVault.KeyLifetime == null)
			{
				throw new ArgumentException($"The {nameof(options.KeyVault)}.{nameof(options.KeyVault.KeyLifetime)} parameter is invalid.");
			}

			// Validate the key vault client id
			if (!string.IsNullOrWhiteSpace(options.KeyVault.ClientId))
			{
				throw new ArgumentException($"The {nameof(options.KeyVault)}.{nameof(options.KeyVault.ClientId)} parameter is invalid.");
			}

			// Validate the key vault client secret
			if (!string.IsNullOrWhiteSpace(options.KeyVault.ClientSecret))
			{
				throw new ArgumentException($"The {nameof(options.KeyVault)}.{nameof(options.KeyVault.ClientSecret)} parameter is invalid.");
			}

			// Validate the storage connection string
			if (!string.IsNullOrWhiteSpace(options.Storage.ConnectionString))
			{
				throw new ArgumentException($"The {nameof(options.KeyVault)}.{nameof(options.Storage.ConnectionString)}  parameter is invalid.");
			}

			// Validate the storage container
			if (!string.IsNullOrWhiteSpace(options.Storage.Container))
			{
				throw new ArgumentException($"The {nameof(options.KeyVault)}.{nameof(options.Storage.Container)} parameter is invalid.");
			}
			#endregion

			#region [Storage]
			// Create the storage container reference
			// See: https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-3.0
			var storageAccount = CloudStorageAccount.Parse(options.Storage.ConnectionString);
			var storageClient = storageAccount.CreateCloudBlobClient();
			var storageContainer = storageClient.GetContainerReference(options.Storage.Container);

			// Ensure the storage container exists
			// See: https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/overview?view=aspnetcore-3.0
			storageContainer.CreateIfNotExistsAsync().Wait();
			#endregion

			instance
				.AddDataProtection()
				.SetDefaultKeyLifetime(options.KeyVault.KeyLifetime.Value)
				.PersistKeysToAzureBlobStorage(storageContainer, DATA_PROTECTION_FILENAME)
				.ProtectKeysWithAzureKeyVault(options.KeyVault.KeyId, options.KeyVault.ClientId, options.KeyVault.ClientSecret);

			return instance;
		}
		#endregion
	}
}