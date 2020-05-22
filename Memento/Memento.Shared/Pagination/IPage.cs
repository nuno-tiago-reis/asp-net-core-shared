using System;
using System.Collections.Generic;

namespace Memento.Shared.Pagination
{
	/// <summary>
	/// Defines a generic interface for a page.
	/// Provides properties to paginate queries.
	/// </summary>
	/// 
	/// <typeparam name="T">The type.</typeparam>
	public interface IPage<T> : IList<T>
		where T : class
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