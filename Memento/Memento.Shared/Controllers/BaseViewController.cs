using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Memento.Shared.Controllers
{
	/// <summary>
	/// Implements an abstract controller that provides logging and mapping properties.
	/// </summary>
	/// 
	/// <seealso cref="Controller" />
	public abstract class BaseViewController : Controller
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
		/// Initializes a new instance of the <see cref="BaseViewController"/> class.
		/// </summary>
		/// 
		/// <param name="logger">The logger.</param>
		/// <param name="mapper">The mapper.</param>
		protected BaseViewController(ILogger logger = null, IMapper mapper = null)
		{
			this.Logger = logger;
			this.Mapper = mapper;
		}
		#endregion
	}
}