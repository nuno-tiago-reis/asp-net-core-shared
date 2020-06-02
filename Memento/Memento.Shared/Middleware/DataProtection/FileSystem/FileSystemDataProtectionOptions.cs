namespace Memento.Shared.Middleware.DataProtection.FileSystem
{
	/// <summary>
	/// Implements the 'FileSystemDataProtection' options.
	/// </summary>
	public sealed class FileSystemDataProtectionOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the folder.
		/// </summary>
		public string Folder { get; set; }

		/// <summary>
		/// Gets or sets the certificate filename.
		/// </summary>
		public string CertificateFileName { get; set; }

		/// <summary>
		/// Gets or sets the certificate password.
		/// </summary>
		public string CertificatePassword { get; set; }
		#endregion
	}
}