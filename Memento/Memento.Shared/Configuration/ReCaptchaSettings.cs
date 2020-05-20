namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the 'ReCaptcha' settings.
	/// </summary>
	public sealed class ReCaptchaSettings
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the host.
		/// </summary>
		public string Host { get; set; }

		/// <summary>
		/// Gets or sets the secret key.
		/// </summary>
		public string SiteKey { get; set; }

		/// <summary>
		/// Gets or sets the site secret.
		/// </summary>
		public string SiteSecret { get; set; }
		#endregion
	}
}