namespace Memento.Shared.Services.Localization
{
	/// <summary>
	/// Defines a generic interface for a localizer service.
	/// Provides properties and methods to get localized strings using keys.
	/// </summary>
	public interface ILocalizer
	{
		#region [Properties]
		/// <summary>
		/// Returns the localized string with the given key.
		/// </summary>
		/// 
		/// <param name="key">The key</param>
		string this[string key] { get; }
		#endregion

		#region [Properties]
		/// <summary>
		/// Returns the localized string with the given key.
		/// </summary>
		/// 
		/// <param name="key">The key</param>
		string GetString(string key);
		#endregion
	}
}