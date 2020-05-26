using Memento.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Storage
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
		public async Task<string> CreateAsync(IFormFile file, string fileName = null)
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
		public async Task<string> UpdateAsync(IFormFile file, string fileName = null)
		{
			try
			{
				// Get the container reference
				var container = await this.GetCloudBlobContainerAsync();

				// Delete the blob
				await this.DeleteCloudBlobAsync(container, fileName ?? file.FileName);

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
		/// <param name="file">The file.</param>
		/// <param name="fileName">The file name (optional, only if it should be override the file).</param>
		private async Task<CloudBlockBlob> CreateCloudBlobAsync(CloudBlobContainer container, IFormFile file, string fileName = null)
		{
			// Get the blob reference
			var blob = container.GetBlockBlobReference(fileName ?? file.FileName);

			// Upload the blob
			await blob.UploadFromStreamAsync(file.OpenReadStream());

			// Update the blobs properties
			blob.Properties.ContentDisposition = file.ContentDisposition;
			blob.Properties.ContentType = file.ContentType;

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