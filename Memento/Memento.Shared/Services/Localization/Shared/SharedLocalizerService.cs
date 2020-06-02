using Microsoft.Extensions.Localization;
using System;

namespace Memento.Shared.Services.Localization.Shared
{
	/// <summary>
	/// Implements the generic interface for a shared localizer service.
	/// Provides methods to get localized strings using keys.
	/// </summary>
	///
	/// <typeparam name="TSharedResources">The shared resources type.</typeparam>
	public sealed class SharedLocalizerService<TSharedResources> : ILocalizerService where TSharedResources : class
	{
		#region [Properties]
		/// <summary>
		/// The string localizer.
		/// </summary>
		private readonly IStringLocalizer StringLocalizer;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="SharedLocalizerService{TSharedResources}"/> class.
		/// </summary>
		/// 
		/// <param name="stringLocalizer">The string localizer.</param>
		public SharedLocalizerService(IStringLocalizer<TSharedResources> stringLocalizer)
		{
			this.StringLocalizer = stringLocalizer;
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		public string GetString(string key, params object[] arguments)
		{
			if (arguments != null && arguments.Length > 0)
			{
				return this.StringLocalizer[string.Format(key, arguments)];
			}
			else
			{
				return this.StringLocalizer[key];
			}
		}

		/// <inheritdoc />
		public string GetString<T>(string key, params object[] arguments) where T : class
		{
			string format = $"{typeof(T).FullName}.{key}";

			if (arguments != null && arguments.Length > 0)
			{
				return this.StringLocalizer[string.Format(format, arguments)];
			}
			else
			{
				return this.StringLocalizer[format];
			}
		}

		/// <inheritdoc />
		public string GetString(Type type, string key, params object[] arguments)
		{
			string format = $"{type.FullName}.{key}";

			if (arguments != null && arguments.Length > 0)
			{
				return this.StringLocalizer[string.Format(format, arguments)];
			}
			else
			{
				return this.StringLocalizer[format];
			}
		}
		#endregion
	}
}