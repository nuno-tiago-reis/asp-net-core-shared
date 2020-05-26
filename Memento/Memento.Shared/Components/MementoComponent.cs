using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Sotsera.Blazor.Toaster;

namespace Memento.Shared.Components
{
	/// <summary>
	/// Implements an abstract componnent that provides logging and mapping properties.
	/// </summary>
	/// 
	/// <seealso cref="ComponentBase" />
	public abstract class MementoComponent<T> : ComponentBase
	{
		#region [Services]
		/// <summary>
		/// The navigation manager.
		/// </summary>
		[Inject]
		protected NavigationManager NavigationManager { get; set; }

		/// <summary>
		/// The logger service.
		/// </summary>
		[Inject]
		protected ILogger Logger { get; set; }

		/// <summary>
		/// The mapper service
		/// </summary>
		[Inject]
		protected IMapper Mapper { get; set; }

		/// <summary>
		/// The string localizer service.
		/// </summary>
		[Inject]
		protected IStringLocalizer<T> StringLocalizer { get; set; }

		/// <summary>
		/// The toaster service.
		/// </summary>
		[Inject]
		protected IToaster Toaster { get; set; }
		#endregion
	}
}