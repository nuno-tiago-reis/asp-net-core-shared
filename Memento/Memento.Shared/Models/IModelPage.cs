using System;
using System.Collections.Generic;

namespace Memento.Shared.Models
{
	/// <summary>
	/// Defines a generic interface for a model page.
	/// Provides properties to paginate the model queries.
	/// </summary>
	/// 
	/// <typeparam name="TModel">The model type.</typeparam>
	public interface IModelPage<TModel> : IList<TModel>
		where TModel : class, IModel
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the number of the current page.
		/// </summary>
		int PageNumber { get; set; }

		/// <summary>
		/// Gets or sets the size of the current page.
		/// </summary>
		int PageSize { get; set; }

		/// <summary>
		/// Gets or sets the total count of model pages.
		/// </summary>
		int TotalPages { get; set; }

		/// <summary>
		/// Gets or sets the total count of model instances.
		/// </summary>
		int TotalCount { get; set; }

		/// <summary>
		/// Gets or sets the parameter on which the results were ordered.
		/// </summary>
		Enum OrderBy { get; set; }

		/// <summary>
		/// Gets or sets the direction on which the results were ordered.
		/// </summary>
		Enum OrderDirection { get; set; }
		#endregion
	}
}