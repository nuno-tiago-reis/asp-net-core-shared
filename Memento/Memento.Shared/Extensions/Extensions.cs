using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace Memento.Shared.Extensions
{
	/// <summary>
	/// Implements several extension methods.
	/// </summary>
	[UsedImplicitly]
	public static class Extensions
	{
		#region [Extensions] Byte[]
		/// <summary>
		/// Converts the string to an utf8 byte array.
		/// </summary>
		/// 
		/// <param name="string">The string.</param>
		[UsedImplicitly]
		public static byte[] GetBytes(this string @string)
		{
			return Encoding.UTF8.GetBytes(@string);
		}

		/// <summary>
		/// Converts the byte array to an utf8 string.
		/// </summary>
		/// 
		/// <param name="bytes">The bytes.</param>
		[UsedImplicitly]
		public static string GetString(this byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes);
		}
		#endregion

		#region [Extensions] DateTime
		/// <summary>
		/// Converts the DateTime to an UTC string.
		/// </summary>
		///
		/// <param name="dateTime">The date time.</param>
		[UsedImplicitly]
		public static string ToUtcString(this DateTime dateTime)
		{
			return dateTime.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
		}
		#endregion

		#region [Extensions] Enum
		/// <summary>
		/// Returns a message using the localized display name of the expression field.
		/// </summary>
		///
		/// <param name="enum">The enum.</param>
		/// <param name="message">The message.</param>
		[UsedImplicitly]
		public static string GetLocalizedMessage(this Enum @enum, string message)
		{
			var member = @enum.GetType().GetMember(@enum.ToString()).First();
			var memberAttribute = member.GetCustomAttribute<DisplayAttribute>();

			return string.Format(message, memberAttribute.GetName() ?? member.Name.SpacesFromCamel());
		}

		/// <summary>
		/// Returns a message using the localized display name of the enum value.
		/// </summary>
		/// 
		/// <param name="enum">The enum.</param>
		[UsedImplicitly]
		public static string GetLocalizedName(this Enum @enum)
		{
			var member = @enum.GetType().GetMember(@enum.ToString()).First();
			var memberAttribute = member.GetCustomAttribute<DisplayAttribute>();

			return memberAttribute.GetName() ?? member.Name.SpacesFromCamel();
		}
		#endregion

		#region [Extensions] EditForm
		/// <summary>
		/// Submits the form.
		/// </summary>
		/// 
		/// <param name="form">The form.</param>
		[UsedImplicitly]
		public static async Task SubmitAsync(this EditForm form)
		{
			if (form.OnSubmit.HasDelegate)
			{
				// When using OnSubmit, the developer takes control of the validation lifecycle
				await form.OnSubmit.InvokeAsync(form.EditContext);
			}
			else
			{
				// Otherwise, the system implicitly runs validation on form submission
				var isValid = form.EditContext.Validate();

				if (isValid && form.OnValidSubmit.HasDelegate)
				{
					await form.OnValidSubmit.InvokeAsync(form.EditContext);
				}

				if (!isValid && form.OnInvalidSubmit.HasDelegate)
				{
					await form.OnInvalidSubmit.InvokeAsync(form.EditContext);
				}
			}
		}
		#endregion

		#region [Extensions] Generic
		/// <summary>
		/// Returns a message using the localized display name of the expression field.
		/// </summary>
		/// 
		/// <param name="_">The object.</param>
		/// <param name="expression">The expression.</param>
		/// <param name="message">The message.</param>
		[UsedImplicitly]
		public static string GetLocalizedMessage<TObject, TProperty>(this TObject _, Expression<Func<TObject, TProperty>> expression, string message)
		{
			var property = ((MemberExpression)expression.Body).Member;
			var propertyDisplayName = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

			return string.Format(message, propertyDisplayName?.GetName() ?? property.Name.SpacesFromCamel());
		}

		/// <summary>
		/// Returns the localized display name of the expression field.
		/// </summary>
		/// 
		/// <param name="_">The object.</param>
		/// <param name="expression">The expression.</param>
		[UsedImplicitly]
		public static string GetLocalizedName<TObject, TProperty>(this TObject _, Expression<Func<TObject, TProperty>> expression)
		{
			var property = ((MemberExpression)expression.Body).Member;
			var propertyDisplayName = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

			return propertyDisplayName?.GetName() ?? property.Name.SpacesFromCamel();
		}

		/// <summary>
		/// Clamps a value according to the given minimum and maximum.
		/// </summary>
		///
		/// <param name="value">The value.</param>
		/// <param name="minimum">The minimum.</param>
		/// <param name="maximum">The maximum.</param>
		[UsedImplicitly]
		public static T Clamp<T>(this T value, T minimum, T maximum) where T : IComparable<T>
		{
			var result = value;
			if (value.CompareTo(maximum) > 0)
				result = maximum;
			if (value.CompareTo(minimum) < 0)
				result = minimum;
			return result;
		}

		/// <summary>
		/// Floors a value according to the given maximum.
		/// </summary>
		///
		/// <param name="value">The value.</param>
		/// <param name="maximum">The maximum.</param>
		[UsedImplicitly]
		public static T Floor<T>(this T value, T maximum) where T : IComparable<T>
		{
			var result = value;
			if (value.CompareTo(maximum) > 0)
				result = maximum;
			return result;
		}

		/// <summary>
		/// Ceils a value according to the given minimum.
		/// </summary>
		///
		/// <param name="value">The value.</param>
		/// <param name="minimum">The minimum.</param>
		[UsedImplicitly]
		public static T Ceil<T>(this T value, T minimum) where T : IComparable<T>
		{
			var result = value;
			if (value.CompareTo(minimum) < 0)
				result = minimum;
			return result;
		}
		#endregion

		#region [Extensions] JsonOptions
		/// <summary>
		/// Configures the default options for the JsonSerializerOptions.
		/// </summary>
		///
		/// <param name="options">The options.</param>
		[UsedImplicitly]
		public static void ConfigureDefaultOptions(this JsonSerializerOptions options)
		{
			// convert enums to strings
			options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
			// don't convert dictionary keys
			options.DictionaryKeyPolicy = null;
			// don't ignore null values
			options.IgnoreNullValues = false;
			// ignore casing when deserializing
			options.PropertyNameCaseInsensitive = true;
			// convert properties to camel case
			options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			// ignore comments
			options.ReadCommentHandling = JsonCommentHandling.Skip;
		}
		#endregion

		#region [Extensions] Expression
		/// <summary>
		/// Returns a message using the name of the expression member.
		/// </summary>
		///
		/// <param name="expression">The expression.</param>
		[UsedImplicitly]
		public static string GetName(this Expression expression)
		{
			if (expression is MemberExpression member)
			{
				var property = member.Member;

				return property.Name;
			}

			if (expression is LambdaExpression lambda)
			{
				var property = lambda.Body as MemberExpression;

				return property?.Member.Name;
			}

			return null;
		}

		/// <summary>
		/// Returns a message using the localized display name of the expression member.
		/// </summary>
		///
		/// <param name="expression">The expression.</param>
		[UsedImplicitly]
		public static string GetDisplayName(this Expression expression)
		{
			if (expression is MemberExpression member)
			{
				var property = member.Member;
				var propertyDisplayName = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

				return propertyDisplayName?.GetName() ?? property.Name.SpacesFromCamel();
			}

			if (expression is LambdaExpression lambda)
			{
				var property = lambda.Body as MemberExpression;
				var propertyDisplayName = property?.Member.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

				return propertyDisplayName?.GetName() ?? property?.Member.Name.SpacesFromCamel();
			}

			return null;
		}
		#endregion

		#region [Extensions] MemberExpression
		/// <summary>
		/// Returns a message using the name of the expression member.
		/// </summary>
		///
		/// <param name="expression">The expression.</param>
		[UsedImplicitly]
		public static string GetName(this MemberExpression expression)
		{
			var property = expression.Member;

			return property.Name;
		}

		/// <summary>
		/// Returns a message using the localized display name of the expression member.
		/// </summary>
		///
		/// <param name="expression">The expression.</param>
		[UsedImplicitly]
		public static string GetDisplayName(this MemberExpression expression)
		{
			var property = expression.Member;
			var propertyDisplayName = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

			return propertyDisplayName?.GetName() ?? property.Name.SpacesFromCamel();
		}
		#endregion

		#region [Extensions] MemberInfo
		/// <summary>
		/// Returns a message using the name of the member.
		/// </summary>
		///
		/// <param name="member">The member.</param>
		[UsedImplicitly]
		public static string GetName(this MemberInfo member)
		{
			return member.Name;
		}

		/// <summary>
		/// Returns a message using the localized display name of the member.
		/// </summary>
		///
		/// <param name="member">The member.</param>
		[UsedImplicitly]
		public static string GetDisplayName(this MemberInfo member)
		{
			var propertyDisplayName = member.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

			return propertyDisplayName?.GetName() ?? member.Name.SpacesFromCamel();
		}
		#endregion

		#region [Extensions] String
		/// <summary>
		/// Spaces a string according to its camel case.
		/// </summary>
		/// 
		/// <param name="string">The string.</param>
		[UsedImplicitly]
		public static string SpacesFromCamel(this string @string)
		{
			if (@string.Length <= 0)
				return @string;

			var result = new List<char>();
			var array = @string.ToCharArray();

			foreach (char item in array)
			{
				if (char.IsUpper(item))
				{
					result.Add(' ');
				}
				result.Add(item);
			}

			return new string(result.ToArray()).Trim();
		}

		/// <summary>
		/// Converts a string to camel case.
		/// </summary>
		/// 
		/// <param name="string">The string.</param>
		[UsedImplicitly]
		public static string ToCamelCase(this string @string)
		{
			if (@string.Length < 2)
				return @string.ToLower();

			// Start with the first character
			var result = @string.Substring(0, 1).ToLower();

			// Remove extra whitespaces
			result += Regex.Replace(@string.Substring(1), " +([a-zA-Z])", "$1".ToUpper());

			return result;
		}

		/// <summary>
		/// Converts a string to pascal case.
		/// </summary>
		/// 
		/// <param name="string">The string.</param>
		[UsedImplicitly]
		public static string ToPascalCase(this string @string)
		{
			if (@string == null)
				return null;

			if (@string.Length < 2)
				return @string.ToUpper();

			// Start with the first character
			var result = @string.Substring(0, 1).ToUpper();

			// Remove extra whitespaces
			result += Regex.Replace(@string.Substring(1), " +([a-zA-Z])", "$1".ToUpper());

			return result;
		}

		/// <summary>
		/// Converts a string to proper case.
		/// </summary>
		/// 
		/// <param name="string">The string.</param>
		[UsedImplicitly]
		public static string ToProperCase(this string @string)
		{
			if (@string == null)
				return null;

			if (@string.Length < 2)
				return @string.ToUpper();

			// Start with the first character
			var result = @string.Substring(0, 1).ToUpper();

			// Remove extra whitespaces
			result += Regex.Replace(@string.Substring(1), " +([a-zA-Z])", " $1".ToUpper());

			// Insert missing whitespaces
			result = Regex.Replace(result, "([a-z])([A-Z])", "$1 $2");

			return result;
		}

		/// <summary>
		/// Compares two strings with the invariant culture and ignoring case.
		/// </summary>
		/// 
		/// <param name="string">The string.</param>
		/// <param name="value">The string to compare to.</param>
		[UsedImplicitly]
		public static bool EqualsNormalized(this string @string, string value)
		{
			return @string.Equals(value, StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Converts the utf8 string to a base64 string.
		/// </summary>
		/// 
		/// <param name="string">The string.</param>
		[UsedImplicitly]
		public static string ConvertToBase64(this string @string)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(@string));
		}

		/// <summary>
		/// Converts the base64 string to an utf8 string.
		/// </summary>
		/// 
		/// <param name="string">The string.</param>
		[UsedImplicitly]
		public static string ConvertFromBase64(this string @string)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(@string));
		}
		#endregion

		#region [Extensions] Timer
		/// <summary>
		/// Resets the timer by stopping and starting it again. 
		/// </summary>
		/// 
		/// <param name="timer">The timer.</param>
		[UsedImplicitly]
		public static void Reset(this Timer timer)
		{
			timer.Stop();
			timer.Start();
		}
		#endregion
	}
}