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
		/// <param name="recaptchaSettings">The recaptcha settings.</param>
		/// <param name="httpClient">The http client.</param>
		/// <param name="logger">The logger.</param>
		public GoogleRecaptchaService(IOptions<GoogleReCaptchaSettings> recaptchaSettings, HttpClient httpClient, ILogger<GoogleRecaptchaService> logger)
		{
			this.ReCaptchaSettings = recaptchaSettings.Value;
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
				// Create the request
				var request = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("secret", this.ReCaptchaSettings.SiteSecret),
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