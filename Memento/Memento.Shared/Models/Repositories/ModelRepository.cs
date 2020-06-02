using Memento.Shared.Exceptions;
using Memento.Shared.Extensions;
using Memento.Shared.Models.Pagination;
using Memento.Shared.Services.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Memento.Shared.Models.Repositories
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
		where TModel : class, IModel, new()
		where TModelFilter : class, IModelFilter<TModelFilterOrderBy, TModelFilterOrderDirection>, new()
		where TModelFilterOrderBy : Enum
		where TModelFilterOrderDirection : Enum
	{
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
		protected readonly ILocalizerService Localizer;

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
			ILocalizerService localizer,
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
				throw new MementoException(this.GetModelDoesNotMessage(), MementoExceptionType.NotFound);
			}

			// Normalize the model
			this.NormalizeModel(model);
			// Validate the model
			this.ValidateModel(model);

			// Update the model
			this.UpdateModel(model, contextModel);
			// Save the changes
			await this.Context.SaveChangesAsync();

			// Update the models relations
			this.UpdateModelRelations(model, contextModel);
			// Save the changes
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
				throw new MementoException(this.GetModelDoesNotMessage(), MementoExceptionType.NotFound);
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
				throw new MementoException(this.GetModelDoesNotMessage(), MementoExceptionType.NotFound);
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
			modelQuery = this.FilterQueryable(modelQuery, modelFilter);
			modelCountQuery = this.FilterQueryable(modelCountQuery, modelFilter);

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

		#region [Methods] Model
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
		/// Updates the models relations.
		/// </summary>
		/// 
		/// <param name="sourceModel">The source model.</param>
		/// <param name="targetModel">The target model.</param>
		protected abstract void UpdateModelRelations(TModel sourceModel, TModel targetModel);
		#endregion

		#region [Methods] Queryable
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
		protected abstract IQueryable<TModel> FilterQueryable(IQueryable<TModel> modelQueryable, TModelFilter modelFilter);
		#endregion

		#region [Methods] Messages
		/// <summary>
		/// Returns a message indicating that the given models field is invalid.
		/// </summary>
		protected abstract string GetModelDoesNotMessage();

		/// <summary>
		/// Returns a message indicating that the given models field is invalid.
		/// </summary>
		/// 
		/// <param name="expression">The expression.</param>
		protected abstract string GetModelHasDuplicateFieldMessage<TProperty>(Expression<Func<TModel, TProperty>> expression);

		/// <summary>
		/// Returns a message indicating that the given models field is invalid.
		/// </summary>
		/// 
		/// <param name="expression">The expression.</param>
		protected abstract string GetModelHasInvalidFieldMessage<TProperty>(Expression<Func<TModel, TProperty>> expression);
		#endregion
	}
}