using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Memento.Shared.Models.Repository
{
	/// <summary>
	/// Implements the generic interface for a model context.
	/// Provides methods to automatically maintain traceability during create and update operations.
	/// </summary>
	public abstract class ModelContext : DbContext, IModelContext
	{
		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="ModelContext"/> class.
		/// </summary>
		/// 
		/// <param name="options">The options.</param>
		public ModelContext(DbContextOptions options) : base(options)
		{
			// Nothing to do here.
		}
		#endregion

		#region [Methods]
		/// <inheritdoc />
		public override int SaveChanges()
		{
			this.UpdateModelTimestamps();

			return base.SaveChanges();
		}

		/// <inheritdoc />
		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			this.UpdateModelTimestamps();

			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		/// <inheritdoc />
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			this.UpdateModelTimestamps();

			return base.SaveChangesAsync(cancellationToken);
		}

		/// <inheritdoc />
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
		private void UpdateModelTimestamps()
		{
			// Find entries that were created
			var createdEntries = ChangeTracker.Entries().Where(entry => entry.State == EntityState.Added);

			// Update their 'CreatedAt' fields if they implement 'IModel'
			foreach (var createdEntry in createdEntries)
			{
				if (createdEntry.Entity is IModel model)
				{
					model.CreatedAt = DateTime.UtcNow;
				}
			}

			// Find entries that were created
			var modifiedEntries = ChangeTracker.Entries().Where(entry => entry.State == EntityState.Modified);

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