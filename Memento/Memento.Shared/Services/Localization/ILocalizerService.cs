using JetBrains.Annotations;
using System;

namespace Memento.Shared.Services.Localization
{
	/// <summary>
	/// Defines a generic interface for a localizer service.
	/// Provides methods to get localized strings using keys.
	/// </summary>
	[UsedImplicitly]
	public interface ILocalizerService
	{
		#region [Methods]
		/// <summary>
		/// Returns the localized string with the given key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="arguments">The arguments.</param>
		[UsedImplicitly]
		string GetString(string key, params object[] arguments);

		/// <summary>
		/// Returns the localized string with the given key (assumes the context of <seealso cref="T"/>).
		/// </summary>
		/// 
		/// <param name="key">The key</param>
		/// <param name="arguments">The arguments.</param>
		[UsedImplicitly]
		string GetString<T>(string key, params object[] arguments) where T : class;

		/// <summary>
		/// Returns the localized string with the given key (assumes the context of <seealso cref="Type"/>).
		/// </summary>
		/// 
		/// <param name="type">The type</param>
		/// <param name="key">The key</param>
		/// <param name="arguments">The arguments.</param>
		[UsedImplicitly]
		string GetString(Type type, string key, params object[] arguments);
		#endregion
	}
}