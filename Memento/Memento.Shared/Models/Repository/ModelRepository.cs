using Memento.Shared.Exceptions;
using Memento.Shared.Models.Pagination;
using Memento.Shared.Services.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Memento.Shared.Models.Repository
{
	/// <summary>
	/// Implements the generic interface for a model repository.
	/// Provides methods to interact with the models (CRUD and more).
	/// </summary>
	/// 
	/// <typeparam name="TModel">The model type.</typeparam>
	/// <typeparam name="TModelFilter">The model filter type.</typeparam>
	/// <typeparam name="TModelFilterOrderBy">The model filter order by type.</typeparam>
	/// <typeparam name="TModelFilterOrderDirection">The model filter order direction type.</typeparam>
	public abstract class ModelRepository<TModel, TModelFilter, TModelFilterOrderBy, TModelFilterOrderDirection> : IModelRepository<TModel, TModelFilter, TModelFilterOrderBy, TModelFilterOrderDirection>
		where TModel : Model
		where TModelFilter : ModelFilter<TModelFilterOrderBy, TModelFilterOrderDirection>, new()
		where TModelFilterOrderBy : Enum
		where TModelFilterOrderDirection : Enum
	{
		#region [Constants]
		/// <summary>
		/// The message that indicates that the model does not exist.
		/// </summary>
		protected const string MODEL_DOES_NOT_EXIST_MESSAGE = "The '{0}' does not exist.";
		#endregion

		#region [Properties]
		/// <summary>
		/// The context.
		/// </summary>
		protected readonly DbContext Context;

		/// <summary>
		/// The models.
		/// </summary>
		protected readonly DbSet<TModel> Models;

		/// <summary>
		/// The localizer.
		/// </summary>
		protected readonly ISharedLocalizer Localizer;

		/// <summary>
		/// The lookup normalizer.
		/// </summary>
		protected readonly ILookupNormalizer LookupNormalizer;

		/// <summary>
		/// The logger.
		/// </summary>
		protected readonly ILogger Logger;
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="ModelRepository{TModel, TModelFilter, TModelFilterOrderBy, TModelFilterOrderDirection}"/> class.
		/// </summary>
		/// 
		/// <param name="context">The context.</param>
		/// <param name="logger">The logger.</param>
		/// <param name="lookupNormalizer">The lookup normalizer.</param>
		/// <param name="stringLocalizer">The string localizer.</param>
		public ModelRepository
		(
			DbContext context,
			ISharedLocalizer localizer,
			ILookupNormalizer lookupNormalizer,
			ILogger<ModelRepository<TModel, TModelFilter, TModelFilterOrderBy, TModelFilterOrderDirection>> logger
		)
		{
			this.Context = context;
			this.Models = context.Set<TModel>();
			this.Localizer = localizer;
			this.LookupNormalizer = lookupNormalizer;
			this.Logger = logger;
		}
		#endregion

		#region [Methods] IModelRepository
		/// <inheritdoc />
		public async virtual Task<TModel> CreateAsync(TModel model)
		{
			// Normalize the model
			this.NormalizeModel(model);
			// Validate the model
			this.ValidateModel(model);

			// Create the model
			await this.Models.AddAsync(model);
			// Save the changes
			await this.Context.SaveChangesAsync();

			// Detach the model before returning it
			this.Context.Entry(model).State = EntityState.Detached;

			return model;
		}

		/// <inheritdoc />
		public async virtual Task<TModel> UpdateAsync(TModel model)
		{
			// Check if the model exists
			var contextModel = await this.Models.FirstOrDefaultAsync(m => m.Id == model.Id);
			if (contextModel == null)
			{
				throw new MementoException(string.Format(MODEL_DOES_NOT_EXIST_MESSAGE, typeof(TModel).Name), MementoExceptionType.NotFound);
			}

			// Normalize the model
			this.NormalizeModel(model);
			// Validate the model
			this.ValidateModel(model);

			// Update the model
			this.UpdateModel(model, contextModel);
			// Save the model changes
			await this.Context.SaveChangesAsync();

			// Detach the model before returning it
			this.Context.Entry(model).State = EntityState.Detached;

			return model;
		}

		/// <inheritdoc />
		public async virtual Task DeleteAsync(long modelId)
		{
			// Check if the model exists
			var contextModel = await this.Models.FirstOrDefaultAsync(m => m.Id == modelId);
			if (contextModel == null)
			{
				throw new MementoException(string.Format(MODEL_DOES_NOT_EXIST_MESSAGE, typeof(TModel).Name), MementoExceptionType.NotFound);
			}

			// Delete the model
			this.Models.Remove(contextModel);
			// Save the changes
			await this.Context.SaveChangesAsync();
		}

		/// <inheritdoc />
		public async virtual Task<TModel> GetAsync(long modelId)
		{
			// Check if the model exists
			var contextModel = await this.GetDetailedQueryable().FirstOrDefaultAsync(m => m.Id == modelId);
			if (contextModel == null)
			{
				throw new MementoException(string.Format(MODEL_DOES_NOT_EXIST_MESSAGE, typeof(TModel).Name), MementoExceptionType.NotFound);
			}

			// Detach the model before returning it
			this.Context.Entry(contextModel).State = EntityState.Detached;

			return contextModel;
		}

		/// <inheritdoc />
		public async virtual Task<IPage<TModel>> GetAllAsync(TModelFilter modelFilter = null)
		{
			// Get the queryables
			var modelQuery = this.GetSimpleQueryable();
			var modelCountQuery = this.GetCountQueryable();

			// Ensure the filter exists
			modelFilter = modelFilter ?? new TModelFilter();

			// Filter the queryables
			this.FilterQueryable(modelQuery, modelFilter);
			this.FilterQueryable(modelCountQuery, modelFilter);

			// Create the model page
			var models = await Page<TModel>.CreateAsync
			(
				// models
				modelQuery,
				modelCountQuery,
				// model pagination
				modelFilter.PageNumber,
				modelFilter.PageSize,
				modelFilter.OrderBy.ToString(),
				modelFilter.OrderDirection.ToString()
			);

			// Detach the models before returning them
			foreach (var model in models)
			{
				this.Context.Entry(model).State = EntityState.Detached;
			}

			return models;
		}

		/// <inheritdoc />
		public async virtual Task<bool> ExistsAsync(long modelId)
		{
			return await this.Models.AnyAsync(m => m.Id == modelId);
		}
		#endregion

		#region [Methods] Utility
		/// <summary>
		/// Normalizes the model.
		/// </summary>
		/// 
		/// <param name="sourceModel">The source model.</param>
		protected abstract void NormalizeModel(TModel sourceModel);

		/// <summary>
		/// Validates the model.
		/// </summary>
		/// 
		/// <param name="sourceModel">The source model.</param>
		protected abstract void ValidateModel(TModel sourceModel);

		/// <summary>
		/// Updates the model.
		/// </summary>
		/// 
		/// <param name="sourceModel">The source model.</param>
		/// <param name="targetModel">The target model.</param>
		protected abstract void UpdateModel(TModel sourceModel, TModel targetModel);

		/// <summary>
		/// Gets a model queryable to be used in count queries.
		/// This queryable should only include the base entity without including any relations.
		/// </summary>
		protected abstract IQueryable<TModel> GetCountQueryable();

		/// <summary>
		/// Gets a model queryable to be used in simple queries.
		/// This queryable should include the base entity and other direct relations.
		/// </summary>
		protected abstract IQueryable<TModel> GetSimpleQueryable();

		/// <summary>
		/// Gets a model queryable to be used in detailed queries.
		/// This queryable should include the base entity and every relation.
		/// </summary>
		protected abstract IQueryable<TModel> GetDetailedQueryable();

		/// <summary>
		/// Filters the model queryable.
		/// </summary>
		/// 
		/// <param name="modelQueryable">The model queryable.</param>
		/// <param name="modelFilter">The model filter.</param>
		protected abstract void FilterQueryable(IQueryable<TModel> modelQueryable, TModelFilter modelFilter);
		#endregion
	}
}