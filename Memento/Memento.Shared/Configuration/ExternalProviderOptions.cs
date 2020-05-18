using JetBrains.Annotations;

namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the 'ExternalProvider' options.
	/// </summary>
	[UsedImplicitly]
	public sealed class ExternalProviderOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the api key.
		/// </summary>
		[UsedImplicitly]
		public string ApiKey { get; set; }

		/// <summary>
		/// Gets or sets the api secret.
		/// </summary>
		[UsedImplicitly]
		public string ApiSecret { get; set; }
		#endregion
	}
}