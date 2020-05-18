using JetBrains.Annotations;
using System.Collections.Generic;

namespace Memento.Shared.Models.Pagination
{
	/// <summary>
	/// Defines a generic interface for a page.
	/// Provides properties to paginate queries.
	/// </summary>
	/// 
	/// <typeparam name="T">The type.</typeparam>
	[UsedImplicitly]
	public interface IPage<T> : IList<T>
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the number of the current page.
		/// </summary>
		[UsedImplicitly]
		int PageNumber { get; }

		/// <summary>
		/// Gets or sets the size of the current page.
		/// </summary>
		[UsedImplicitly]
		int PageSize { get; }

		/// <summary>
		/// Gets or sets the total count of item pages.
		/// </summary>
		[UsedImplicitly]
		int TotalPages { get; }

		/// <summary>
		/// Gets or sets the total count of items.
		/// </summary>
		[UsedImplicitly]
		int TotalItems { get; }

		/// <summary>
		/// Gets or sets the parameter on which the items were ordered.
		/// </summary>
		[UsedImplicitly]
		string OrderBy { get; }

		/// <summary>
		/// Gets or sets the direction on which the items were ordered.
		/// </summary>
		[UsedImplicitly]
		string OrderDirection { get; }

		/// <summary>
		/// Gets or sets the items.
		/// </summary>
		[UsedImplicitly]
		T[] Items { get; }
		#endregion
	}
}