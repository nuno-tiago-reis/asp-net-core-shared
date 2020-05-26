using System.Threading.Tasks;

namespace Memento.Shared.Services.Emails
{
	/// <summary>
	/// Defines a generic interface for an email service.
	/// Provides methods to send emails.
	/// </summary>
	public interface IEmailService
	{
		#region [Methods]
		/// <summary>
		/// Sends an email with the specified subject and content.
		/// </summary>
		/// 
		/// <param name="email">The email.</param>
		/// <param name="subject">The subject.</param>
		/// <param name="content">The content.</param>
		Task SendEmailAsync(string email, string subject, string content);
		#endregion
	}
}