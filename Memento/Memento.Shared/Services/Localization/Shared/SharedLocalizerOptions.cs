namespace Memento.Shared.Services.Localization
{
	/// <summary>
	/// Implements the 'SharedLocalizer' options.
	/// </summary>
	public sealed class SharedLocalizerOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the default culture.
		/// </summary>
		public string DefaultCulture { get; set; }

		/// <summary>
		/// Gets or sets the supported cultures.
		/// </summary>
		public string[] SupportedCultures { get; set; }
		#endregion

		#region [Constructor]
		/// <summary>
		/// Initializes a new instance of the <see cref="SharedLocalizerOptions"/> class.
		/// </summary>
		public SharedLocalizerOptions()
		{
			// Nothing to do here.
		}
		#endregion
	}
}