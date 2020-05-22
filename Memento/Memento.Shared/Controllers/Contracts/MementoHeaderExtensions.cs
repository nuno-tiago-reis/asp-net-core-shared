using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;

namespace Memento.Shared.Controllers
{
	/// <summary>
	/// Implements the Memento http header extensions.
	/// Provides a way to identity if the response originated in a Memento API.
	/// </summary>
	public static class MementoHeaderExtensions
	{
		#region [Constants]
		/// <summary>
		/// The headers name.
		/// </summary>
		public const string HEADER_NAME = "Memento";
		#endregion

		#region [Methods]
		/// <summary>
		/// Adds a 'Memento' http header to the http response.
		/// </summary>
		public static void AddMementoHeader(this HttpResponse response)
		{
			response.Headers.Add(HEADER_NAME, Guid.NewGuid().ToString());
			response.Headers.Add("Access-Control-Expose-Headers", HEADER_NAME);
		}

		/// <summary>
		/// Adds a 'Memento' http header to the http response.
		/// </summary>
		public static bool HasMementoHeader(this HttpResponse response)
		{
			return response.Headers.ContainsKey(HEADER_NAME);
		}

		/// <summary>
		/// Adds a 'Memento' http header to the http response message.
		/// </summary>
		public static void AddMementoHeader(this HttpResponseMessage responseMessage)
		{
			responseMessage.Headers.Add(HEADER_NAME, Guid.NewGuid().ToString());
			responseMessage.Headers.Add("Access-Control-Expose-Headers", HEADER_NAME);
		}

		/// <summary>
		/// Adds a 'Memento' http header to the http response message.
		/// </summary>
		public static bool HasMementoHeader(this HttpResponseMessage responseMessage)
		{
			return responseMessage.Headers.Contains(HEADER_NAME);
		}
		#endregion
	}
}