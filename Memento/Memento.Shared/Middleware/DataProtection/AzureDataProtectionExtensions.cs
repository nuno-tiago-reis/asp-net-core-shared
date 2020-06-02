using JetBrains.Annotations;
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
	[UsedImplicitly]
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
		/// Registers the AzureDataProtection middleware in the pipeline of the specified <seealso cref="IServiceCollection"/>.
		/// Uses the specified <seealso cref="AzureDataProtectionOptions"/>
		/// </summary>
		///
		/// <param name="services">The service collection.</param>
		/// <param name="options">The options.</param>
		[UsedImplicitly]
		public static IServiceCollection AddAzureDataProtection(this IServiceCollection services, AzureDataProtectionOptions options)
		{
			#region [Validation]
			// Validate the options
			if (options == null)
			{
				throw new ArgumentException($"The {nameof(options)} are invalid.");
			}

			// Validate the key vault key id
			if (string.IsNullOrWhiteSpace(options.KeyVault?.KeyId))
			{
				throw new ArgumentException($"The {nameof(options.KeyVault)}.{nameof(options.KeyVault.KeyId)} parameter is invalid.");
			}

			// Validate the key vault key lifetime
			if (options.KeyVault?.KeyLifetime == null)
			{
				throw new ArgumentException($"The {nameof(options.KeyVault)}.{nameof(options.KeyVault.KeyLifetime)} parameter is invalid.");
			}

			// Validate the key vault client id
			if (string.IsNullOrWhiteSpace(options.KeyVault?.ClientId))
			{
				throw new ArgumentException($"The {nameof(options.KeyVault)}.{nameof(options.KeyVault.ClientId)} parameter is invalid.");
			}

			// Validate the key vault client secret
			if (string.IsNullOrWhiteSpace(options.KeyVault?.ClientSecret))
			{
				throw new ArgumentException($"The {nameof(options.KeyVault)}.{nameof(options.KeyVault.ClientSecret)} parameter is invalid.");
			}

			// Validate the storage connection string
			if (string.IsNullOrWhiteSpace(options.Storage?.ConnectionString))
			{
				throw new ArgumentException($"The {nameof(options.KeyVault)}.{nameof(options.Storage.ConnectionString)}  parameter is invalid.");
			}

			// Validate the storage container
			if (string.IsNullOrWhiteSpace(options.Storage?.Container))
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

			services
				.AddDataProtection()
				.SetDefaultKeyLifetime(options.KeyVault.KeyLifetime.Value)
				.PersistKeysToAzureBlobStorage(storageContainer, DATA_PROTECTION_FILENAME)
				.ProtectKeysWithAzureKeyVault(options.KeyVault.KeyId, options.KeyVault.ClientId, options.KeyVault.ClientSecret);

			return services;
		}

		/// <summary>
		/// Registers the AzureDataProtection middleware in the pipeline of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{AzureDataProtectionOptions}"/>
		/// </summary>
		///
		/// <param name="services">The service collection.</param>
		/// <param name="action">The action that configures the <seealso cref="AzureDataProtectionOptions"/>.</param>
		[UsedImplicitly]
		public static IServiceCollection AddAzureDataProtection(this IServiceCollection services, Action<AzureDataProtectionOptions> action)
		{
			// Create the options
			var options = new AzureDataProtectionOptions();
			// Configure the options
			action?.Invoke(options);

			// Register the service
			services.AddAzureDataProtection(options);

			return services;
		}
		#endregion
	}
}