using JetBrains.Annotations;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Notifications
{
	/// <summary>
	/// Defines a generic interface for an email service.
	/// Provides methods to send emails.
	/// </summary>
	[UsedImplicitly]
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
		[UsedImplicitly]
		Task SendEmailAsync(string email, string subject, string content);
		#endregion
	}
}