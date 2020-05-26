using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Memento.Shared.Models.Bindings
{
	/// <summary>
	/// Implements a datetime model binder provider that uses an UTC aware binder.
	/// </summary>
	/// 
	/// <seealso cref="IModelBinderProvider" />
	public sealed class DateTimeModelBinderProvider : IModelBinderProvider
	{
		#region [Methods]
		/// <inheritdoc />
		public IModelBinder GetBinder(ModelBinderProviderContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException(nameof(context));
			}

			var modelType = context.Metadata.UnderlyingOrModelType;
			if (modelType == typeof(DateTime))
			{
				return new UtcAwareDateTimeModelBinder();
			}

			return null;
		}
		#endregion
	}
}