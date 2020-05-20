namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the 'DataProtection' settings.
	/// </summary>
	public sealed class DataProtectionSettings
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the blob storage account.
		/// </summary>
		public string BlobStorageAccount { get; set; }

		/// <summary>
		/// Gets or sets the blob storage container.
		/// </summary>
		public string BlobStorageContainer { get; set; }

		/// <summary>
		/// Gets or sets the key vault key identifier.
		/// </summary>
		public string KeyVaultKeyId { get; set; }
		#endregion
	}
}