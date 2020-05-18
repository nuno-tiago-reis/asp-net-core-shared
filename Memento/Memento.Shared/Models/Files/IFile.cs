using JetBrains.Annotations;

namespace Memento.Shared.Models.Files
{
	/// <summary>
	/// Defines a generic interface for a file.
	/// Provides properties to query the file.
	/// </summary>
	[UsedImplicitly]
	public interface IFile
	{
		#region [Properties]
		/// <summary>
		/// The file (base64).
		/// </summary>
		[UsedImplicitly]
		string FileBase64 { get; set; }

		/// <summary>
		/// The file name.
		/// </summary>
		[UsedImplicitly]
		string FileName { get; set; }

		/// <summary>
		/// The file content type.
		/// </summary>
		[UsedImplicitly]
		string FileContentType { get; set; }
		#endregion
	}
}