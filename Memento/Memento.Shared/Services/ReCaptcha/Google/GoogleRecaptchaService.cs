using Memento.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Memento.Shared.Services.ReCaptcha
{
	/// <summary>
	/// Implements the generic interface for a recaptcha service using the Google ReCaptcha API.
	/// </summary>
	public sealed class GoogleRecaptchaService : IRecaptchaService
	{
		#region [Properties]
		/// <summary>
		/// The options.
		/// </summary>
		private readonly GoogleReCaptchaOptions Options;

		/// <summary>
		/// The http client.
		/// </summary>
		private readonly HttpClient HttpClient;

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly ILogger Logger;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="GoogleRecaptchaService"/> class.
		/// </summary>
		/// 
		/// <param name="options">The options.</param>
		/// <param name="httpClient">The http client.</param>
		/// <param name="logger">The logger.</param>
		public GoogleRecaptchaService(IOptions<GoogleReCaptchaOptions> options, HttpClient httpClient, ILogger<GoogleRecaptchaService> logger)
		{
			this.Options = options.Value;
			this.HttpClient = httpClient;
			this.Logger = logger;
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		public async Task<bool> IsReCaptchaPassedAsync(string recaptchaResponse)
		{
			try
			{
				// Configure the client
				this.HttpClient.BaseAddress = new Uri(this.Options.Host);

				// Create the request
				var request = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("secret", this.Options.SiteSecret),
					new KeyValuePair<string, string>("response", recaptchaResponse)
				});

				// Send the request and process the response
				using (var response = await this.HttpClient.PostAsync(string.Empty, request))
				{
					// An error occurred
					if (response.StatusCode != HttpStatusCode.OK)
					{
						return false;
					}

					// Parse the document
					var document = JsonDocument.Parse(response.Content.ReadAsStringAsync().Result);

					// Iterate the response
					foreach (var property in document.RootElement.EnumerateObject())
					{
						if (property.Name.Equals("success", StringComparison.InvariantCultureIgnoreCase) && property.Value.GetBoolean() == true)
						{
							return true;
						}
					}

					return false;
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
	}
}