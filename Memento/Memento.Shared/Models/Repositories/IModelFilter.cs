using JetBrains.Annotations;
using System;

namespace Memento.Shared.Models.Repositories
{
	/// <summary>
	/// Defines a generic interface for a model filter.
	/// Provides properties to filter the model queries.
	/// </summary>
	/// 
	/// <typeparam name="TModelFilterOrderBy">The model filter order by type.</typeparam>
	/// <typeparam name="TModelFilterOrderDirection">The model filter order direction type.</typeparam>
	[UsedImplicitly]
	public interface IModelFilter<TModelFilterOrderBy, TModelFilterOrderDirection>
		where TModelFilterOrderBy : Enum
		where TModelFilterOrderDirection : Enum
	{
		#region [Properties]
		/// <summary>
		/// Gets or sets the page number.
		/// </summary>
		[UsedImplicitly]
		int PageNumber { get; set; }

		/// <summary>
		/// Gets or sets the page size.
		/// </summary>
		[UsedImplicitly]
		int PageSize { get; set; }

		/// <summary>
		/// Gets or sets the order by value.
		/// </summary>
		[UsedImplicitly]
		TModelFilterOrderBy OrderBy { get; set; }

		/// <summary>
		/// Gets or sets the order direction value.
		/// </summary>
		[UsedImplicitly]
		TModelFilterOrderDirection OrderDirection { get; set; }
		#endregion
	}
}