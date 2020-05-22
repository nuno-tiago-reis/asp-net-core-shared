using Memento.Shared.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Http
{
	/// <summary>
	/// Defines a generic interface for a storage services.
	/// Provides methods to interact with the storage (CRUD and more).
	/// </summary>
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
		Task<MementoHttpResponse<TResponse>> Post<TRequest, TResponse>(string url, TRequest request)
			where TRequest : class
			where TResponse : class;

		/// <summary>
		/// Sends a 'GET' request to the given url.
		/// </summary>
		/// 
		/// <typeparam name="TResponse">The request type.</typeparam>
		/// 
		/// <param name="url">The url.</param>
		/// <param name="parameters">The parameters.</param>
		Task<MementoHttpResponse<TResponse>> Get<TResponse>(string url, Dictionary<string, string> parameters)
			where TResponse : class;

		/// <summary>
		/// Sends a 'PUT' request to the given url.
		/// </summary>
		/// 
		/// <typeparam name="TRequest">The request type.</typeparam>
		/// 
		/// <param name="url">The url.</param>
		/// <param name="request">The request.</param>
		Task<MementoHttpResponse> Put<TRequest>(string url, TRequest request)
			where TRequest : class;

		/// <summary>
		/// Sends a 'PUT' request to the given url.
		/// </summary>
		/// 
		/// <param name="url">The url.</param>
		Task<MementoHttpResponse> Delete(string url);
		#endregion
	}
}