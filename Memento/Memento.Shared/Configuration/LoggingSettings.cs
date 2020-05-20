using System.Collections.Generic;

namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the 'Logging' settings.
	/// </summary>
	public sealed class LoggingSettings
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the log level.
		/// </summary>
		public string Level { get; set; }

		/// <summary>
		/// Gets or sets a value indicating when to log http payloads globally.
		/// </summary>
		public HttpPayloadOptions HttpPayloads { get; set; }

		/// <summary>
		/// Gets or sets the http payloads with individual configurations.
		/// </summary>
		public IEnumerable<HttpPayloadSettings> HttpPayloadSettings { get; set; }

		/// <summary>
		/// Gets or sets a value indicating when to log http client payloads globally.
		/// </summary>
		public HttpPayloadOptions HttpClientPayloads { get; set; }

		/// <summary>
		/// Gets or sets the http client payloads with individual configurations.
		/// </summary>
		public IEnumerable<HttpPayloadSettings> HttpClientPayloadSettings { get; set; }
		#endregion
	}

	/// <summary>
	/// Defines the 'HttpPayload' settings.
	/// </summary>
	public sealed class HttpPayloadSettings
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the options.
		/// </summary>
		public HttpPayloadOptions Options { get; set; }
		#endregion
	}

	/// <summary>
	/// Defines the log payload options.
	/// </summary>
	public enum HttpPayloadOptions
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