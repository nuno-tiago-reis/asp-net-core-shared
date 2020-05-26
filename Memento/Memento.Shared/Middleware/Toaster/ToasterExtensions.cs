using Microsoft.Extensions.DependencyInjection;
using System;

namespace Memento.Shared.Middleware.Toaster
{
	/// <summary>
	/// Implements the necessary methods to add the Toaster middleware to the ASP.NET Core Pipeline.
	/// </summary>
	public static class ToasterExtensions
	{
		#region [Extensions]
		/// <summary>
		/// Registers the Toaster middleware in the pipeline of the specified <seealso cref="IServiceCollection"/>.
		/// Configures the options using specified <seealso cref="Action{ToasterOptions}"/>
		/// </summary>
		/// 
		/// <param name="action">The action that configures the <seealso cref="ToasterOptions"/>.</param>
		public static IServiceCollection AddToaster(this IServiceCollection instance, Action<ToasterOptions> action)
		{
			// Create the options
			var options = new ToasterOptions();
			// Configure the options
			action?.Invoke(options);

			// Register the middleware
			instance.AddToaster(options);

			return instance;
		}
		#endregion
	}
}