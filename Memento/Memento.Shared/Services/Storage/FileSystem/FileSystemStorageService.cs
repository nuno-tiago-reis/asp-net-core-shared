﻿using Microsoft.Extensions.Logging;
using System.IO;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Storage
{
	/// <summary>
	/// Implements the generic interface for a storage service using the File System Storage.
	/// Provides methods to interact with the storage (CRUD and more).
	/// </summary>
	public sealed class FileSystemStorageService : IStorageService
	{
		#region [Properties]
		/// <summary>
		/// The options.
		/// </summary>
		private readonly FileSystemStorageOptions Options;

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
		public FileSystemStorageService(FileSystemStorageOptions options, ILogger<FileSystemStorageService> logger)
		{
			this.Options = options;
			this.Logger = logger;
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		public Task<string> CreateAsync(string file, string fileName)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		public Task<string> UpdateAsync(string file, string fileName)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		public Task<Stream> GetAsync(string fileName)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		public Task DeleteAsync(string fileName)
		{
			throw new System.NotImplementedException();
		}
		#endregion
	}
}