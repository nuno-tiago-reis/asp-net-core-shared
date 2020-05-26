using System.Threading.Tasks;

namespace Memento.Shared.Services.TextMessages
{
	/// <summary>
	/// Defines the interface for a notifications service.
	/// </summary>
	public interface ITextMessageService
	{
		#region [Methods]
		/// <summary>
		/// Sends a text message with the specified content.
		/// </summary>
		/// 
		/// <param name="phoneNumber">The phone number.</param>
		/// <param name="content">The content.</param>
		Task SendTextMessageAsync(string phoneNumber, string content);
		#endregion
	}
}