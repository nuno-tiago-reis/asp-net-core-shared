namespace Memento.Shared.Models.Files
{
	/// <summary>
	/// Defines a generic interface for a file.
	/// Provides properties to query the file.
	/// </summary>
	public interface IFile
	{
		#region [Properties]
		/// <summary>
		/// The file (base64).
		/// </summary>
		string FileBase64 { get; set; }

		/// <summary>
		/// The file name.
		/// </summary>
		string FileName { get; set; }

		/// <summary>
		/// The file content type.
		/// </summary>
		string FileContentType { get; set; }
		#endregion
	}
}