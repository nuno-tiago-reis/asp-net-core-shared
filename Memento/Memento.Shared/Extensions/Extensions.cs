using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

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

		#region [Extensions] Generic
		/// <summary>
		/// Clamps a value according to the given minimum and maximum.
		/// </summary>
		/// 
		/// <param name="value">The value.</param>
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
		/// <param name="value">The value.</param>
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
		/// <param name="value">The value.</param>
		/// <param name="minimum">The minimum.</param>
		public static T Ceil<T>(this T value, T minimum) where T : IComparable<T>
		{
			var result = value;
			if (value.CompareTo(minimum) < 0)
				result = minimum;
			return result;
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
	}
}