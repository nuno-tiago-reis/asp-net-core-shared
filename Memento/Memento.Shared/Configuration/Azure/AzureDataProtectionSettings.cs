using System;

namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the 'AzureDataProtection' settings.
	/// </summary>
	public sealed class AzureDataProtectionSettings
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the storage settings.
		/// </summary>
		public AzureDataProtectionKeyVaultSettings KeyVault { get; set; }

		/// <summary>
		/// Gets or sets the storage settings.
		/// </summary>
		public AzureDataProtectionStorageSettings Storage { get; set; }
		#endregion
	}

	/// <summary>
	/// Implements the 'AzureDataProtectionKeyVault' settings.
	/// </summary>
	public sealed class AzureDataProtectionKeyVaultSettings
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the key identifier.
		/// </summary>
		public string KeyId { get; set; }

		/// <summary>
		/// Gets or sets the key lifetime.
		/// </summary>
		public TimeSpan KeyLifetime { get; set; }
		#endregion
	}

	/// <summary>
	/// Implements the 'AzureDataProtectionStorage' settings.
	/// </summary>
	public sealed class AzureDataProtectionStorageSettings
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