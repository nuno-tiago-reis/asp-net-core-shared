using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Memento.Shared.Models.Pagination
{
	/// <summary>
	/// Implements the Page json converter.
	/// 
	/// <seealso cref="Page{T}"/>
	/// </summary>
	public sealed class PageJsonConverter : JsonConverterFactory
	{
		#region [Methods]
		/// <inheritdoc />
		public override bool CanConvert(Type typeToConvert)
		{
			if (!typeToConvert.IsGenericType)
			{
				return false;
			}

			if (typeToConvert.GetGenericTypeDefinition() != typeof(Page<>))
			{
				return false;
			}

			return true;
		}

		/// <inheritdoc />
		public override JsonConverter CreateConverter(Type type, JsonSerializerOptions options)
		{
			var genericType = type.GetGenericArguments()[0];

			JsonConverter converter = (JsonConverter)Activator.CreateInstance(typeof(PageJsonConverterInner<>).MakeGenericType
			(
				new Type[] { genericType }), BindingFlags.Instance | BindingFlags.Public,
				binder: null,
				args: new object[] { options },
				culture: null)
			;

			return converter;
		}
		#endregion

		/// <summary>
		/// Implements the json converter for a page.
		/// </summary>
		/// 
		/// <typeparam name="T">The type.</typeparam>
		private sealed class PageJsonConverterInner<T> : JsonConverter<Page<T>>
		{
			#region [Attributes]
			/// <summary>
			/// The type.
			/// </summary>
			private readonly Type Type;

			/// <summary>
			/// The type converter.
			/// </summary>
			private readonly JsonConverter<IList<T>> TypeConverter;
			#endregion

			#region [Constructors]
			/// <summary>
			/// Initializes a new instance of the <see cref="PageJsonConverterInner{T}"/> class.
			/// </summary>
			/// 
			/// <param name="options">The options.</param>
			public PageJsonConverterInner(JsonSerializerOptions options)
			{
				// Cache the types
				this.Type = typeof(IList<T>);

				// For performance, use the existing converter if available
				this.TypeConverter = (JsonConverter<IList<T>>)options.GetConverter(this.Type);
			}
			#endregion

			#region [Methods]
			/// <inheritdoc />
			public override Page<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				if (reader.TokenType != JsonTokenType.StartObject)
				{
					throw new JsonException();
				}

				int pageNumber = 0;
				int pageSize = 0;
				int totalPages = 0;
				int totalItems = 0;
				string orderBy = default;
				string orderDirection = default;
				var items = new List<T>();

				while (reader.Read())
				{
					if (reader.TokenType == JsonTokenType.EndObject)
					{
						return new Page<T>(items, totalItems, totalPages, pageNumber, pageSize, orderBy, orderDirection);
					}

					if (reader.TokenType != JsonTokenType.PropertyName)
					{
						throw new JsonException();
					}

					string propertyName = reader.GetString();

					// PageSize
					if (this.PropertyNameMatches(propertyName, nameof(Page<T>.PageSize), options))
					{
						pageSize = JsonSerializer.Deserialize<int>(ref reader, options);
					}

					// PageNumber
					if (this.PropertyNameMatches(propertyName, nameof(Page<T>.PageNumber), options))
					{
						pageNumber = JsonSerializer.Deserialize<int>(ref reader, options);
					}

					// TotalPages
					if (this.PropertyNameMatches(propertyName, nameof(Page<T>.TotalPages), options))
					{
						totalPages = JsonSerializer.Deserialize<int>(ref reader, options);
					}

					// TotalItems
					if (this.PropertyNameMatches(propertyName, nameof(Page<T>.TotalItems), options))
					{
						totalItems = JsonSerializer.Deserialize<int>(ref reader, options);
					}

					// OrderBy
					if (this.PropertyNameMatches(propertyName, nameof(Page<T>.OrderBy), options))
					{
						orderBy = JsonSerializer.Deserialize<string>(ref reader, options);
					}

					// OrderDirection
					if (this.PropertyNameMatches(propertyName, nameof(Page<T>.OrderDirection), options))
					{
						orderDirection = JsonSerializer.Deserialize<string>(ref reader, options);
					}

					// Items
					if (this.PropertyNameMatches(propertyName, nameof(Page<T>.Items), options))
					{
						if (this.TypeConverter != null)
						{
							reader.Read();
							items.AddRange(this.TypeConverter.Read(ref reader, this.Type, options));
						}
						else
						{
							items.AddRange(JsonSerializer.Deserialize<IEnumerable<T>>(ref reader, options));
						}
					}
				}

				throw new JsonException();
			}

			/// <inheritdoc />
			public override void Write(Utf8JsonWriter writer, Page<T> page, JsonSerializerOptions options)
			{
				writer.WriteStartObject();

				// PageSize
				writer.WritePropertyName(ConvertPropertyName(nameof(page.PageSize), options));
				JsonSerializer.Serialize(writer, page.PageSize, options);

				// PageNumber
				writer.WritePropertyName(ConvertPropertyName(nameof(page.PageNumber), options));
				JsonSerializer.Serialize(writer, page.PageNumber, options);

				// TotalPages
				writer.WritePropertyName(ConvertPropertyName(nameof(page.TotalPages), options));
				JsonSerializer.Serialize(writer, page.TotalPages, options);

				// TotalItems
				writer.WritePropertyName(ConvertPropertyName(nameof(page.TotalItems), options));
				JsonSerializer.Serialize(writer, page.TotalItems, options);

				// OrderBy
				writer.WritePropertyName(ConvertPropertyName(nameof(page.OrderBy), options));
				JsonSerializer.Serialize(writer, page.OrderBy, options);

				// OrderDirection
				writer.WritePropertyName(ConvertPropertyName(nameof(page.OrderDirection), options));
				JsonSerializer.Serialize(writer, page.OrderDirection, options);

				// Items
				writer.WritePropertyName(ConvertPropertyName(nameof(page.Items), options));
				if (this.TypeConverter != null)
				{
					this.TypeConverter.Write(writer, page.Items, options);
				}
				else
				{
					JsonSerializer.Serialize(writer, page.Items, options);
				}

				writer.WriteEndObject();
			}
			#endregion

			#region [Methods] Utility
			/// <summary>
			/// Converts a property name according to the given options.
			/// </summary>
			/// 
			/// <param name="sourceName">The source name.</param>
			/// <param name="options">The options.</param>
			private string ConvertPropertyName(string sourceName, JsonSerializerOptions options)
			{
				if (options != null && options.PropertyNamingPolicy != null)
				{
					return options.PropertyNamingPolicy.ConvertName(sourceName);
				}
				else
				{
					return sourceName;
				}
			}

			/// <summary>
			/// Checks if a property name matches according to the given options.
			/// </summary>
			/// 
			/// <param name="sourceName">The source name.</param>
			/// <param name="targetName">The target name.</param>
			/// <param name="options">The options.</param>
			private bool PropertyNameMatches(string sourceName, string targetName, JsonSerializerOptions options)
			{
				if (options.PropertyNameCaseInsensitive == false && sourceName == targetName)
				{
					return true;
				}

				if (options.PropertyNameCaseInsensitive == true && sourceName.Equals(targetName, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}

				return false;
			}
			#endregion
		}
	}
}