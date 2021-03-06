﻿using JetBrains.Annotations;
using System.Collections.Generic;

namespace Memento.Shared.Models.Responses
{
	/// <summary>
	/// Implements the generic 'Memento' response.
	/// Provides a shared structure to be used in every request.
	/// </summary>
	[UsedImplicitly]
	public sealed class MementoResponse<T> where T : class
	{
		#region [Properties]
		/// <summary>
		/// Whether the request was a success.
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// The status code.
		/// </summary>
		public int StatusCode { get; set; }

		/// <summary>
		/// The message.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// The data.
		/// </summary>
		public T Data { get; set; }

		/// <summary>
		/// The errors.
		/// </summary>
		public IEnumerable<string> Errors { get; set; }
		#endregion

		#region [Constructor]
		/// <summary>
		/// Initializes a new instance of the <see cref="MementoResponse{T}"/> class.
		/// </summary>
		/// 
		/// <param name="success">The success flag.</param>
		/// <param name="statusCode">The status code.</param>
		/// <param name="message">The message.</param>
		/// <param name="data">The data.</param>
		/// <param name="errors">The errors.</param>
		[UsedImplicitly]
		public MementoResponse(bool success, int statusCode, string message, T data = null, IEnumerable<string> errors = null)
		{
			this.Success = success;
			this.StatusCode = statusCode;
			this.Message = message;
			this.Data = data;
			this.Errors = errors;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MementoResponse{T}"/> class.
		/// </summary>
		[UsedImplicitly]
		public MementoResponse()
		{
			// Nothing to do here.
		}
		#endregion
	}

	/// <summary>
	/// Implements the 'Memento' response.
	/// Provides a shared structure to be used in every request.
	/// </summary>
	[UsedImplicitly]
	public sealed class MementoResponse
	{
		#region [Properties]
		/// <summary>
		/// Whether the request was a success.
		/// </summary>
		public bool Success { get; set; }

		/// <summary>
		/// The status code.
		/// </summary>
		public int StatusCode { get; set; }

		/// <summary>
		/// The message.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// The errors.
		/// </summary>
		public IEnumerable<string> Errors { get; set; }
		#endregion

		#region [Constructor]
		/// <summary>
		/// Initializes a new instance of the <see cref="MementoResponse"/> class.
		/// </summary>
		/// 
		/// <param name="success">The success flag.</param>
		/// <param name="statusCode">The status code.</param>
		/// <param name="message">The message.</param>
		/// <param name="errors">The errors.</param>
		[UsedImplicitly]
		public MementoResponse(bool success, int statusCode, string message, IEnumerable<string> errors = null)
		{
			this.Success = success;
			this.StatusCode = statusCode;
			this.Message = message;
			this.Errors = errors;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MementoResponse"/> class.
		/// </summary>
		[UsedImplicitly]
		public MementoResponse()
		{
			// Nothing to do here.
		}
		#endregion
	}
}