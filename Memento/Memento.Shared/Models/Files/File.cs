namespace Memento.Shared.Models.Files
{
	/// <summary>
	/// Implements the generic interface for a file.
	/// Provides properties to query the file.
	/// </summary>
	public sealed class File : IFile
	{
		#region [Properties]
		/// <summary>
		/// The file (base64).
		/// </summary>
		public string FileBase64 { get; set; }

		/// <summary>
		/// The file name.
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// The file content type.
		/// </summary>
		public string FileContentType { get; set; }
		#endregion
	}
}