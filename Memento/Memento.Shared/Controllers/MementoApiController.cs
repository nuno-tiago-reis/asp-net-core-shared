using AutoMapper;
using Memento.Shared.Models.Pagination;
using Memento.Shared.Models.Repositories;
using Memento.Shared.Models.Responses;
using Memento.Shared.Services.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Memento.Shared.Controllers
{
	/// <summary>
	/// Implements an abstract controller that provides logging and mapping properties.
	/// </summary>
	/// 
	/// <seealso cref="ControllerBase" />
	public abstract class MementoApiController : ControllerBase
	{
		#region [Constants]
		/// <summary>
		/// The key for the message that indicates that the model was created successfully.
		/// </summary>
		protected const string CREATE_SUCCESSFUL = "{0}_CONTROLLER_CREATE_SUCCESSFUL";

		/// <summary>
		/// The key for the message that indicates that the model was updated successfully.
		/// </summary>
		protected const string UPDATE_SUCCESSFUL = "{0}_CONTROLLER_UPDATE_SUCCESSFUL";

		/// <summary>
		/// The key for the message that indicates that the model was deleted successfully.
		/// </summary>
		protected const string DELETE_SUCCESSFUL = "{0}_CONTROLLER_DELETE_SUCCESSFUL";

		/// <summary>
		/// The key for the message that indicates that the model was obtained successfully.
		/// </summary>
		protected const string GET_SUCCESSFUL = "{0}_CONTROLLER_GET_SUCCESSFUL";

		/// <summary>
		/// The key for the message that indicates that the models was obtained successfully.
		/// </summary>
		protected const string GET_ALL_SUCCESSFUL = "{0}_CONTROLLER_GET_ALL_SUCCESSFUL";
		#endregion

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

		#region [Methods] Responses
		/// <summary>
		/// Builds an <seealso cref="ActionResult"/> response for a 'Create'.
		/// </summary>
		/// 
		/// <typeparam name="TModel">The model type.</typeparam>
		/// <typeparam name="TContract">The contract type.</typeparam>
		/// 
		/// <param name="model">The model.</param>
		protected ActionResult<MementoResponse<TContract>> BuildCreateResponse<TModel, TContract>(TModel model)
			where TModel : class, IModel
			where TContract : class
		{
			// Build the message
			var message = this.BuildCreateSuccessfulMessage();

			// Build the contract
			var contract = this.Mapper.Map<TContract>(model);

			// Build the response
			var response = new MementoResponse<TContract>(true, StatusCodes.Status201Created, message, contract);

			// Build the response header
			this.HttpContext.Response.AddMementoHeader();

			return this.Created(new Uri($"{this.Request.GetDisplayUrl()}/{model.Id}"), response);
		}

		/// <summary>
		/// Builds an <seealso cref="ActionResult"/> response for an 'Update'.
		/// </summary>
		protected ActionResult<MementoResponse> BuildUpdateResponse<TModel>()
		{
			// Build the message
			var message = this.BuildUpdateSuccessfulMessage();

			// Build the response
			var response = new MementoResponse(true, StatusCodes.Status200OK, message);

			// Build the response header
			this.HttpContext.Response.AddMementoHeader();

			return this.Ok(response);
		}

		/// <summary>
		/// Builds an <seealso cref="ActionResult"/> response for a 'Delete'.
		/// </summary>
		protected ActionResult<MementoResponse> BuildDeleteResponse<TModel>()
		{
			// Build the message
			var message = this.BuildDeleteSuccessfulMessage();

			// Build the response
			var response = new MementoResponse(true, StatusCodes.Status200OK, message);

			// Build the response header
			this.HttpContext.Response.AddMementoHeader();

			return this.Ok(response);
		}

		/// <summary>
		/// Builds an <seealso cref="ActionResult"/> response for a 'Get'.
		/// </summary>
		/// 
		/// <typeparam name="TModel">The model type.</typeparam>
		/// <typeparam name="TContract">The contract type.</typeparam>
		/// 
		/// <param name="model">The model.</param>
		protected ActionResult<MementoResponse<TContract>> BuildGetResponse<TModel, TContract>(TModel model) 
			where TModel : class, IModel
			where TContract : class
		{
			// Build the message
			var message = this.BuildGetSuccessfulMessage();

			// Build the contract
			var contract = this.Mapper.Map<TContract>(model);

			// Build the response
			var response = new MementoResponse<TContract>(true, StatusCodes.Status200OK, message, contract);

			// Build the response header
			this.HttpContext.Response.AddMementoHeader();

			return this.Ok(response);
		}

		/// <summary>
		/// Builds an <seealso cref="ActionResult"/> response for a 'GetAll'.
		/// </summary>
		/// 
		/// <typeparam name="TModel">The model type.</typeparam>
		/// <typeparam name="TContract">The contract type.</typeparam>
		/// 
		/// <param name="models">The models.</param>
		protected ActionResult<MementoResponse<Page<TContract>>> BuildGetAllResponse<TModel, TContract>(IPage<TModel> models)
			where TModel : class, IModel
			where TContract : class
		{
			// Build the message
			var message = this.BuildGetAllSuccessfulMessage();

			// Build the contracts
			var contracts = this.Mapper.Map<Page<TContract>>(models);

			// Build the response
			var response = new MementoResponse<Page<TContract>>(true, StatusCodes.Status200OK, message, contracts);

			// Build the response header
			this.HttpContext.Response.AddMementoHeader();

			return this.Ok(response);
		}
		#endregion

		#region [Methods] Messages
		/// <summary>
		/// Returns the message that is sent when a 'Create' is successful.
		/// </summary>
		protected abstract string BuildCreateSuccessfulMessage();

		/// <summary>
		/// Returns the message that is sent when an 'Update' is successful.
		/// </summary>
		protected abstract string BuildUpdateSuccessfulMessage();

		/// <summary>
		/// Returns the message that is sent when a 'Delete' is successful.
		/// </summary>
		protected abstract string BuildDeleteSuccessfulMessage();

		/// <summary>
		/// Returns the message that is sent when a 'Get' is successful.
		/// </summary>
		protected abstract string BuildGetSuccessfulMessage();

		/// <summary>
		/// Returns the message that is sent when a 'GetAll' is successful.
		/// </summary>
		protected abstract string BuildGetAllSuccessfulMessage();
		#endregion
	}
}