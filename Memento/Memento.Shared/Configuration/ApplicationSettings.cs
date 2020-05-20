namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the application settings.
	/// </summary>
	public abstract class ApplicationSettings
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the 'DataProtection' settings.
		/// </summary>
		public DataProtectionSettings DataProtection { get; set; }

		/// <summary>
		/// Gets or sets the 'Logging' settings.
		/// </summary>
		public LoggingSettings Logging { get; set; }

		/// <summary>
		/// Gets or sets the 'ReCaptcha' settings.
		/// </summary>
		public ReCaptchaSettings ReCaptcha { get; set; }

		/// <summary>
		/// Gets or sets the 'SendGrid' settings.
		/// </summary>
		public SendGridSettings SendGrid { get; set; }

		/// <summary>
		/// Gets or sets the 'Twilio' settings.
		/// </summary>
		public TwilioSettings Twilio { get; set; }
		#endregion
	}
}