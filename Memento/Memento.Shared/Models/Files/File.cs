using JetBrains.Annotations;

namespace Memento.Shared.Models.Files
{
	/// <summary>
	/// Implements the generic interface for a file.
	/// Provides properties to query the file.
	/// </summary>
	[UsedImplicitly]
	public sealed class File : IFile
	{
		#region [Properties]
		/// <summary>
		/// The file (base64).
		/// </summary>
		[UsedImplicitly]
		public string FileBase64 { get; set; }

		/// <summary>
		/// The file name.
		/// </summary>
		[UsedImplicitly]
		public string FileName { get; set; }

		/// <summary>
		/// The file content type.
		/// </summary>
		[UsedImplicitly]
		public string FileContentType { get; set; }
		#endregion
	}
}