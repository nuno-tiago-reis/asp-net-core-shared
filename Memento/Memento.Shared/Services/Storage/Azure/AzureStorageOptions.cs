namespace Memento.Shared.Services.Storage.Azure
{
	/// <summary>
	/// Implements the 'AzureStorage' options.
	/// </summary>
	public sealed class AzureStorageOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the storage connection string.
		/// </summary>
		public string ConnectionString { get; set; }

		/// <summary>
		/// Gets or sets the storage container.
		/// </summary>
		public string Container { get; set; }
		#endregion
	}
}