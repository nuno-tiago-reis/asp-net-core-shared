using JetBrains.Annotations;

namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the 'IdentityServerResource' options.
	/// </summary>
	[UsedImplicitly]
	public sealed class IdentityServerResourceOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the authority.
		/// </summary>
		[UsedImplicitly]
		public string Authority { get; set; }

		/// <summary>
		/// Gets or sets the audience.
		/// </summary>
		[UsedImplicitly]
		public string Audience { get; set; }
		#endregion
	}
}