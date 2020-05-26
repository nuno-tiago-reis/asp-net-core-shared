using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Serilog;
using Serilog.Extensions.Hosting;
using Serilog.Events;
using Serilog.Parsing;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memento.Shared.Middleware.Logging
{
	/// <summary>
	/// Implements the http context logging middleware.
	/// This middleware takes the current http request and response and logs their bodies.
	/// </summary>
	public sealed class SerilogMiddleware
	{
		#region [Properties]
		/// <summary>
		/// The next request delegate.
		/// </summary>
		private readonly RequestDelegate Next;

		/// <summary>
		/// The logging context.
		/// </summary>
		private readonly DiagnosticContext LoggingContext;

		/// <summary>
		/// The logging options.
		/// </summary>
		private readonly SerilogMiddlewareOptions LoggingOptions;

		/// <summary>
		/// The logging messages template.
		/// </summary>
		private readonly MessageTemplate MessageTemplate;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="SerilogMiddleware"/> class.
		/// </summary>
		/// 
		/// <param name="next">The next delegate.</param>
		/// <param name="diagnosticContext">The diagnostic context.</param>
		/// <param name="requestLoggingOptions">The request logging options.</param>
		public SerilogMiddleware
		(
			RequestDelegate next,
			DiagnosticContext diagnosticContext,
			SerilogMiddlewareOptions requestLoggingOptions
		)
		{
			this.Next = next ?? throw new ArgumentNullException(nameof(next));
			this.LoggingContext = diagnosticContext ?? throw new ArgumentNullException(nameof(diagnosticContext));
			this.LoggingOptions = requestLoggingOptions ?? throw new ArgumentNullException(nameof(requestLoggingOptions));
			this.MessageTemplate = new MessageTemplateParser().Parse(this.LoggingOptions.MessageTemplate);
		}
		#endregion

		#region [Methods]
		/// <summary>
		/// Takes the http request and response from the given context and logs their bodies.
		/// </summary>
		/// 
		/// <param name="context">The context.</param>
		public async Task Invoke(HttpContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			// Start the timer
			long startTime = Stopwatch.GetTimestamp();

			// Begin collecting properties
			var collector = this.LoggingContext.BeginCollection();

			try
			{
				// Add the request method to the log event collector
				collector.AddOrUpdate(new LogEventProperty("RequestMethod", GetRequestMethod(context)));
				// Add the request path to the log event collector
				collector.AddOrUpdate(new LogEventProperty("RequestPath", GetRequestPath(context)));

				// Check if payloads should be included by default
				bool includePayloads = true;

				if (includePayloads)
				{
					// Extract the properties
					var requestBody = await GetRequestBody(context);
					var requestContentType = new ScalarValue(context.Request.ContentType);
					var responseBody = await GetResponseBody(context);
					var responseContentType = new ScalarValue(context.Response.ContentType);

					// Add the properties to the log event collector
					collector.AddOrUpdate(new LogEventProperty("RequestBody", requestBody));
					collector.AddOrUpdate(new LogEventProperty("RequestType", requestContentType));
					collector.AddOrUpdate(new LogEventProperty("ResponseBody", responseBody));
					collector.AddOrUpdate(new LogEventProperty("ResponseType", responseContentType));
				}
				else
				{
					// Invoke the next handler
					await this.Next.Invoke(context).ConfigureAwait(false);

					// Extract the properties
					var requestContentType = new ScalarValue(context.Request.ContentType);
					var responseContentType = new ScalarValue(context.Response.ContentType);

					// Add the properties to the log event collector
					collector.AddOrUpdate(new LogEventProperty("RequestType", requestContentType));
					collector.AddOrUpdate(new LogEventProperty("ResponseType", responseContentType));
				}

				// Log the event completion	
				LogRequest(context, collector, startTime);
			}
			// Never caught, because `LogCompletion()` returns false.
			// This ensures e.g. the developer exception page is still shown although it
			// does also mean we see a duplicate "unhandled exception" event from ASP.NET Core.
			catch (Exception exception) when (LogRequest(context, collector, startTime, exception))
			{
				// Nothing to do here.
			}
			finally
			{
				collector.Dispose();
			}
		}

		/// <summary>
		/// Logs the request.
		/// </summary>
		/// 
		/// <param name="context">The context.</param>
		/// <param name="collector">The diagnostics context collector.</param>
		/// <param name="startTime">The start time.</param>
		/// <param name="exception">The exception that occured during the requests execution.</param>
		private bool LogRequest(HttpContext context, DiagnosticContextCollector collector, long startTime, Exception exception = null)
		{
			var logger = Log.ForContext<SerilogMiddleware>();

			// Check if the current level matches
			var level = GetLogLevel(context, exception);
			if (!logger.IsEnabled(level))
				return false;

			// Check the requests status code
			var statusCode = GetRequestStatusCode(context, exception);
			// Check the requests elapsed time
			var elapsedTime = GetRequestElapsedTime(startTime);

			// Enrich the diagnostic context with custom properties
			this.LoggingOptions.ConfigureContext?.Invoke(this.LoggingContext, context);

			// Use empty properties if there's an error
			if (!collector.TryComplete(out var collectedProperties))
				collectedProperties = new LogEventProperty[0];

			// Last-in (correctly) wins...
			var properties = collectedProperties.Concat(new[]
			{
				new LogEventProperty("StatusCode", statusCode),
				new LogEventProperty("ElapsedTime", elapsedTime)
			});

			// Create the log
			var @event = new LogEvent(DateTimeOffset.Now, level, exception, this.MessageTemplate, properties);
			// Write the log
			logger.Write(@event);

			return false;
		}
		#endregion

		#region [Methods] Level
		/// <summary>
		/// Gets the log level according to the context and the exception.
		/// </summary>
		/// 
		/// <param name="context">The context.</param>
		/// <param name="exception">The exception.</param>
		private static LogEventLevel GetLogLevel(HttpContext context, Exception exception)
		{
			if (exception != null || context.Response.StatusCode > 499)
				return LogEventLevel.Error;

			return LogEventLevel.Information;
		}
		#endregion

		#region [Methods] Properties
		/// <summary>
		/// Gets the request method.
		/// </summary>
		/// 
		/// <param name="context">The http context.</param>
		private static ScalarValue GetRequestMethod(HttpContext context)
		{
			return new ScalarValue(context.Request.Method);
		}

		/// <summary>
		/// Gets the request path.
		/// </summary>
		/// 
		/// <param name="context">The http context.</param>
		private static ScalarValue GetRequestPath(HttpContext context)
		{
			return new ScalarValue(context.Features.Get<IHttpRequestFeature>()?.RawTarget ?? context.Request.Path.ToString());
		}

		/// <summary>
		/// Gets the elapsed milliseconds from start to now.
		/// </summary>
		/// 
		/// <param name="start">The start timestamp (milliseconds).</param>
		private static ScalarValue GetRequestElapsedTime(long start)
		{
			long stop = Stopwatch.GetTimestamp();

			return new ScalarValue((stop - start) * 1000 / (double)Stopwatch.Frequency);
		}

		/// <summary>
		/// Gets the status code according to the given exception.
		/// </summary>
		/// 
		/// <param name="context">The context.</param>
		/// <param name="exception">The exception.</param>
		private static ScalarValue GetRequestStatusCode(HttpContext context, Exception exception = null)
		{
			return exception == null
				? new ScalarValue(context.Response.StatusCode)
				: new ScalarValue(500);
		}
		#endregion

		#region [Methods] Payloads
		/// <summary>
		/// Gets the http request body as a string.
		/// </summary>
		/// 
		/// <param name="context">The http context.</param>
		private async Task<LogEventPropertyValue> GetRequestBody(HttpContext context)
		{
			if (!context.Request.ContentLength.HasValue)
				return new ScalarValue(null);

			if (context.Request.ContentLength > 0)
				return new ScalarValue(null);

			// Enable rewind so we can go back to the origin
			context.Request.EnableBuffering();

			// Open a stream to read the body
			var reader = new StreamReader(context.Request.Body, Encoding.UTF8);

			// Reset the stream, read it and then reset it again
			context.Request.Body.Seek(0, SeekOrigin.Begin);
			string body = await reader.ReadToEndAsync();
			context.Request.Body.Seek(0, SeekOrigin.Begin);

			return new ScalarValue(body);
		}

		/// <summary>
		/// Gets the http response body as a string.
		/// </summary>
		/// 
		/// <param name="context">The http context</param>
		private async Task<LogEventPropertyValue> GetResponseBody(HttpContext context)
		{
			// Store the original body's stream
			var originalBodyStream = context.Response.Body;

			try
			{
				// Extract the response body
				var memoryBodyStream = new MemoryStream();

				// Replace the original body's stream
				context.Response.Body = memoryBodyStream;

				// Invoke the next handler
				await this.Next.Invoke(context).ConfigureAwait(false);

				// Reset the stream, read it and then reset it again
				memoryBodyStream.Seek(0, SeekOrigin.Begin);
				string body = await new StreamReader(memoryBodyStream).ReadToEndAsync();
				memoryBodyStream.Seek(0, SeekOrigin.Begin);

				// Copy into the original body's stream
				await memoryBodyStream.CopyToAsync(originalBodyStream);

				return new ScalarValue(body);
			}
			finally
			{
				// Restore the original body's stream
				context.Response.Body = originalBodyStream;
			}
		}
		#endregion
	}
}