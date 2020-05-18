using JetBrains.Annotations;
using System.Threading.Tasks;

namespace Memento.Shared.Services.ReCaptcha
{
	/// <summary>
	/// Defines a generic interface for a google recaptcha service.
	/// Provides methods to check if the recaptcha was successful.
	/// </summary>
	[UsedImplicitly]
	public interface IRecaptchaService
	{
		#region [Methods]
		/// <summary>
		/// Checks if the recaptcha was passed successfully.
		/// </summary>
		/// 
		/// <param name="recaptchaResponse"></param>
		[UsedImplicitly]
		Task<bool> IsReCaptchaPassedAsync(string recaptchaResponse);
		#endregion
	}
}