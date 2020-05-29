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
using System.Timers;

namespace Memento.Shared.Extensions
{
	/// <summary>
	/// Implements several extension methods.
	/// </summary>
	public static class Extensions
	{
		#region [Extensions] Byte[]
		/// <summary>
		/// Converts the string to an utf8 byte array.
		/// </summary>
		/// 
		/// <param name="instance">The instance.</param>
		public static byte[] GetBytes(this string instance)
		{
			return Encoding.UTF8.GetBytes(instance);
		}

		/// <summary>
		/// Converts the byte array to an utf8 string.
		/// </summary>
		/// 
		/// <param name="instance">The instance.</param>
		public static string GetString(this byte[] instance)
		{
			return Encoding.UTF8.GetString(instance);
		}
		#endregion

		#region [Extensions] DateTime
		/// <summary>
		/// Converts the DateTime to an UTC string.
		/// </summary>
		public static String ToUtcString(this DateTime instance)
		{
			return instance.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
		}
		#endregion

		#region [Extensions] Enum
		/// <summary>
		/// Returns a message using the localized display name of the expression field.
		/// </summary>
		/// 
		/// <param name="message">The message.</param>
		public static string GetLocalizedMessage<T, P>(this Enum instance, string message)
		{
			var member = instance.GetType().GetMember(instance.ToString()).First();
			var memberAttribute = member.GetCustomAttribute<DisplayAttribute>();

			return string.Format(message, memberAttribute.GetName() ?? member.Name.SpacesFromCamel());
		}


		/// <summary>
		/// Returns a message using the localized display name of the enum value.
		/// </summary>
		public static string GetLocalizedName(this Enum instance)
		{
			var member = instance.GetType().GetMember(instance.ToString()).First();
			var memberAttribute = member.GetCustomAttribute<DisplayAttribute>();

			return memberAttribute.GetName() ?? member.Name.SpacesFromCamel();
		}
		#endregion

		#region [Extensions] Generic
		/// <summary>
		/// Returns a message using the localized display name of the expression field.
		/// </summary>
		/// 
		/// <param name="expression">The expression.</param>
		/// <param name="message">The message.</param>
		public static string GetLocalizedMessage<T, P>(this T _, Expression<Func<T, P>> expression, string message)
		{
			var property = ((MemberExpression)expression.Body).Member;
			var propertyDisplayName = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

			return string.Format(message, propertyDisplayName.GetName() ?? property.Name.SpacesFromCamel());
		}

		/// <summary>
		/// Returns the localized display name of the expression field.
		/// </summary>
		/// 
		/// <param name="expression">The expression.</param>
		public static string GetLocalizedName<T, P>(this T _, Expression<Func<T, P>> expression)
		{
			var property = ((MemberExpression)expression.Body).Member;
			var propertyDisplayName = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

			return propertyDisplayName.GetName() ?? property.Name.SpacesFromCamel();
		}

