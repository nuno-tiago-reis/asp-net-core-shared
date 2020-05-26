using Memento.Shared.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Emails
{
	/// <summary>
	/// Implements the generic interface for an email service using SendGrid.
	/// Provides methods to send emails.
	/// </summary>
	public sealed class SendGridService : IEmailService
	{
		#region [Properties]
		/// <summary>
		/// The options.
		/// </summary>
		private readonly SendGridOptions Options;

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
		/// Initializes a new instance of the <see cref="SendGridService"/> class.
		/// </summary>
		/// 
		/// <param name="options">The options.</param>
		/// <param name="httpClient">The http client.</param>
		/// <param name="logger">The logger.</param>
		public SendGridService(IOptions<SendGridOptions> options, HttpClient httpClient, ILogger<SendGridService> logger)
		{
			this.Options = options.Value;
			this.HttpClient = httpClient;
			this.Logger = logger;
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		public async Task SendEmailAsync(string email, string subject, string content)
		{
			try
			{
				// Create the client
				var client = new SendGridClient(this.Options.ApiKey);

				// Create the message
				var message = new SendGridMessage();
				message.AddTo(email);
				message.From = new EmailAddress(this.Options.Sender.Email, this.Options.Sender.Name);
				message.Subject = subject;
				message.HtmlContent = content;

				// Send the message
				await client.SendEmailAsync(message);
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