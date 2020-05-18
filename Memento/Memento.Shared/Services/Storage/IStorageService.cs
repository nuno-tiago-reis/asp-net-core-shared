using JetBrains.Annotations;
using System.IO;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Storage
{
	/// <summary>
	/// Defines a generic interface for a storage services.
	/// Provides methods to interact with the storage (CRUD and more).
	/// </summary>
	[UsedImplicitly]
	public interface IStorageService
	{
		#region [Methods]
		/// <summary>
		/// Creates a file in the storage provider.
		/// </summary>
		/// 
		/// <param name="file">The file.</param>
		/// <param name="fileName">The file name (optional, only if it should be override the file).</param>
		[UsedImplicitly]
		Task<string> CreateAsync(string file, string fileName);

		/// <summary>
		/// Updates a file in the storage provider.
		/// </summary>
		/// 
		/// <param name="file">The file.</param>
		/// <param name="fileName">The file name (optional, only if it should be override the file).</param>
		[UsedImplicitly]
		Task<string> UpdateAsync(string file, string fileName);

		/// <summary>
		/// Deletes a file from the storage provider.
		/// </summary>
		///
		/// <param name="fileName">The container file name.</param>
		[UsedImplicitly]
		Task DeleteAsync(string fileName);

		/// <summary>
		/// Gets a file from the storage provider.
		/// </summary>
		///
		/// <param name="fileName">The file name.</param>
		[UsedImplicitly]
		Task<Stream> GetAsync(string fileName);
		#endregion
	}
}