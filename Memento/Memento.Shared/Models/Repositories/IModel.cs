using JetBrains.Annotations;
using System;

namespace Memento.Shared.Models.Repositories
{
	/// <summary>
	/// Defines a generic interface for a model.
	/// Provides properties to maintain traceability during create and update operations.
	/// </summary>
	[UsedImplicitly]
	public interface IModel
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		[UsedImplicitly]
		long Id { get; set; }

		/// <summary>
		/// Gets or sets the created by user identifier.
		/// </summary>
		[UsedImplicitly]
		long CreatedBy { get; set; }

		/// <summary>
		/// Gets or sets the created at timestamp.
		/// </summary>
		[UsedImplicitly]
		DateTime CreatedAt { get; set; }

		/// <summary>
		/// Gets or sets the updated by user identifier.
		/// </summary>
		[UsedImplicitly]
		long? UpdatedBy { get; set; }

		/// <summary>
		/// Gets or sets the updated at timestamp.
		/// </summary>
		[UsedImplicitly]
		DateTime? UpdatedAt { get; set; }
		#endregion
	}
}