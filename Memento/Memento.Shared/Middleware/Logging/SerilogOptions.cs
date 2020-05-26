using System.Collections.Generic;

namespace Memento.Shared.Middleware.Logging
{
	/// <summary>
	/// Implements the 'Serilog' options.
	/// </summary>
	public sealed class SerilogOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the log level.
		/// </summary>
		public string Level { get; set; }

		/// <summary>
		/// Gets or sets the payload options.
		/// </summary>
		public HttpPayloadOptionsEnum PayloadOptions { get; set; }

		/// <summary>
		/// Gets or sets the overriden payload options.
		/// </summary>
		public IEnumerable<SerilogHttpPayloadOptions> OverridenPayloadOptions { get; set; }
		#endregion
	}

	/// <summary>
	/// Defines the 'HttpPayload' options.
	/// </summary>
	public sealed class SerilogHttpPayloadOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the path.
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// Gets or sets the options.
		/// </summary>
		public HttpPayloadOptionsEnum Options { get; set; }
		#endregion
	}

	/// <summary>
	/// Defines the log payload options.
	/// </summary>
	public enum HttpPayloadOptionsEnum
	{
		/// <summary>
		/// Always include payloads.
		/// </summary>
		IncludeAlways = 0,
		/// <summary>
		/// Always include payloads if there is an error.
		/// </summary>
		IncludeOnError = 1,
		/// <summary>
		/// Always exclude payloads.
		/// </summary>
		ExcludeAlways = 2,
		/// <summary>
		/// Always exclude payloads if there is an error.
		/// </summary>
		ExcludeOnError = 3
	}
}