using Sotsera.Blazor.Toaster.Core.Models;

namespace Memento.Shared.Services.Toaster
{
	/// <summary>
	/// Implements the 'Toaster' options.
	/// </summary>
	public sealed class ToasterOptions : ToasterConfiguration
	{
		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="ToasterOptions"/> class.
		/// </summary>
		public ToasterOptions()
		{
			this.PositionClass = Defaults.Classes.Position.TopRight;
			this.PreventDuplicates = false;
			this.NewestOnTop = false;
			this.MaximumOpacity = 100;
			this.VisibleStateDuration = 5000;
			this.ShowTransitionDuration = 100;
			this.HideTransitionDuration = 100;
		}
		#endregion
	}
}