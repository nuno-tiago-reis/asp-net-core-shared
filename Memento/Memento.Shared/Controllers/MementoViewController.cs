using AutoMapper;
using JetBrains.Annotations;
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
	[UsedImplicitly]
	public abstract class MementoViewController : Controller
	{
		#region [Attributes]
		/// <summary>
		/// The logger service.
		/// </summary>
		[UsedImplicitly]
		protected readonly ILogger Logger;

		/// <summary>
		/// The mapper service.
		/// </summary>
		[UsedImplicitly]
		protected readonly IMapper Mapper;

		/// <summary>
		/// The localizer service.
		/// </summary>
		[UsedImplicitly]
		protected readonly ILocalizerService Localizer;

		/// <summary>
		/// The toaster service.
		/// </summary>
		[UsedImplicitly]
		protected readonly IToaster Toaster;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="MementoViewController"/> class.
		/// </summary>
		/// 
		/// <param name="logger">The logger.</param>
		/// <param name="mapper">The mapper.</param>
		/// <param name="localizer">The localizer.</param>
		/// <param name="toaster">The toaster.</param>
		protected MementoViewController(ILogger logger = null, IMapper mapper = null, ILocalizerService localizer = null, IToaster toaster = null)
		{
			this.Logger = logger;
			this.Mapper = mapper;
			this.Localizer = localizer;
			this.Toaster = toaster;
		}
		#endregion
	}
}