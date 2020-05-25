using Memento.Shared.Pagination;
using System;
using System.Threading.Tasks;

namespace Memento.Shared.Models
{
	/// <summary>
	/// Defines a generic interface for a model repository.
	/// Provides methods to interact with the models (CRUD and more).
	/// </summary>
	/// 
	/// <typeparam name="TModel">The model type.</typeparam>
	/// <typeparam name="TModelFilter">The model filter type.</typeparam>
	/// <typeparam name="TModelFilterOrderBy">The model filter order by type.</typeparam>
	/// <typeparam name="TModelFilterOrderDirection">The model filter order direction type.</typeparam>
	public interface IModelRepository<TModel, TModelFilter, TModelFilterOrderBy, TModelFilterOrderDirection>
		where TModel : class, IModel
		where TModelFilter : class, IModelFilter<TModelFilterOrderBy, TModelFilterOrderDirection>
		where TModelFilterOrderBy : Enum
		where TModelFilterOrderDirection : Enum
	{
		#region [Methods]
		/// <summary>
		/// Creates the specified model instance.
		/// </summary>
		/// 
		/// <param name="model">The model.</param>
		Task<TModel> CreateAsync(TModel model);

		/// <summary>
		/// Updates the specified model instance.
		/// </summary>
		/// 
		/// <param name="model">The model.</param>
		Task<TModel> UpdateAsync(TModel model);

		/// <summary>
		/// Deletes the model instance matching the given id.
		/// </summary>
		/// 
		/// <param name="modelId">The model identifier.</param>
		Task DeleteAsync(long modelId);

		/// <summary>
		/// Gets the model instance matching the given id.
		/// </summary>
		/// 
		/// <param name="modelId">The model identifier.</param>
		Task<TModel> GetAsync(long modelId);

		/// <summary>
		/// Gets all the model instances matching the given filter.
		/// </summary>
		/// 
		/// <param name="modelFilter">The model filter.</param>
		Task<IPage<TModel>> GetAllAsync(TModelFilter modelFilter = null);

		/// <summary>
		/// Checks if a model instance matching the given id exists.
		/// </summary>
		/// 
		/// <param name="modelId">The model identifier.</param>
		Task<bool> ExistsAsync(long modelId);
		#endregion
	}
}