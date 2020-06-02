using JetBrains.Annotations;
using Memento.Shared.Models.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Http
{
	/// <summary>
	/// Defines a generic interface for an http service.
	/// Provides methods to interact with the APIs (CRUD).
	/// </summary>
	[UsedImplicitly]
	public interface IHttpService
	{
		#region [Methods]
		/// <summary>
		/// Sends a 'POST' request to the given url.
		/// </summary>
		/// 
		/// <typeparam name="TRequest">The request type.</typeparam>
		/// <typeparam name="TResponse">The request type.</typeparam>
		/// 
		/// <param name="url">The url.</param>
		/// <param name="request">The request.</param>
		[UsedImplicitly]
		Task<MementoResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest request)
			where TRequest : class
			where TResponse : class;

		/// <summary>
		/// Sends a 'PUT' request to the given url.
		/// </summary>
		/// 
		/// <typeparam name="TRequest">The request type.</typeparam>
		/// 
		/// <param name="url">The url.</param>
		/// <param name="request">The request.</param>
		[UsedImplicitly]
		Task<MementoResponse> PutAsync<TRequest>(string url, TRequest request)
			where TRequest : class;

		/// <summary>
		/// Sends a 'PUT' request to the given url.
		/// </summary>
		/// 
		/// <param name="url">The url.</param>
		[UsedImplicitly]
		Task<MementoResponse> DeleteAsync(string url);

		/// <summary>
		/// Sends a 'GET' request to the given url.
		/// </summary>
		/// 
		/// <typeparam name="TResponse">The request type.</typeparam>
		/// 
		/// <param name="url">The url.</param>
		/// <param name="parameters">The parameters.</param>
		[UsedImplicitly]
		Task<MementoResponse<TResponse>> GetAsync<TResponse>(string url, Dictionary<string, string> parameters = null)
			where TResponse : class;
		#endregion
	}
}