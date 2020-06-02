using JetBrains.Annotations;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Notifications
{
	/// <summary>
	/// Defines the interface for an sms service.
	/// </summary>
	[UsedImplicitly]
	public interface ISmsService
	{
		#region [Methods]
		/// <summary>
		/// Sends a text message with the specified content.
		/// </summary>
		/// 
		/// <param name="phoneNumber">The phone number.</param>
		/// <param name="content">The content.</param>
		[UsedImplicitly]
		Task SendTextMessageAsync(string phoneNumber, string content);
		#endregion
	}
}