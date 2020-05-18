using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;

namespace Memento.Shared.Models.Responses
{
	/// <summary>
	/// Implements the 'MementoResponse' extensions.
	/// Provides ways to signal and identity that a response originated in a Memento API.
	/// </summary>
	[UsedImplicitly]
	public static class MementoResponseExtensions
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
		[UsedImplicitly]
		public static void AddMementoHeader(this HttpResponse response)
		{
			response.Headers.Add(HEADER_NAME, Guid.NewGuid().ToString());
			response.Headers.Add("Access-Control-Expose-Headers", HEADER_NAME);
		}

		/// <summary>
		/// Adds a 'Memento' http header to the http response.
		/// </summary>
		[UsedImplicitly]
		public static bool HasMementoHeader(this HttpResponse response)
		{
			return response.Headers.ContainsKey(HEADER_NAME);
		}

		/// <summary>
		/// Adds a 'Memento' http header to the http response message.
		/// </summary>
		[UsedImplicitly]
		public static void AddMementoHeader(this HttpResponseMessage responseMessage)
		{
			responseMessage.Headers.Add(HEADER_NAME, Guid.NewGuid().ToString());
			responseMessage.Headers.Add("Access-Control-Expose-Headers", HEADER_NAME);
		}

		/// <summary>
		/// Adds a 'Memento' http header to the http response message.
		/// </summary>
		[UsedImplicitly]
		public static bool HasMementoHeader(this HttpResponseMessage responseMessage)
		{
			return responseMessage.Headers.Contains(HEADER_NAME);
		}
		#endregion
	}
}