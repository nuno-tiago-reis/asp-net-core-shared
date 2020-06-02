namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the 'ExternalProvider' options.
	/// </summary>
	public sealed class ExternalProviderOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the api key.
		/// </summary>
		public string ApiKey { get; set; }

		/// <summary>
		/// Gets or sets the api secret.
		/// </summary>
		public string ApiSecret { get; set; }
		#endregion
	}
}