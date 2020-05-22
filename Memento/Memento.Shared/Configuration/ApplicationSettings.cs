namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the application settings.
	/// </summary>
	public abstract class ApplicationSettings
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the 'Logging' settings.
		/// </summary>
		public LoggingSettings Logging { get; set; }
		#endregion
	}
}