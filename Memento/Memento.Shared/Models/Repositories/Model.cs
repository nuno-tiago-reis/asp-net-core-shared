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
		public virtual long Id { get; set; }

		/// <inheritdoc />
		public virtual long CreatedBy { get; set; }

		/// <inheritdoc />
		public virtual DateTime CreatedAt { get; set; }

		/// <inheritdoc />
		public virtual long? UpdatedBy { get; set; }

		/// <inheritdoc />
		public virtual DateTime? UpdatedAt { get; set; }
		#endregion
	}
}