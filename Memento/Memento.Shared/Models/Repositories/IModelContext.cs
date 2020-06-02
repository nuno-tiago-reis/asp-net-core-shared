using JetBrains.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Memento.Shared.Models.Repositories
{
	/// <summary>
	/// Defines a generic interface for a model context.
	/// Provides methods to automatically maintain traceability during create and update operations.
	/// </summary>
	[UsedImplicitly]
	public interface IModelContext
	{
		#region [Methods]
		/// <summary>
		/// Updates the changes made to the context and automatically updates the models timestamps.
		/// </summary>
		[UsedImplicitly]
		int SaveChanges();

		/// <summary>
		/// Updates the changes made to the context and automatically updates the models timestamps.
		/// </summary>
		///
		/// <param name="acceptAllChangesOnSuccess">Whether to accept all changes on success.</param>
		[UsedImplicitly]
		int SaveChanges(bool acceptAllChangesOnSuccess);

		/// <summary>
		/// Updates the changes made to the context and automatically updates the models timestamps.
		/// </summary>
		///
		/// <param name="cancellationToken">The cancellation token.</param>
		[UsedImplicitly]
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

		/// <summary>
		/// Updates the changes made to the context and automatically updates the models timestamps.
		/// </summary>
		///
		/// <param name="acceptAllChangesOnSuccess">Whether to accept all changes on success.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		[UsedImplicitly]
		Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
		#endregion
	}
}