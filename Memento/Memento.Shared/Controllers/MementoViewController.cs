using AutoMapper;
using Memento.Shared.Services.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sotsera.Blazor.Toaster;

namespace Memento.Shared.Controllers
{
	/// <summary>
	/// Implements an abstract controller that provides logging and mapping properties.
	/// </summary>
	/// 
	/// <seealso cref="Controller" />
	public abstract class MementoViewController : Controller
	{
		#region [Attributes]
		/// <summary>
		/// The logger service.
		/// </summary>
		protected readonly ILogger Logger;

		/// <summary>
		/// The mapper service.
		/// </summary>
		protected readonly IMapper Mapper;

		/// <summary>
		/// The shared localizer service.
		/// </summary>
		protected readonly ILocalizerService SharedLocalizer;

		/// <summary>
		/// The toaster service.
		/// </summary>
		protected readonly IToaster Toaster;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="MementoViewController"/> class.
		/// </summary>
		/// 
		/// <param name="logger">The logger.</param>
		/// <param name="mapper">The mapper.</param>
		/// <param name="sharedLocalizer">The shared localizer.</param>
		/// <param name="toaster">The toaster.</param>
		protected MementoViewController(ILogger logger = null, IMapper mapper = null, ILocalizerService sharedLocalizer = null, IToaster toaster = null)
		{
			this.Logger = logger;
			this.Mapper = mapper;
			this.SharedLocalizer = sharedLocalizer;
			this.Toaster = toaster;
		}
		#endregion
	}
}