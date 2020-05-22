using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Memento.Shared.Controlers
{
	/// <summary>
	/// Implements an abstract controller that provides logging and mapping properties.
	/// </summary>
	/// 
	/// <seealso cref="ControllerBase" />
	public abstract class BaseApiController : ControllerBase
	{
		#region [Attributes]
		/// <summary>
		/// The logger.
		/// </summary>
		protected readonly ILogger Logger;

		/// <summary>
		/// The mapper.
		/// </summary>
		protected readonly IMapper Mapper;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="BaseApiController"/> class.
		/// </summary>
		/// 
		/// <param name="logger">The logger.</param>
		/// <param name="mapper">The mapper.</param>
		protected BaseApiController(ILogger logger = null, IMapper mapper = null)
		{
			this.Logger = logger;
			this.Mapper = mapper;
		}
		#endregion
	}
}