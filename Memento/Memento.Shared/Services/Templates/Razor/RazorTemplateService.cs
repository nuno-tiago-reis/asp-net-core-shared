using Memento.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Templates
{
	/// <summary>
	/// Implements the generic interface for a template service using the Razor Template Engine.
	/// </summary>
	public sealed class RazorTemplateService : ITemplateService
	{
		#region [Properties]
		/// <summary>
		/// The view engine.
		/// </summary>
		private readonly IRazorViewEngine ViewEngine;

		/// <summary>
		/// The service provider.
		/// </summary>
		private readonly IServiceProvider ServiceProvider;

		/// <summary>
		/// The temp data provider.
		/// </summary>
		private readonly ITempDataProvider TempDataProvider;

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly ILogger Logger;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="RazorTemplateService"/> class.
		/// </summary>
		///
		/// <param name="viewEngine">The environment.</param>
		/// <param name="serviceProvider">The service provider.</param>
		/// <param name="tempDataProvider">The temp data provider.</param>
		/// <param name="logger">The logger.</param>
		public RazorTemplateService
		(
			IRazorViewEngine viewEngine,
			IServiceProvider serviceProvider,
			ITempDataProvider tempDataProvider,
			ILogger<RazorTemplateService> logger
		)
		{
			this.ViewEngine = viewEngine;
			this.ServiceProvider = serviceProvider;
			this.TempDataProvider = tempDataProvider;
			this.Logger = logger;
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		public async Task<string> RenderAsync(string name)
		{
			// Render the template with no model
			return await RenderAsync<object>(name, null);
		}

		/// <inheritdoc />
		public async Task<string> RenderAsync<TModel>(string name, TModel model)
		{
			try
			{
				// Create the action context
				var actionContext = this.GetActionContext();

				// Find the view
				var viewEngineResult = this.ViewEngine.FindView(actionContext, name, false);
				if (!viewEngineResult.Success)
				{
					throw new ArgumentException($"The {nameof(name)} parameter is invalid (couldn't find the view).");
				}

				using (var output = new StringWriter())
				{
					// Create the necessary parameters
					var view = viewEngineResult.View;
					var tempData = new TempDataDictionary(actionContext.HttpContext, this.TempDataProvider);
					var viewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
					{
						Model = model
					};
					var htmlHelperOptions = new HtmlHelperOptions();

					// Create the view context
					var viewContext = new ViewContext
					(
						view: view,
						viewData: viewData,
						tempData: tempData,
						actionContext: actionContext,
						htmlHelperOptions: htmlHelperOptions,
						writer: output
					);

					// Render the view
					await view.RenderAsync(viewContext);

					return output.ToString();
				}
			}
			catch (Exception exception)
			{
				// Log the exception
				this.Logger.LogError(exception.Message, exception);

				// Wrap the exception
				throw new MementoException(exception.Message, exception, MementoExceptionType.InternalServerError);
			}
		}
		#endregion

		#region [Methods] Utility
		/// <summary>
		/// Generates an action context.
		/// </summary>
		private ActionContext GetActionContext()
		{
			var httpContext = new DefaultHttpContext { RequestServices = ServiceProvider };
			var routeData = new RouteData();
			var actionDescriptor = new ActionDescriptor();

			return new ActionContext(httpContext, routeData, actionDescriptor);
		}
		#endregion
	}
}