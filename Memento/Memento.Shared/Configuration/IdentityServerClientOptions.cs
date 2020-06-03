using System.Collections.Generic;

namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the 'IdentityServerClient' options.
	/// </summary>
	public sealed class IdentityServerClientOptions
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the authority.
		/// </summary>
		public string Authority { get; set; }

		/// <summary>
		/// Gets or sets the client of the application.
		/// </summary>
		public string ClientId { get; set; }

		/// <summary>
		/// Gets or sets the redirect uri.
		/// </summary>
		public string RedirectUri { get; set; }

		/// <summary>
		/// Gets or sets the post logout redirect uri.
		/// </summary>
		public string PostLogoutRedirectUri { get; set; }

		/// <summary>
		/// Gets or sets the black listed uris
		/// </summary>
		public IEnumerable<string> BlackListedUris { get; set; }

		/// <summary>
		/// Gets or sets the white listed uris
		/// </summary>
		public IEnumerable<string> WhiteListedUris { get; set; }

		/// <summary>
		/// Gets or sets the response type to use on the authorization flow.
		/// </summary>
		public string ResponseType { get; set; }

		/// <summary>
		/// Gets or sets the response mode to use in the authorization flow.
		/// </summary>
		public string ResponseMode { get; set; }

		/// <summary>
		/// Gets or sets the scopes.
		/// </summary>
		public IEnumerable<string> Scopes { get; set; }
		#endregion
	}
}