using System;
using System.Collections.Generic;
using System.Text;

namespace Memento.Shared.Services.Localization
{
	/// <summary>
	/// Implements the generic interface for a localizer service.
	/// Provides properties and methods to get localized strings using keys.
	/// </summary>
	public sealed class Localizer : ILocalizer
	{
		private readonly IStringLocalizer _localizer;

		public Localizer(IStringLocalizer<SharedResource> localizer)
		{
			_localizer = localizer;
		}

		public string this[string index]
		{
			get
			{
				return _localizer[index];
			}
		}
	}
}