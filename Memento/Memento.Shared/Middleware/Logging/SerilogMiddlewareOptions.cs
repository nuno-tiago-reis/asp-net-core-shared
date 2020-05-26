using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;

namespace Memento.Shared.Middleware.Logging
{
	/// <summary>
	/// Implements the 'SerilogMiddleware' options.
	/// </summary>
	public sealed class SerilogMiddlewareOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the message template.
		/// </summary>
		public string MessageTemplate { get; set; }

		/// <summary>
		/// A callback that can be used to set additional properties on the request completion event.
		/// </summary>
		public Action<IDiagnosticContext, HttpContext> ConfigureContext { get; set; }
		#endregion
	}
}