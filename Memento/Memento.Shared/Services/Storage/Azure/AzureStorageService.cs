using Memento.Shared.Exceptions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Storage.Azure
{
	/// <summary>
	/// Implements the generic interface for a storage service using the Azure Storage.
	/// Provides methods to interact with the storage (CRUD and more).
	/// </summary>
	public sealed class AzureStorageService : IStorageService
	{
		#region [Properties]
		/// <summary>
		/// The options.
		/// </summary>
		private readonly AzureStorageOptions Options;

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly ILogger Logger;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="AzureStorageService"/> class.
		/// </summary>
		/// 
		/// <param name="options">The options.</param>
		/// <param name="logger">The logger.</param>
		public AzureStorageService(AzureStorageOptions options, ILogger<AzureStorageService> logger)
		{
			this.Options = options;
			this.Logger = logger;
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		public async Task<string> CreateAsync(string file, string fileName)
		{
			try
			{
				// Get the container reference
				var container = await this.GetCloudBlobContainerAsync();

				// Create the blob
				var blob = await this.CreateCloudBlobAsync(container, file, fileName);

				// Return the blob uri
				return blob.Uri.ToString();
			}
			catch (Exception exception)
			{
				// Log the exception
				this.Logger.LogError(exception.Message, exception);

				// Wrap the exception
				throw new MementoException(exception.Message, exception, MementoExceptionType.InternalServerError);
			}
		}

		/// <inheritdoc />
		public async Task<string> UpdateAsync(string file, string fileName)
		{
			try
			{
				// Get the container reference
				var container = await this.GetCloudBlobContainerAsync();

				// Delete the blob
				await this.DeleteCloudBlobAsync(container, fileName);

				// Create the blob
				var blob = await this.CreateCloudBlobAsync(container, file, fileName);
				
				// Return the blob uri
				return blob.Uri.ToString();
			}
			catch (Exception exception)
			{
				// Log the exception
				this.Logger.LogError(exception.Message, exception);

				// Wrap the exception
				throw new MementoException(exception.Message, exception, MementoExceptionType.InternalServerError);
			}
		}

		/// <inheritdoc />
		public async Task DeleteAsync(string fileName)
		{
			try
			{
				// Get the container reference
				var container = await this.GetCloudBlobContainerAsync();

				// Delete the blob
				await this.DeleteCloudBlobAsync(container, fileName);
			}
			catch (Exception exception)
			{
				// Log the exception
				this.Logger.LogError(exception.Message, exception);

				// Wrap the exception
				throw new MementoException(exception.Message, exception, MementoExceptionType.InternalServerError);
			}
		}

		/// <inheritdoc />
		public async Task<Stream> GetAsync(string fileName)
		{
			try
			{
				// Get the container reference
				var container = await this.GetCloudBlobContainerAsync();

				// Get the blob
				var blob = await this.GetCloudBlobAsync(container, fileName);

				// Return the blob stream
				return await blob.OpenReadAsync();
			}
			catch (Exception exception)
			{
				// Log the exception
				this.Logger.LogError(exception.Message, exception);

				// Wrap the exception
				throw new MementoException(exception.Message, exception, MementoExceptionType.InternalServerError);
			}
		}
		#endregion

		#region [Methods] Utility
		/// <summary>
		/// Gets a cloud blob container reference.
		/// </summary>
		private async Task<CloudBlobContainer> GetCloudBlobContainerAsync()
		{
			// Create the container reference
			var storageAccount = CloudStorageAccount.Parse(this.Options.ConnectionString);
			var storageAccountClient = storageAccount.CreateCloudBlobClient();
			var storageAccountContainer = storageAccountClient.GetContainerReference(this.Options.Container);

			// Ensure the container exists and set its permissions
			await storageAccountContainer.CreateIfNotExistsAsync();
			await storageAccountContainer.SetPermissionsAsync(new BlobContainerPermissions
			{
				PublicAccess = BlobContainerPublicAccessType.Blob
			});

			return storageAccountContainer;
		}

		/// <summary>
		/// Creates a cloud blob in the given container.
		/// </summary>
		/// 
		/// <param name="container">The container.</param>
		/// <param name="file">The file (base64).</param>
		/// <param name="fileName">The file name (optional, only if it should be override the file).</param>
		private async Task<CloudBlockBlob> CreateCloudBlobAsync(CloudBlobContainer container, string file, string fileName)
		{
			// Get the blob reference
			var blob = container.GetBlockBlobReference(fileName);

			// Convert the file
			var bytes = Convert.FromBase64String(file);

			// Upload the blob
			await blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length);

			// Update the blobs properties
			var provider = new FileExtensionContentTypeProvider();
			if (provider.TryGetContentType(fileName, out var fileContentType))
			{
				blob.Properties.ContentType = fileContentType;
			}
			else
			{
				blob.Properties.ContentType = "application/octet-stream";
			}

			// Upload the blobs properties
			await blob.SetPropertiesAsync();

			return blob;
		}

		/// <summary>
		/// Gets a cloud blob from the given container.
		/// </summary>
		/// 
		/// <param name="container">The container.</param>
		/// <param name="fileName">The file name.</param>
		private async Task<ICloudBlob> GetCloudBlobAsync(CloudBlobContainer container, string fileName)
		{
			// Get the blob reference
			var blob = await container.GetBlobReferenceFromServerAsync(fileName);

			return blob;
		}

		/// <summary>
		/// Deletes a cloud blob from the given container.
		/// </summary>
		/// 
		/// <param name="container">The container.</param>
		/// <param name="fileName">The file name.</param>
		private async Task DeleteCloudBlobAsync(CloudBlobContainer container, string fileName)
		{
			// Get the blob reference
			var blob = container.GetBlockBlobReference(fileName);

			// Delete the blob
			await blob.DeleteIfExistsAsync();
		}
		#endregion
	}
}