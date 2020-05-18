﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Memento.Shared.ModelBinding
{
	/// <summary>
	/// Implements a datetime model binder that converts the datetime kind to utc.
	/// </summary>
	/// 
	/// <seealso cref="IModelBinder" />
	public sealed class UtcAwareDateTimeModelBinder : IModelBinder
	{
		#region [Methods]
		/// <inheritdoc />
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			string modelName = bindingContext.ModelName;
			var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
			if (valueProviderResult == ValueProviderResult.None)
			{
				return Task.CompletedTask;
			}

			var modelState = bindingContext.ModelState;
			modelState.SetModelValue(modelName, valueProviderResult);

			var metadata = bindingContext.ModelMetadata;
			var type = metadata.UnderlyingOrModelType;

			string value = valueProviderResult.FirstValue;
			var culture = valueProviderResult.Culture;

			object model;
			if (string.IsNullOrWhiteSpace(value))
			{
				model = null;
			}
			else if (type == typeof(DateTime))
			{
				// You could put custom logic here to sniff the raw value and call other DateTime.Parse overloads, e.g. forcing UTC
				model = DateTime.Parse(value, culture, DateTimeStyles.AdjustToUniversal);
			}
			else if (type == typeof(DateTime?))
			{
				// You could put custom logic here to sniff the raw value and call other DateTime.Parse overloads, e.g. forcing UTC
				model = DateTime.Parse(value, culture, DateTimeStyles.AdjustToUniversal);
			}
			else
			{
				// unreachable
				throw new NotSupportedException();
			}

			// When converting value, a null model may indicate a failed conversion for an otherwise required
			// model (can't set a ValueType to null). This detects if a null model value is acceptable given the
			// current bindingContext. If not, an error is logged.
			if (model == null && !metadata.IsReferenceOrNullableType)
			{
				modelState.TryAddModelError(
					modelName,
					metadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(valueProviderResult.ToString()));
			}
			else
			{
				bindingContext.Result = ModelBindingResult.Success(model);
			}

			return Task.CompletedTask;
		}
		#endregion
	}
}