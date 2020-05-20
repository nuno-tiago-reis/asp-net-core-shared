namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the 'Twilio' settings.
	/// </summary>
	public sealed class TwilioSettings
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

		/// <summary>
		/// Gets or sets the from number.
		/// </summary>
		public string FromNumber { get; set; }
		#endregion
	}
}