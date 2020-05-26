using AutoMapper;
using Memento.Shared.Models.Pagination;

namespace Memento.Shared.Configuration
{
	/// <summary>
	/// Implements the 'MementoMapper' profile.
	/// </summary>
	/// 
	/// <seealso cref="Profile" />
	public abstract class MementoMapperProfile : Profile
	{
		#region [Constructor]
		/// <summary>
		/// Initializes a new instance of the <see cref="MementoMapperProfile"/> class.
		/// </summary>
		public MementoMapperProfile()
		{
			this.CreateMappings();
		}
		#endregion

		#region [Methods]
		/// <summary>
		/// Creates the mapping configurations.
		/// </summary>
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