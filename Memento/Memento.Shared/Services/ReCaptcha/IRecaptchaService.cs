using System.Threading.Tasks;

namespace Memento.Shared.Services.ReCaptcha
{
	/// <summary>
	/// Defines a generic interface for a google recaptcha service.
	/// Provides methods to check if the recaptcha was successful.
	/// </summary>
	public interface IRecaptchaService
	{
		#region [Methods]
		/// <summary>
		/// Checks if the recaptcha was passed successfully.
		/// </summary>
		/// 
		/// <param name="recaptchaResponse"></param>
		Task<bool> IsReCaptchaPassedAsync(string recaptchaResponse);
		#endregion
	}
}