using System;
using System.Collections.Generic;
using System.Linq;

namespace Memento.Shared.Exceptions
{
	/// <summary>
	/// Implements a custom exception that allows the correlation of exceptions with specific http status codes.
	/// </summary>
	/// 
	/// <seealso cref="Exception" />
	public sealed class MementoException : Exception
	{
		#region [Properties]
		/// <summary>
		/// The message.
		/// </summary>
		public override string Message
		{
			get { return this.Messages.Aggregate((i, j) => i + Environment.NewLine + j); }
		}

		/// <value>
		/// The messages.
		/// </value>
		public string[] Messages { get; }

		/// <summary>
		/// The type.
		/// </summary>
		public MementoExceptionType Type { get; }
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="MementoException"/> class.
		/// </summary>
		/// 
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="type">The corresponding status code enum value.</param>
		public MementoException(string message, MementoExceptionType type)
			: this(new[] { message }, null, type)
		{
			// Nothing to do here.
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MementoException"/> class.
		/// </summary>
		/// 
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="exception">The inner exception that originated this exception.</param>
		/// <param name="type">The corresponding status code enum value.</param>
		public MementoException(string message, Exception exception, MementoExceptionType type)
			: this(new[] { message }, exception, type)
		{
			this.Type = type;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MementoException"/> class.
		/// </summary>
		/// 
		/// <param name="messages">The error messages that explain the reasons for the exception.</param>
		/// <param name="type">The corresponding status code enum value.</param>
		public MementoException(IEnumerable<string> messages, MementoExceptionType type)
			: this(messages, null, type)
		{
			// Nothing to do here.
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MementoException"/> class.
		/// </summary>
		/// 
		/// <param name="messages">The error messages that explain the reasons for the exception.</param>
		/// <param name="exception">The inner exception that originated this exception.</param>
		/// <param name="type">The corresponding status code enum value.</param>
		public MementoException(IEnumerable<string> messages, Exception exception, MementoExceptionType type)
			: base(null, exception)
		{
			this.Messages = messages.ToArray();
			this.Type = type;
		}
		#endregion
	}

	/// <summary>
	/// Defines the available exception types.
	/// These exception types are then read by an exception handler
	/// in order to return the corresponding http status code instead of a generic 500 error.
	/// </summary>
	public enum MementoExceptionType
	{
		/// <summary>
		/// Maps to the 400 status code.
		/// </summary>
		BadRequest,
		/// <summary>
		/// Maps to the 401 status code.
		/// </summary>
		Unauthorized,
		/// <summary>
		/// Maps to the 403 status code.
		/// </summary>
		Forbidden,
		/// <summary>
		/// Maps to the 404 status code.
		/// </summary>
		NotFound,
		/// <summary>
		/// Maps to the 500 status code.
		/// </summary>
		InternalServerError
	}
}