		/// <summary>
		/// Clamps a value according to the given minimum and maximum.
		/// </summary>
		/// 
		/// <param name="minimum">The minimum.</param>
		/// <param name="maximum">The maximum.</param>
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
		/// <param name="maximum">The maximum.</param>
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
		/// <param name="minimum">The minimum.</param>
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
		public static void ConfigureDefaultOptions(this JsonSerializerOptions instance)
		{
			// convert enums to strings
			instance.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, true));
			// don't convert dictionary keys
			instance.DictionaryKeyPolicy = null;
			// dont ignore null values
			instance.IgnoreNullValues = false;
			// ignore casing when deserializing
			instance.PropertyNameCaseInsensitive = true;
			// convert properties to camel case
			instance.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
		}
		#endregion

		#region [Extensions] Expression
		/// <summary>
		/// Returns a message using the name of the expression member.
		/// </summary>
		public static string GetName(this Expression instance)
		{
			if (instance is MemberExpression member)
			{
				var property = member.Member;

				return property.Name;
			}
			if (instance is LambdaExpression lambda)
			{
				var property = lambda.Body as MemberExpression;

				return property?.Member.Name;
			}

			return null;
		}

		/// <summary>
		/// Returns a message using the localized display name of the expression member.
		/// </summary>
		public static string GetDisplayName(this Expression instance)
		{
			if (instance is MemberExpression member)
			{
				var property = member.Member;
				var propertyDisplayName = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

				return propertyDisplayName?.GetName() ?? property.Name.SpacesFromCamel();
			}
			if (instance is LambdaExpression lambda)
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
		public static string GetName(this MemberExpression instance)
		{
			var property = instance.Member;

			return property.Name;
		}

		/// <summary>
		/// Returns a message using the localized display name of the expression member.
		/// </summary>
		public static string GetDisplayName(this MemberExpression instance)
		{
			var property = instance.Member;
			var propertyDisplayName = property.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

			return propertyDisplayName?.GetName() ?? property.Name.SpacesFromCamel();
		}
		#endregion

		#region [Extensions] MemberInfo
		/// <summary>
		/// Returns a message using the name of the member.
		/// </summary>
		public static string GetName(this MemberInfo instance)
		{
			return instance.Name;
		}

		/// <summary>
		/// Returns a message using the localized display name of the member.
		/// </summary>
		/// 
		/// <param name="expression">The expression.</param>
		/// <param name="message">The message.</param>
		public static string GetDisplayName(this MemberInfo instance)
		{
			var propertyDisplayName = instance.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;

			return propertyDisplayName?.GetName() ?? instance.Name.SpacesFromCamel();
		}
		#endregion

		#region [Extensions] String
		/// <summary>
		/// Spaces a string according to its camel case.
		/// </summary>
		/// 
		/// <param name="instance">The instance.</param>
		public static string SpacesFromCamel(this string instance)
		{
			if (instance.Length <= 0)
				return instance;

			var result = new List<char>();
			var array = instance.ToCharArray();

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
		/// <param name="instance">The instance.</param>
		public static string ToCamelCase(this string instance)
		{
			if (instance.Length < 2)
				return instance.ToLower();

			// Start with the first character
			string result = instance.Substring(0, 1).ToLower();

			// Remove extra whitespaces
			result += Regex.Replace(instance.Substring(1), " +([a-zA-Z])", "$1".ToUpper());

			return result;
		}

		/// <summary>
		/// Converts a string to pascal case.
		/// </summary>
		/// 
		/// <param name="instance">The instance.</param>
		public static string ToPascalCase(this string instance)
		{
			if (instance == null)
				return null;

			if (instance.Length < 2)
				return instance.ToUpper();

			// Start with the first character
			string result = instance.Substring(0, 1).ToUpper();

			// Remove extra whitespaces
			result += Regex.Replace(instance.Substring(1), " +([a-zA-Z])", "$1".ToUpper());

			return result;
		}

		/// <summary>
		/// Converts a string to proper case.
		/// </summary>
		/// 
		/// <param name="instance">The instance.</param>
		public static string ToProperCase(this string instance)
		{
			if (instance == null)
				return null;

			if (instance.Length < 2)
				return instance.ToUpper();

			// Start with the first character
			string result = instance.Substring(0, 1).ToUpper();

			// Remove extra whitespaces
			result += Regex.Replace(instance.Substring(1), " +([a-zA-Z])", " $1".ToUpper());

			// Insert missing whitespaces
			result = Regex.Replace(result, "([a-z])([A-Z])", "$1 $2");

			return result;
		}

		/// <summary>
		/// Compares two strings with the invariant culture and ignoring case.
		/// </summary>
		/// 
		/// <param name="instance">The instance.</param>
		/// <param name="value">The string to compare to.</param>
		public static bool EqualsNormalized(this string instance, string value)
		{
			return instance.Equals(value, StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Converts the utf8 string to a base64 string.
		/// </summary>
		/// 
		/// <param name="instance">The instance.</param>
		public static string ConvertToBase64(this string instance)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(instance));
		}

		/// <summary>
		/// Converts the base64 string to an utf8 string.
		/// </summary>
		/// 
		/// <param name="instance">The instance.</param>
		public static string ConvertFromBase64(this string instance)
		{
			return Encoding.UTF8.GetString(Convert.FromBase64String(instance));
		}
		#endregion

		#region [Extensions] Timer
		/// <summary>
		/// Resets the timer by stopping and starting it again. 
		/// </summary>
		/// 
		/// <param name="instance">The instance.</param>
		public static void Reset(this Timer instance)
		{
			instance.Stop();
			instance.Start();
		}
		#endregion
	}
}