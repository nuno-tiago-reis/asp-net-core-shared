using AutoMapper;
using JetBrains.Annotations;
using Memento.Shared.Models.Pagination;

namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the 'MementoMapper' profile.
	/// </summary>
	/// 
	/// <seealso cref="Profile" />
	[UsedImplicitly]
	public abstract class MementoMapperProfile : Profile
	{
		#region [Methods]
		/// <summary>
		/// Creates the mapping configurations.
		/// </summary>
		[UsedImplicitly]
		protected virtual void CreateMappings()
		{
			#region [Pagination]
			// Pagination
			this.CreateMap(typeof(Page<>), typeof(Page<>));
			#endregion
		}
		#endregion
	}
}