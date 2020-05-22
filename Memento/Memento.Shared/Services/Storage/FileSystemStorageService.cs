using Memento.Shared.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Storage
{
	/// <summary>
	/// Implements the generic interface for a storage services.
	/// Provides methods to interact with the storage (CRUD and more).
	/// </summary>
	public sealed class FileSystemStorageService : IStorageService
	{
		#region [Properties]
		/// <summary>
		/// The storage settings.
		/// </summary>
		private readonly FileSystemStorageSettings Settings;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="AzureStorageService"/> class.
		/// </summary>
		/// 
		/// <param name="settings">The settings.</param>
		public FileSystemStorageService(IOptions<FileSystemStorageSettings> settings)
		{
			this.Settings = settings.Value;
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		public Task<string> CreateAsync(IFormFile file, string fileName = null)
		{
			throw new System.NotImplementedException();
		}

		/// <inheritdoc />
		public Task<string> UpdateAsync(IFormFile file, string fileName = null)
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