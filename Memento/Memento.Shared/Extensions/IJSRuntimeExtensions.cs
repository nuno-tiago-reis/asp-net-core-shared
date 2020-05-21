using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Memento.Shared.Extensions
{
	/// <summary>
	/// Implements several 'ISJRuntime' extension methods.
	/// </summary>
	public static class IJSRuntimeExtensions
	{
		#region [Methods] Modal
		/// <summary>
		/// Shows the modal with the given identifier, using the given options.
		/// </summary>
		/// 
		/// <param name="id">The modal identifier.</param>
		/// <param name="backdropOptions">The modal backdrop options.</param>
		/// <param name="keyboardOptions">The modal keyboard options.</param>
		/// <returns></returns>
		public static async ValueTask ShowModalAsync(this IJSRuntime instance, string id, ModalBackdropOptions? backdropOptions = null, ModalKeyboardOptions? keyboardOptions = null)
		{
			object backdrop = null;
			object keyboard = null;

			// Match the backdrop options
			switch (backdropOptions)
			{
				case ModalBackdropOptions.None:
				{
					backdrop = false;
					break;
				}
				case ModalBackdropOptions.Normal:
				{
					backdrop = true;
					break;
				}
				case ModalBackdropOptions.Static:
				{
					backdrop = "static";
					break;
				}
			}

			// Match the keyboard options
			switch (keyboardOptions)
			{
				case ModalKeyboardOptions.None:
				{
					keyboard = false;
					break;
				}
				case ModalKeyboardOptions.Escape:
				{
					keyboard = true;
					break;
				}
			}

			// Show the modal
			await instance.InvokeVoidAsync($"$('#{id}').modal", new { backdrop, keyboard });
		}

		/// <summary>
		/// Hides the modal with the given identifier.
		/// </summary>
		/// 
		/// <param name="id">The modal identifier.</param>
		/// <returns></returns>
		public static async ValueTask HideModalAsync(this IJSRuntime instance, string id)
		{
			// Show the modal
			await instance.InvokeVoidAsync($"$('#{id}').modal", "hide");
		}
		#endregion
	}

	/// <summary>
	/// Defines the modal backdrop options.
	/// </summary>
	public enum ModalBackdropOptions
	{
		/// <summary>
		/// No backdrop is rendered.
		/// </summary>
		None,
		/// <summary>
		/// A backdrop is rendered that closes the modal when clicked on.
		/// </summary>
		Normal,
		/// <summary>
		/// A backdrop is rendered that does not close the modal when clicked on.
		/// </summary>
		Static
	}

	/// <summary>
	/// Defines the modal keyboard options.
	/// </summary>
	public enum ModalKeyboardOptions
	{
		/// <summary>
		/// The keyboard clicks do not close the modal.
		/// </summary>
		None,
		/// <summary>
		/// The keyboards escape key clicks closes the modal.
		/// </summary>
		Escape
	}
}