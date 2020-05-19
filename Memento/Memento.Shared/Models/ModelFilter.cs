using System;

namespace Memento.Shared.Models
{
	/// <summary>
	/// Implements the generic interface for a model filter.
	/// Provides properties to filter the model queries.
	/// </summary>
	/// 
	/// <typeparam name="TModelFilterOrderBy">The model filter order by type.</typeparam>
	/// <typeparam name="TModelFilterOrderDirection">The model filter order direction type.</typeparam>
	public abstract class ModelFilter<TModelFilterOrderBy, TModelFilterOrderDirection> : IModelFilter<TModelFilterOrderBy, TModelFilterOrderDirection>
		where TModelFilterOrderBy : Enum
		where TModelFilterOrderDirection : Enum
	{
		#region [Constants]
		/// <summary>
		/// The maximum page size.
		/// </summary>
		public const int MaximumPageSize = 50;

		/// <summary>
		/// The default page size.
		/// </summary>
		public const int DefaultPageSize = 10;

		/// <summary>
		/// The minimum page size.
		/// </summary>
		public const int MinimumPageSize = 5;
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
		public int PageNumber
		{
			get { return this.InnerPageNumber; }
			set { this.InnerPageNumber = Math.Max(value, 1); }
		}

		/// <inheritdoc />
		public int PageSize
		{
			get { return this.InnerPageSize; }
			set { this.InnerPageSize = Math.Min(Math.Max(value, MinimumPageSize), MaximumPageSize); }
		}

		/// <inheritdoc />
		public TModelFilterOrderBy OrderBy { get; set; }

		/// <inheritdoc />
		public TModelFilterOrderDirection OrderDirection { get; set; }
		#endregion

		#region [Constructors]
		/// <summary>
		/// Initializes a new instance of the <see cref="ModelFilter{TModelFilterOrder}"/> class.
		/// </summary>
		public ModelFilter()
		{
			this.PageNumber = 1;
			this.PageSize = DefaultPageSize;
		}
		#endregion
	}
}