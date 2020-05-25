using AutoMapper;
using Memento.Shared.Pagination;

namespace Memento.Movies.Shared.Configuration
{
	/// <summary>
	/// Implements the 'AutoMapper' settings.
	/// </summary>
	/// 
	/// <seealso cref="Profile" />
	public abstract class AutoMapperSettings : Profile
	{
		#region [Constructor]
		/// <summary>
		/// Initializes a new instance of the <see cref="AutoMapperSettings"/> class.
		/// </summary>
		public AutoMapperSettings()
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