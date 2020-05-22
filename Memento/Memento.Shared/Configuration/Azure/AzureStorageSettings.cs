namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the 'AzureStorage' settings.
	/// </summary>
	public sealed class AzureStorageSettings
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