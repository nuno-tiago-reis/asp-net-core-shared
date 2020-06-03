using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Memento.Shared.Models.Repositories
{
	/// <summary>
	/// Implements the generic interface for a model context.
	/// Provides methods to automatically maintain traceability during create and update operations.
	/// </summary>
	[UsedImplicitly]
	public abstract class ModelContext : DbContext, IModelContext
	{
		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="ModelContext"/> class.
		/// </summary>
		/// 
		/// <param name="options">The options.</param>
		protected ModelContext(DbContextOptions options) : base(options)
		{
			// Nothing to do here.
		}
		#endregion

		#region [Methods]
		/// <inheritdoc cref="IModelContext"/> />
		[UsedImplicitly]
		public override int SaveChanges()
		{
			this.UpdateModelTimestamps();

			return base.SaveChanges();
		}

		/// <inheritdoc cref="IModelContext"/> />
		[UsedImplicitly]
		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			this.UpdateModelTimestamps();

			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		/// <inheritdoc cref="IModelContext"/> />
		[UsedImplicitly]
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			this.UpdateModelTimestamps();

			return base.SaveChangesAsync(cancellationToken);
		}

		/// <inheritdoc cref="IModelContext"/> />
		[UsedImplicitly]
		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			this.UpdateModelTimestamps();

			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		/// <summary>
		/// Updates the entries in the change tracker that were either created or updated.
		/// - If an entry was created, then the 'CreatedAt' field is automatically populated.
		/// - If an entry was updated, then the 'UpdatedAt' field is automatically populated.
		/// </summary>
		[UsedImplicitly]
		private void UpdateModelTimestamps()
		{
			// Find entries that were created
			var createdEntries = this.ChangeTracker.Entries().Where(entry => entry.State == EntityState.Added);

			// Update their 'CreatedAt' fields if they implement 'IModel'
			foreach (var createdEntry in createdEntries)
			{
				if (createdEntry.Entity is IModel model)
				{
					model.CreatedAt = DateTime.UtcNow;
				}
			}

			// Find entries that were created
			var modifiedEntries = this.ChangeTracker.Entries().Where(entry => entry.State == EntityState.Modified);

			// Update their 'UpdatedAt' fields if they implement 'IModel'
			foreach (var modifiedEntry in modifiedEntries)
			{
				if (modifiedEntry.Entity is IModel model)
				{
					model.UpdatedAt = DateTime.UtcNow;
				}
			}
		}
		#endregion
	}
}