using Memento.Shared.Exceptions;
using Memento.Shared.Extensions;
using Memento.Shared.Models.Responses;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Http
{
	/// <summary>
	/// Implements the generic interface for an http service.
	/// Provides methods to interact with the APIs (CRUD).
	/// </summary>
	public sealed class HttpService : IHttpService
	{
		#region [Constants]
		/// <summary>
		/// The serializer options
		/// </summary>
		private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions();
		#endregion

		#region [Properties]
		/// <summary>
		/// The http client.
		/// </summary>
		private readonly HttpClient HttpClient;

		/// <summary>
		/// The logger.
		/// </summary>
		private readonly ILogger Logger;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="HttpService"/> class.
		/// </summary>
		/// 
		/// <param name="httpClient">The http client.</param>
		/// <param name="logger">The logger.</param>
		public HttpService(HttpClient httpClient, ILogger<HttpService> logger)
		{
			this.HttpClient = httpClient;
			this.Logger = logger;

			SerializerOptions.ConfigureDefaultOptions();
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		public async Task<MementoResponse<TResponse>> PostAsync<TRequest, TResponse>(string url, TRequest request)
			where TRequest : class where TResponse : class
		{
			try
			{
				// Serialize the request
				var requestMessage = Serialize(request);

				// Send the request and process the response
				var responseMessage = await this.HttpClient.PostAsync(url, requestMessage);
				if (responseMessage.HasMementoHeader())
				{
					// Deserialize the response
					var response = await DeserializeAsync<MementoResponse<TResponse>>(responseMessage.Content);

					return response;
				}
				else
				{
					// Deserialize the response
					var response = await DeserializeAsync<TResponse>(responseMessage.Content);

					return new MementoResponse<TResponse>(responseMessage.IsSuccessStatusCode, (int)responseMessage.StatusCode ,responseMessage.ReasonPhrase, response);
				}
			}
			catch (Exception exception)
			{
				// Log the exception
				this.Logger.LogError(exception.Message, exception);

				// Wrap the exception
				throw new MementoException(exception.Message, exception, MementoExceptionType.InternalServerError);
			}
		}

		/// <inheritdoc />
		public async Task<MementoResponse> PutAsync<TRequest>(string url, TRequest request)
			where TRequest : class
		{
			try
			{
				// Serialize the request
				var requestMessage = Serialize(request);

				// Send the request and process the response
				var responseMessage = await this.HttpClient.PutAsync(url, requestMessage);
				if (responseMessage.HasMementoHeader())
				{
					// Deserialize the response
					var response = await DeserializeAsync<MementoResponse>(responseMessage.Content);

					return response;
				}
				else
				{
					return new MementoResponse(responseMessage.IsSuccessStatusCode, (int)responseMessage.StatusCode, responseMessage.ReasonPhrase);
				}
			}
			catch (Exception exception)
			{
				// Log the exception
				this.Logger.LogError(exception.Message, exception);

				// Wrap the exception
				throw new MementoException(exception.Message, exception, MementoExceptionType.InternalServerError);
			}
		}

		/// <inheritdoc />
		public async Task<MementoResponse> DeleteAsync(string url)
		{
			try
			{
				// Send the request and process the response
				var responseMessage = await this.HttpClient.DeleteAsync(url);
				if (responseMessage.HasMementoHeader())
				{
					// Deserialize the response
					var response = await DeserializeAsync<MementoResponse>(responseMessage.Content);

					return response;
				}
				else
				{
					return new MementoResponse(responseMessage.IsSuccessStatusCode, (int)responseMessage.StatusCode, responseMessage.ReasonPhrase);
				}
			}
			catch (Exception exception)
			{
				// Log the exception
				this.Logger.LogError(exception.Message, exception);

				// Wrap the exception
				throw new MementoException(exception.Message, exception, MementoExceptionType.InternalServerError);
			}
		}

		/// <inheritdoc />
		public async Task<MementoResponse<TResponse>> GetAsync<TResponse>(string url, Dictionary<string, string> parameters = null)
			where TResponse : class
		{
			try
			{
				// Serialize the query string parameters
				if (parameters != null && parameters.Count > 0)
				{
					url += $"?{string.Join("&", parameters.Select(parameter => $"{parameter.Key}={parameter.Value}"))}";
				}

				// Send the request and process the response
				var responseMessage = await this.HttpClient.GetAsync(url);
				if (responseMessage.HasMementoHeader())
				{
					// Deserialize the response
					var response = await DeserializeAsync<MementoResponse<TResponse>>(responseMessage.Content);

					return response;
				}
				else
				{
					// Deserialize the response
					var response = await DeserializeAsync<TResponse>(responseMessage.Content);

					return new MementoResponse<TResponse>(responseMessage.IsSuccessStatusCode, (int)responseMessage.StatusCode, responseMessage.ReasonPhrase, response);
				}
			}
			catch (Exception exception)
			{
				// Log the exception
				this.Logger.LogError(exception.Message, exception);

				// Wrap the exception
				throw new MementoException(exception.Message, exception, MementoExceptionType.InternalServerError);
			}
		}
		#endregion

		#region [Methods] Utility
		/// <summary>
		/// Serializes the given object into a string content.
		/// </summary>
		/// 
		/// <typeparam name="T">The request type.</typeparam>
		/// 
		/// <param name="request">The request.</param>
		private static StringContent Serialize<T>(T request)
			where T : class
		{
			var @string = JsonSerializer.Serialize(request);

			var content = new StringContent(@string, Encoding.UTF8, MediaTypeNames.Application.Json);

			return content;
		}

		/// <summary>
		/// Deserializes the given http content into an object.
		/// </summary>
		/// 
		/// <typeparam name="T">The object type.</typeparam>
		/// 
		/// <param name="content">The content.</param>
		private static async Task<T> DeserializeAsync<T>(HttpContent content)
			where T : class
		{
			var @string = await content.ReadAsStringAsync();

			var response = JsonSerializer.Deserialize<T>(@string, SerializerOptions);

			return response;
		}
		#endregion
	}
}