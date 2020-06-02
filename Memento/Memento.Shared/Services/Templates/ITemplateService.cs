using JetBrains.Annotations;
using System.Threading.Tasks;

namespace Memento.Shared.Services.Templates
{
	/// <summary>
	/// Defines the generic interface for a template service.
	/// Provides methods to render template with and without parameters.
	/// </summary>
	[UsedImplicitly]
	public interface ITemplateService
	{
		#region [Methods]
		/// <summary>
		/// Renders the template with the given name.
		/// </summary>
		/// 
		/// <param name="name">The template name.</param>
		[UsedImplicitly]
		Task<string> RenderAsync(string name);

		/// <summary>
		/// Renders the template with the given name and model.
		/// </summary>
		/// 
		/// <param name="name">The template name.</param>
		/// <param name="model">The template model.</param>
		[UsedImplicitly]
		Task<string> RenderAsync<TModel>(string name, TModel model);
		#endregion
	}
}