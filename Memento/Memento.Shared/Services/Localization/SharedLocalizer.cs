using Microsoft.Extensions.Localization;
using System;

namespace Memento.Shared.Services.Localization
{
	/// <summary>
	/// Implements the generic interface for a shared localizer service.
	/// Provides methods to get localized strings using keys.
	/// </summary>
	///
	/// <typeparam name="TSharedResources">The shared resources type.</typeparam>
	public abstract class SharedLocalizer<TSharedResources> : ISharedLocalizer where TSharedResources : class
	{
		#region [Properties]
		/// <summary>
		/// The string localizer.
		/// </summary>
		private readonly IStringLocalizer StringLocalizer;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="SharedLocalizer"/> class.
		/// </summary>
		/// 
		/// <param name="stringLocalizer">The string localizer.</param>
		public SharedLocalizer(IStringLocalizer<TSharedResources> stringLocalizer)
		{
			this.StringLocalizer = stringLocalizer;
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		public String GetString(string key)
		{
			return this.StringLocalizer[key];
		}

		/// <inheritdoc />
		public String GetString<T>(string key) where T : class
		{
			return this.StringLocalizer[$"{typeof(T).FullName}.{key}"];
		}
		#endregion
	}
}