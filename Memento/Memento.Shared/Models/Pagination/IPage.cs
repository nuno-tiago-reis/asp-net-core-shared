using System.Collections.Generic;

namespace Memento.Shared.Models.Pagination
{
	/// <summary>
	/// Defines a generic interface for a page.
	/// Provides properties to paginate queries.
	/// </summary>
	/// 
	/// <typeparam name="T">The type.</typeparam>
	public interface IPage<T> : IList<T>
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the number of the current page.
		/// </summary>
		int PageNumber { get; }

		/// <summary>
		/// Gets or sets the size of the current page.
		/// </summary>
		int PageSize { get; }

		/// <summary>
		/// Gets or sets the total count of item pages.
		/// </summary>
		int TotalPages { get; }

		/// <summary>
		/// Gets or sets the total count of items.
		/// </summary>
		int TotalItems { get; }

		/// <summary>
		/// Gets or sets the parameter on which the items were ordered.
		/// </summary>
		string OrderBy { get; }

		/// <summary>
		/// Gets or sets the direction on which the items were ordered.
		/// </summary>
		string OrderDirection { get; }

		/// <summary>
		/// Gets or sets the items.
		/// </summary>
		T[] Items { get; }
		#endregion
	}
}