namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the 'FileSystemDataProtection' settings.
	/// </summary>
	public sealed class FileSystemDataProtectionSettings
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