using Memento.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Memento.Shared.Services.TextMessages
{
	/// <summary>
	/// Implements the generic interface for a text message service using Twilio.
	/// Provides methods to send text messages.
	/// </summary>
	public sealed class TwilioService : ITextMessageService
	{
		#region [Properties]
		/// <summary>
		/// The options.
		/// </summary>
		private readonly TwilioOptions Options;

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
		/// Initializes a new instance of the <see cref="TwilioService"/> class.
		/// </summary>
		/// 
		/// <param name="options">The options.</param>
		/// <param name="httpClient">The http client.</param>
		/// <param name="logger">The logger.</param>
		public TwilioService(IOptions<TwilioOptions> options, HttpClient httpClient, ILogger<TwilioService> logger)
		{
			this.Options = options.Value;
			this.HttpClient = httpClient;
			this.Logger = logger;
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		public async Task SendTextMessageAsync(string phoneNumber, string content)
		{
			try
			{
				// Initialize the client
				TwilioClient.Init(this.Options.ApiKey, this.Options.ApiSecret);

				// Send the message
				await MessageResource.CreateAsync
				(
					to: new PhoneNumber(phoneNumber),
					from: new PhoneNumber(this.Options.Sender.PhoneNumber),
					body: content
				);
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