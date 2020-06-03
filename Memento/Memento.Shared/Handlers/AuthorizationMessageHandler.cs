using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Memento.Shared.Handlers
{
	/// <summary>
	/// Implements a custom message handler that sends an access token along with requests
	/// to a list of white-listed urls unless the request is in a list of black-listed urls.
	///
	/// https://github.com/dotnet/AspNetCore.Docs/blob/master/aspnetcore/security/blazor/webassembly/additional-scenarios.md
	/// </summary>
	///
	/// <seealso cref="DelegatingHandler"/>
	public sealed class AuthorizationMessageHandler : DelegatingHandler
	{
		#region [Properties]
		/// <summary>
		/// The black listed uris.
		/// </summary>
		private IEnumerable<Uri> BlackListedUris;

		/// <summary>
		/// The white listed uris.
		/// </summary>
		private IEnumerable<Uri> WhiteListedUris;

		/// <summary>
		/// The scopes.
		/// </summary>
		private IEnumerable<string> Scopes;

		/// <summary>
		/// The cached header.
		/// </summary>
		private AuthenticationHeaderValue CachedHeader;

		/// <summary>
		/// The last access token to be provided.
		/// </summary>
		private AccessToken AccessToken;

		/// <summary>
		/// The access token options.
		/// </summary>
		private AccessTokenRequestOptions AccessTokenOptions;

		/// <summary>
		/// The access token provider.
		/// </summary>
		private readonly IAccessTokenProvider AccessTokenProvider;

		/// <summary>
		/// The navigation manage.
		/// </summary>
		private readonly NavigationManager NavigationManager;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="AuthorizationMessageHandler"/> class.
		/// </summary>
		/// 
		/// <param name="provider">The service provider.</param>
		public AuthorizationMessageHandler(IServiceProvider provider) : base(new HttpClientHandler())
		{
			this.AccessTokenProvider = provider.GetService<IAccessTokenProvider>();
			this.NavigationManager = provider.GetService<NavigationManager>();
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		protected override async Task<HttpResponseMessage> SendAsync
		(
			HttpRequestMessage request,
			CancellationToken cancellationToken
		)
		{
			if (this.BlackListedUris == null)
			{
				throw new InvalidOperationException
				(
					$"The '{nameof(AuthorizationMessageHandler)}' is not configured. " +
					$"Call '{nameof(this.ConfigureHandler)}' and provide a list of endpoint urls to black list."
				);
			}

			if (this.WhiteListedUris == null)
			{
				throw new InvalidOperationException
				(
					$"The '{nameof(AuthorizationMessageHandler)}' is not configured. " +
					$"Call '{nameof(this.ConfigureHandler)}' and provide a list of endpoint urls to white list."
				);
			}

			if (this.BlackListedUris.All(uri => !uri.IsBaseOf(request.RequestUri)) && this.WhiteListedUris.Any(uri => uri.IsBaseOf(request.RequestUri)))
			{
				if (this.AccessToken == null || DateTimeOffset.Now >= this.AccessToken.Expires.AddMinutes(-5))
				{
					var tokenResult = this.AccessTokenOptions != null
						? await this.AccessTokenProvider.RequestAccessToken(this.AccessTokenOptions)
						: await this.AccessTokenProvider.RequestAccessToken();

					if (tokenResult.TryGetToken(out var token))
					{
						this.AccessToken = token;
						this.CachedHeader = new AuthenticationHeaderValue("Bearer", token.Value);
					}
					else
					{
						// Force a redirection to the login provider
						throw new AccessTokenNotAvailableException(this.NavigationManager, tokenResult, this.AccessTokenOptions?.Scopes);
					}
				}

				// We don't try to handle 401s and retry the request with a new token automatically since that would mean we need to copy the request
				// headers and buffer the body and we expect that the user instead handles the 401s. (Also, we can't really handle all 401s as we might
				// not be able to provision a token without user interaction).
				request.Headers.Authorization = this.CachedHeader;
			}

			return await base.SendAsync(request, cancellationToken);
		}

		/// <summary>
		/// Configures this handler to authorize outbound HTTP requests using an access token.
		/// The access token is only attached if only attached if the <see cref="HttpRequestMessage.RequestUri" />
		/// is a base of the <paramref name="whiteListedUrls" /> while not being a base of the <paramref name="blackListedUrls" />.
		/// </summary>
		/// 
		/// <param name="blackListedUrls">The base addresses of endpoint URLs to be black listed.</param>
		/// <param name="whiteListedUrls">The base addresses of endpoint URLs to be white listed.</param>
		/// <param name="scopes">The list of scopes to use when requesting an access token.</param>
		public AuthorizationMessageHandler ConfigureHandler
		(
			IEnumerable<string> blackListedUrls,
			IEnumerable<string> whiteListedUrls,
			IEnumerable<string> scopes = null
		)
		{
			if (this.BlackListedUris != null)
			{
				throw new InvalidOperationException("Handler already configured.");
			}

			if (this.WhiteListedUris != null)
			{
				throw new InvalidOperationException("Handler already configured.");
			}

			this.BlackListedUris = blackListedUrls.Select(uri => new Uri(uri, UriKind.Absolute)).ToArray();
			this.WhiteListedUris = whiteListedUrls.Select(uri => new Uri(uri, UriKind.Absolute)).ToArray();
			this.Scopes = scopes?.ToArray();

			if (this.Scopes != null)
			{
				this.AccessTokenOptions = new AccessTokenRequestOptions
				{
					Scopes = this.Scopes
				};
			}

			return this;
		}
		#endregion
	}
}