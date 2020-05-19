using System;

namespace Memento.Shared.Models
{
	/// <summary>
	/// Defines a generic interface for a model.
	/// Provides properties to maintain traceability during create and update operations.
	/// </summary>
	public interface IModel
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		long Id { get; set; }

		/// <summary>
		/// Gets or sets the created by user identifier.
		/// </summary>
		long CreatedBy { get; set; }

		/// <summary>
		/// Gets or sets the created at timestamp.
		/// </summary>
		DateTime CreatedAt { get; set; }

		/// <summary>
		/// Gets or sets the updated by user identifier.
		/// </summary>
		long? UpdatedBy { get; set; }

		/// <summary>
		/// Gets or sets the updated at timestamp.
		/// </summary>
		DateTime? UpdatedAt { get; set; }
		#endregion
	}
}