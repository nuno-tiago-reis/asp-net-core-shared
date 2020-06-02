namespace Memento.Shared.Services.Notifications.Twilio
{
	/// <summary>
	/// Implements the 'Twilio' options.
	/// </summary>
	public sealed class TwilioOptions
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
		/// Gets or sets the sender.
		/// </summary>
		public TwilioSenderOptions Sender { get; set; }
		#endregion
	}

	/// <summary>
	/// Implements the ' TwilioSender' options.
	/// </summary>
	public sealed class TwilioSenderOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		public string PhoneNumber { get; set; }
		#endregion
	}
}