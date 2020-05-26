namespace Memento.Shared.Services.Emails
{
	/// <summary>
	/// Implements the 'SendGrid' options.
	/// </summary>
	public sealed class SendGridOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the api key.
		/// </summary>
		public string ApiKey { get; set; }

		/// <summary>
		/// Gets or sets the sender.
		/// </summary>
		public SendGridSenderOptions Sender { get; set; }
		#endregion
	}

	/// <summary>
	/// Implements the 'SendGridSender' options.
	/// </summary>
	public sealed class SendGridSenderOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }
		#endregion
	}
}