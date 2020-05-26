using System;

namespace Memento.Shared.Middleware.DataProtection
{
	/// <summary>
	/// Implements the 'AzureDataProtection' options.
	/// </summary>
	public sealed class AzureDataProtectionOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the storage settings.
		/// </summary>
		public AzureDataProtectionKeyVaultOptions KeyVault { get; set; }

		/// <summary>
		/// Gets or sets the storage settings.
		/// </summary>
		public AzureDataProtectionStorageOptions Storage { get; set; }
		#endregion
	}

	/// <summary>
	/// Implements the 'AzureDataProtectionKeyVault' options.
	/// </summary>
	public sealed class AzureDataProtectionKeyVaultOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the key identifier.
		/// </summary>
		public string KeyId { get; set; }

		/// <summary>
		/// Gets or sets the key lifetime.
		/// </summary>
		public TimeSpan? KeyLifetime { get; set; }

		/// <summary>
		/// Gets or sets the client identifier.
		/// </summary>
		public string ClientId { get; set; }

		/// <summary>
		/// Gets or sets the client secret
		/// </summary>
		public string ClientSecret { get; set; }
		#endregion
	}

	/// <summary>
	/// Implements the 'AzureDataProtectionStorage' options.
	/// </summary>
	public sealed class AzureDataProtectionStorageOptions
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