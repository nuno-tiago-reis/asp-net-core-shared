using Memento.Shared.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Storage.FileSystem
{
	/// <summary>
	/// Implements the generic interface for a storage service using the File System Storage.
	/// Provides methods to interact with the storage (CRUD and more).
	/// </summary>
	public sealed class FileSystemStorageService : IStorageService
	{
		#region [Constants]
		/// <summary>
		/// The file does not exist message.
		/// </summary>
		private const string FILE_DOES_NOT_EXIST = "The file does not exist.";
		#endregion

		#region [Properties]
		/// <summary>
		/// The options.
		/// </summary>
		private readonly FileSystemStorageOptions Options;

		/// <summary>
		/// The environment.
		/// </summary>
		private readonly IHostingEnvironment Environment;

		/// <summary>
		/// The http context accessor.
		/// </summary>
		private readonly IHttpContextAccessor HttpContextAccessor;

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly ILogger Logger;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="FileSystemStorageService"/> class.
		/// </summary>
		/// 
		/// <param name="options">The options.</param>
		/// <param name="environment">The environment.</param>
		/// <param name="httpContextAccessor">The http context accessor.</param>
		/// <param name="logger">The logger.</param>
		public FileSystemStorageService
		(
			FileSystemStorageOptions options,
			IHostingEnvironment environment,
			IHttpContextAccessor httpContextAccessor,
			ILogger<FileSystemStorageService> logger
		)
		{
			this.Options = options;
			this.Environment = environment;
			this.HttpContextAccessor = httpContextAccessor;
			this.Logger = logger;
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		public async Task<string> CreateAsync(string file, string fileName)
		{
			try
			{
				// Create the file
				var url = await this.CreateLocalFileAsync(file, fileName);

				// Return the uri
				return url;
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
				// Delete the file
				await this.DeleteLocalFileAsync(fileName);

				// Create the file
				var url = await this.CreateLocalFileAsync(file, fileName);

				// Return the uri
				return url;
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
				// Delete the file
				await this.DeleteLocalFileAsync(fileName);
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
				// Get the file
				var stream = await this.GetLocalFileAsync(fileName);

				// Return the file stream
				return stream;
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
		/// Creates a file in the local storage.
		/// </summary>
		///
		/// <param name="file">The file (base64).</param>
		/// <param name="fileName">The file name (optional, only if it should be override the file).</param>
		private async Task<string> CreateLocalFileAsync(string file, string fileName)
		{
			// Create the folder path
			string folderPath = Path.Combine(this.Environment.WebRootPath, this.Options.Folder);
			// Create the file path
			string filePath = Path.Combine(folderPath, fileName);

			// Create the folder if missing
			if (!Directory.Exists(folderPath))
			{
				Directory.CreateDirectory(folderPath);
			}

			// Save the file in the folder
			await File.WriteAllBytesAsync(filePath, Convert.FromBase64String(file));

			// Build the server url
			var serverUrl = $"{this.HttpContextAccessor.HttpContext.Request.Scheme}://{this.HttpContextAccessor.HttpContext.Request.Host}";
			// Build the file url
			var serverFileUrl = Path.Combine(serverUrl, this.Options.Folder, fileName);

			return serverFileUrl;
		}

		/// <summary>
		/// Gets a file from the local storage.
		/// </summary>
		///
		/// <param name="fileName">The file name.</param>
		private async Task<FileStream> GetLocalFileAsync(string fileName)
		{
			// Create the folder path
			var folderPath = Path.Combine(this.Environment.WebRootPath, this.Options.Folder);
			// Create the file path
			var filePath = Path.Combine(folderPath, fileName);

			// Check if the file exists
			if (File.Exists(filePath))
			{
				return await Task.Run(() => File.OpenRead(filePath));
			}

			throw new IOException(FILE_DOES_NOT_EXIST);
		}

		/// <summary>
		/// Deletes a file from the local storage.
		/// </summary>
		/// 
		/// <param name="fileName">The file name.</param>
		private async Task DeleteLocalFileAsync(string fileName)
		{
			// Create the folder path
			var folderPath = Path.Combine(this.Environment.WebRootPath, this.Options.Folder);
			// Create the file path
			var filePath = Path.Combine(folderPath, fileName);

			// Check if the file exists
			if (File.Exists(filePath))
			{
				await Task.Run(() => File.Delete(filePath));
			}

			throw new IOException(FILE_DOES_NOT_EXIST);
		}
		#endregion
	}
}