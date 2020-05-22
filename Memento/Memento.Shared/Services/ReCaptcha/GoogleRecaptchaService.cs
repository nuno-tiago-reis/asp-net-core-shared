using Memento.Shared.Configuration;
using Memento.Shared.Exceptions;
using Memento.Shared.Extensions;
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
		/// The recaptcha settings.
		/// </summary>
		private readonly GoogleReCaptchaSettings ReCaptchaSettings;

		/// <summary>
		/// The http client factory.
		/// </summary>
		private readonly IHttpClientFactory HttpClientFactory;

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
		/// <param name="recaptchaSettings">The recaptcha settings.</param>
		/// <param name="httpClientFactory">The http client factory.</param>
		/// <param name="logger">The logger.</param>
		public GoogleRecaptchaService(IOptions<GoogleReCaptchaSettings> recaptchaSettings, IHttpClientFactory httpClientFactory, ILogger<GoogleRecaptchaService> logger)
		{
			this.ReCaptchaSettings = recaptchaSettings.Value;
			this.HttpClientFactory = httpClientFactory;
			this.Logger = logger;
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		public async Task<bool> IsReCaptchaPassedAsync(string recaptchaResponse)
		{
			try
			{
				// Create the client
				var client = this.HttpClientFactory.CreateClient(this.ReCaptchaSettings.HttpClientName);

				// Create the request
				var request = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("secret", this.ReCaptchaSettings.SiteSecret),
					new KeyValuePair<string, string>("response", recaptchaResponse)
				});

				// Send the request and process the response
				using (var response = await client.PostAsync(string.Empty, request))
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
						if (property.Name.EqualsNormalized("success") && property.Value.GetBoolean() == true)
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