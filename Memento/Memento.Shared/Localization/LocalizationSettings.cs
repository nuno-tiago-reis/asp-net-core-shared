using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Memento.Shared.Localization
{
	/// <summary>
	/// Implements the localization settings.
	/// </summary>
	public static class LocalizationSettings
	{
		#region [Properties]
		/// <summary>
		/// Defines the default culture.
		/// </summary>
		private static readonly RequestCulture DefaultCulture = new RequestCulture("en");

		/// <summary>
		/// Defines the supported cultures.
		/// </summary>
		private static readonly CultureInfo[] SupportedCultures = new[]
		{
			new CultureInfo("en"),
			new CultureInfo("pt")
		};
		#endregion

		#region [Methods]
		/// <summary>
		/// Gets the 'RequestLocalizationOptions' object.
		/// </summary>
		/// 
		/// <seealso cref="RequestLocalizationOptions"/>
		public static RequestLocalizationOptions GetLocalizationOptions()
		{
			return new RequestLocalizationOptions
			{
				DefaultRequestCulture = DefaultCulture,
				SupportedCultures = SupportedCultures,
				SupportedUICultures = SupportedCultures
			};
		}
		#endregion
	}
}