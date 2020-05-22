using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Memento.Shared.Pagination
{
	/// <summary>
	/// Implements the generic interface for a page.
	/// Provides properties to paginate the queries.
	/// </summary>
	/// 
	/// <typeparam name="T">The type.</typeparam>
	public sealed class Page<T> : List<T>, IPage<T>
		where T : class
	{
		#region [Properties]
		/// <inheritdoc />
		public int PageNumber { get; set; }

		/// <inheritdoc />
		public int PageSize { get; set; }

		/// <inheritdoc />
		public int TotalPages { get; set; }

		/// <inheritdoc />
		public int TotalCount { get; set; }

		/// <inheritdoc />
		public Enum OrderBy { get; set; }

		/// <inheritdoc />
		public Enum OrderDirection { get; set; }
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="Page{T}"/> class.
		/// </summary>
		/// 
		/// <param name="items">The items.</param>
		/// <param name="itemCount">The item count.</param>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">The page size.</param>
		/// <param name="orderBy">The parameter on which the results were ordered.</param>
		/// <param name="orderDirection">The direction on which the results were ordered.</param>
		private Page(IEnumerable<T> items, int itemCount, int pageNumber, int pageSize, Enum orderBy, Enum orderDirection)
		{
			this.PageNumber = pageNumber;
			this.PageSize = pageSize;

			this.TotalPages = Math.Max(itemCount / pageSize + (itemCount % pageSize == 0 ? 0 : 1), 1);
			this.TotalCount = itemCount;

			this.OrderBy = orderBy;
			this.OrderDirection = orderDirection;

			this.AddRange(items);
		}
		#endregion

		#region [Methods]
		/// <summary>
		/// Creates a new instance of the <see cref="PagedList{T}"/> class.
		/// </summary>
		/// 
		/// <param name="enumerable">The enumerable.</param>
		/// <param name="enumerableCount">The enumerable count.</param>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">The page size.</param>
		/// <param name="orderBy">The parameter on which the results were ordered.</param>
		/// <param name="orderDirection">The direction on which the results were ordered.</param>
		public static Page<T> Create(IEnumerable<T> enumerable, int enumerableCount, int pageNumber, int pageSize, Enum orderBy, Enum orderDirection)
		{
			var items = enumerable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

			return new Page<T>(items, enumerableCount, pageNumber, pageSize, orderBy, orderDirection);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="PagedList{T}"/> class asynchronously.
		/// </summary>
		/// 
		/// <param name="queryable">The queryable.</param>
		/// <param name="queryableCount">The queryable count.</param>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">The page size.</param>
		/// <param name="orderBy">The parameter on which the results were ordered.</param>
		/// <param name="orderDirection">The direction on which the results were ordered.</param>
		public static async Task<Page<T>> CreateAsync(IQueryable<T> queryable, IQueryable<T> queryableCount, int pageNumber, int pageSize, Enum orderBy, Enum orderDirection)
		{
			var items = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

			return new Page<T>(items, await queryableCount.CountAsync(), pageNumber, pageSize, orderBy, orderDirection);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="PagedList{T}"/> class without modifying the enumerable.
		/// </summary>
		/// 
		/// <param name="enumerable">The enumerable.</param>
		/// <param name="enumerableCount">The enumerable count.</param>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">The page size.</param>
		/// <param name="orderBy">The parameter on which the results were ordered.</param>
		/// <param name="orderDirection">The direction on which the results were ordered.</param>
		public static Page<T> CreateUnmodified(IEnumerable<T> enumerable, int enumerableCount, int pageNumber, int pageSize, Enum orderBy, Enum orderDirection)
		{
			var items = enumerable.ToList();

			return new Page<T>(items, enumerableCount, pageNumber, pageSize, orderBy, orderDirection);
		}

		/// <summary>
		/// Creates a new instance of the <see cref="PagedList{T}"/> class without modifying the enumerable.
		/// </summary>
		/// 
		/// <param name="queryable">The queryable.</param>
		/// <param name="queryableCount">The queryable count.</param>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">The page size.</param>
		/// <param name="orderBy">The parameter on which the results were ordered.</param>
		/// <param name="orderDirection">The direction on which the results were ordered.</param>
		public static async Task<Page<T>> CreateUnmodifiedAsync(IQueryable<T> queryable, int queryableCount, int pageNumber, int pageSize, Enum orderBy, Enum orderDirection)
		{
			var items = await queryable.ToListAsync();

			return new Page<T>(items, queryableCount, pageNumber, pageSize, orderBy, orderDirection);
		}
		#endregion
	}
}