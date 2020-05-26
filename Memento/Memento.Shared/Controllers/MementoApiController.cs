using AutoMapper;
using Memento.Shared.Services.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Memento.Shared.Controllers
{
	/// <summary>
	/// Implements an abstract controller that provides logging and mapping properties.
	/// </summary>
	/// 
	/// <seealso cref="ControllerBase" />
	public abstract class MementoApiController : ControllerBase
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
		/// The localizer service.
		/// </summary>
		protected readonly ILocalizerService Localizer;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="MementoApiController"/> class.
		/// </summary>
		/// 
		/// <param name="logger">The logger.</param>
		/// <param name="mapper">The mapper.</param>
		/// <param name="localizer">The localizer.</param>
		protected MementoApiController(ILogger logger = null, IMapper mapper = null, ILocalizerService localizer = null)
		{
			this.Logger = logger;
			this.Mapper = mapper;
			this.Localizer = localizer;
		}
		#endregion
	}
}