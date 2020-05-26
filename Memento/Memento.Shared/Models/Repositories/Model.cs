using System;

namespace Memento.Shared.Models.Repositories
{
	/// <summary>
	/// Implements the generic interface for a model.
	/// Provides properties to maintain traceability during create and update operations.
	/// </summary>
	public abstract class Model : IModel
	{
		#region [Properties]
		/// <inheritdoc />
		public long Id { get; set; }

		/// <inheritdoc />
		public long CreatedBy { get; set; }

		/// <inheritdoc />
		public DateTime CreatedAt { get; set; }

		/// <inheritdoc />
		public long? UpdatedBy { get; set; }

		/// <inheritdoc />
		public DateTime? UpdatedAt { get; set; }
		#endregion
	}
}