using JetBrains.Annotations;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;

namespace Memento.Shared.Models.Repositories
{
	/// <summary>
	/// Implements the generic interface for a model filter.
	/// Provides properties to filter the model queries.
	/// </summary>
	/// 
	/// <typeparam name="TModelFilterOrderBy">The model filter order by type.</typeparam>
	/// <typeparam name="TModelFilterOrderDirection">The model filter order direction type.</typeparam>
	[UsedImplicitly]
	public abstract class ModelFilter<TModelFilterOrderBy, TModelFilterOrderDirection> : IModelFilter<TModelFilterOrderBy, TModelFilterOrderDirection>
		where TModelFilterOrderBy : Enum
		where TModelFilterOrderDirection : Enum
	{
		#region [Constants]
		/// <summary>
		/// The maximum page size.
		/// </summary>
		public const int MAXIMUM_PAGE_SIZE = 50;

		/// <summary>
		/// The default page size.
		/// </summary>
		public const int DEFAULT_PAGE_SIZE = 10;

		/// <summary>
		/// The minimum page size.
		/// </summary>
		public const int MINIMUM_PAGE_SIZE = 1;
		#endregion

		#region [Attributes]
		/// <summary>
		/// The page number.
		/// </summary>
		private int InnerPageNumber;

		/// <summary>
		/// The page size.
		/// </summary>
		private int InnerPageSize;
		#endregion

		#region [Properties]
		/// <inheritdoc />
		[UsedImplicitly]
		public int PageNumber
		{
			get { return this.InnerPageNumber; }
			set { this.InnerPageNumber = Math.Max(value, 1); }
		}

		/// <inheritdoc />
		[UsedImplicitly]
		public int PageSize
		{
			get { return this.InnerPageSize; }
			set { this.InnerPageSize = Math.Min(Math.Max(value, MINIMUM_PAGE_SIZE), MAXIMUM_PAGE_SIZE); }
		}

		/// <inheritdoc />
		[UsedImplicitly]
		public TModelFilterOrderBy OrderBy { get; set; }

		/// <inheritdoc />
		[UsedImplicitly]
		public TModelFilterOrderDirection OrderDirection { get; set; }
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="ModelFilter{TModelFilterOrder, TModelFilterOrderDirection}"/> class.
		/// </summary>
		protected ModelFilter()
		{
			this.PageNumber = 1;
			this.PageSize = DEFAULT_PAGE_SIZE;
		}
		#endregion

		#region [Methods]
		/// <summary>
		/// Reads the filter from a query string.
		/// </summary>
		/// 
		/// <param name="query">The query.</param>
		[UsedImplicitly]
		public virtual void ReadFromQuery(Dictionary<string, StringValues> query)
		{
			if (query != null)
			{
				this.ReadFilterFromQuery(query);
				this.ReadPagingFromQuery(query);
				this.ReadOrderingFromQuery(query);
			}
		}

		/// <summary>
		/// Writes the filter to a query string.
		/// </summary>
		[UsedImplicitly]
		public virtual Dictionary<string, string> WriteToQuery()
		{
			var query = new Dictionary<string, string>();

			this.WriteFilterToQuery(query);
			this.WritePagingToQuery(query);
			this.WriteOrderingToQuery(query);

			return query;
		}

		/// <summary>
		/// Reads the filters remaining properties from a query string.
		/// </summary>
		/// 
		/// <param name="query">The query.</param>
		protected abstract void ReadFilterFromQuery(Dictionary<string, StringValues> query);


		/// <summary>
		/// Reads the filters paging properties from a query string.
		/// </summary>
		/// 
		/// <param name="query">The query.</param>
		protected virtual void ReadPagingFromQuery(Dictionary<string, StringValues> query)
		{
			// PageNumber
			if (query.TryGetValue(nameof(this.PageNumber), out var pageNumberQuery))
			{
				if (int.TryParse(pageNumberQuery, out var pageNumber))
				{
					this.PageNumber = pageNumber;
				}
			}

			// PageSize
			if (query.TryGetValue(nameof(this.PageSize), out var pageSizeQuery))
			{
				if (int.TryParse(pageSizeQuery, out var pageSize))
				{
					this.PageSize = pageSize;
				}
			}
		}

		/// <summary>
		/// Reads the filters ordering properties from a query string.
		/// </summary>
		/// 
		/// <param name="query">The query.</param>
		protected virtual void ReadOrderingFromQuery(Dictionary<string, StringValues> query)
		{
			// OrderBy
			if (query.TryGetValue(nameof(this.OrderBy), out var orderByQuery))
			{
				if (Enum.TryParse(typeof(TModelFilterOrderBy), orderByQuery, out var orderBy))
				{
					this.OrderBy = (TModelFilterOrderBy)orderBy;
				}
			}

			// OrderDirection
			if (query.TryGetValue(nameof(this.OrderDirection), out var orderDirectionQuery))
			{
				if (Enum.TryParse(typeof(TModelFilterOrderDirection), orderDirectionQuery, out var orderDirection))
				{
					this.OrderDirection = (TModelFilterOrderDirection)orderDirection;
				}
			}
		}

		/// <summary>
		/// Writes the filters remaining properties to a query string.
		/// </summary>
		/// 
		/// <param name="query">The query.</param>
		protected abstract void WriteFilterToQuery(Dictionary<string, string> query);

		/// <summary>
		/// Writes the filters paging properties to a query string.
		/// </summary>
		/// 
		/// <param name="query">The query.</param>
		protected virtual void WritePagingToQuery(Dictionary<string, string> query)
		{
			// PageNumber
			query.Add(nameof(this.PageNumber), this.PageNumber.ToString());

			// PageSize
			query.Add(nameof(this.PageSize), this.PageSize.ToString());
		}

		/// <summary>
		/// Writes the filters ordering properties to a query string.
		/// </summary>
		/// 
		/// <param name="query">The query.</param>
		protected virtual void WriteOrderingToQuery(Dictionary<string, string> query)
		{
			// OrderBy
			query.Add(nameof(this.OrderBy), this.OrderBy.ToString());

			// OrderDirection
			query.Add(nameof(this.OrderDirection), this.OrderDirection.ToString());
		}
		#endregion
	}
}