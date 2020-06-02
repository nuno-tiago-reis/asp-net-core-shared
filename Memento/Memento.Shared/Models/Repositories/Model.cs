using JetBrains.Annotations;
using System;

namespace Memento.Shared.Models.Repositories
{
	/// <summary>
	/// Implements the generic interface for a model.
	/// Provides properties to maintain traceability during create and update operations.
	/// </summary>
	[UsedImplicitly]
	public abstract class Model : IModel
	{
		#region [Properties]
		/// <inheritdoc />
		[UsedImplicitly]
		public virtual long Id { get; set; }

		/// <inheritdoc />
		[UsedImplicitly]
		public virtual long CreatedBy { get; set; }

		/// <inheritdoc />
		[UsedImplicitly]
		public virtual DateTime CreatedAt { get; set; }

		/// <inheritdoc />
		[UsedImplicitly]
		public virtual long? UpdatedBy { get; set; }

		/// <inheritdoc />
		[UsedImplicitly]
		public virtual DateTime? UpdatedAt { get; set; }
		#endregion
	}
}