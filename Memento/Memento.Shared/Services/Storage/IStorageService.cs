using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Storage
{
	/// <summary>
	/// Defines a generic interface for a storage services.
	/// Provides methods to interact with the storage (CRUD and more).
	/// </summary>
	public interface IStorageService
	{
		#region [Methods]
		/// <summary>
		/// Creates a file in the storage provider.
		/// </summary>
		/// 
		/// <param name="file">The file.</param>
		/// <param name="fileName">The file name (optional, only if it should be override the file).</param>
		Task<string> CreateAsync(string file, string fileName);

		/// <summary>
		/// Updates a file in the storage provider.
		/// </summary>
		/// 
		/// <param name="file">The file.</param>
		/// <param name="fileName">The file name (optional, only if it should be override the file).</param>
		Task<string> UpdateAsync(string file, string fileName);

		/// <summary>
		/// Gets a file from the storage provider.
		/// </summary>
		/// 
		/// <param name="containerName">The container name.</param>
		/// <param name="fileName">The file name.</param>
		Task<Stream> GetAsync(string fileName);

		/// <summary>
		/// Delets a file from the storage provider.
		/// </summary>
		/// 
		/// <param name="containerName">The container name.</param>
		/// <param name="fileName">The container file name.</param>
		Task DeleteAsync(string fileName);
		#endregion
	}
}