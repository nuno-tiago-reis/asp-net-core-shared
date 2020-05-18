namespace Memento.Shared.Services.ReCaptcha
{
	/// <summary>
	/// Implements the 'GoogleReCaptcha' options.
	/// </summary>
	public sealed class GoogleReCaptchaOptions
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