using System.Collections.Generic;

namespace Memento.Shared.Controllers
{
	/// <summary>
	/// Implements a generic http response.
	/// Provides properties to indicate success.
	/// </summary>
	public sealed class MementoHttpResponse<T> where T : class
	{
		#region [Properties]
		/// <summary>
		/// Whether the request was a success.
		/// </summary>
		public bool Success { get; set; }

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
		/// Initializes a new instance of the <see cref="MementoHttpResponse{T}"/> class.
		/// </summary>
		/// 
		/// <param name="success">The success flag.</param>
		/// <param name="message">The message.</param>
		/// <param name="data">The data.</param>
		/// <param name="errors">The errors.</param>
		public MementoHttpResponse(bool success, string message, T data = null, IEnumerable<string> errors = null)
		{
			this.Success = success;
			this.Message = message;
			this.Data = data;
			this.Errors = errors;
		}
		#endregion
	}

	/// <summary>
	/// Implements a generic http response.
	/// Provides properties to indicate success.
	/// </summary>
	public sealed class MementoHttpResponse
	{
		#region [Properties]
		/// <summary>
		/// Whether the request was a success.
		/// </summary>
		public bool Success { get; set; }

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
		/// Initializes a new instance of the <see cref="MementoHttpResponse"/> class.
		/// </summary>
		/// 
		/// <param name="success">The success flag.</param>
		/// <param name="message">The message.</param>
		/// <param name="errors">The errors.</param>
		public MementoHttpResponse(bool success, string message, IEnumerable<string> errors = null)
		{
			this.Success = success;
			this.Message = message;
			this.Errors = errors;
		}
		#endregion
	}
}