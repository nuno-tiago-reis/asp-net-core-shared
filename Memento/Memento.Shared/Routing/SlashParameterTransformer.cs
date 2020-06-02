using JetBrains.Annotations;
using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace Memento.Shared.Routing
{
	/// <summary>
	/// Implements a parameter transformer that separates words by using slashes.
	/// </summary>
	/// 
	/// <seealso cref="IOutboundParameterTransformer" />
	[UsedImplicitly]
	public sealed class SlashParameterTransformer : IOutboundParameterTransformer
	{
		#region [Methods]
		/// <summary>
		/// Transforms the outbound parameter values by splitting whenever a capital letter occurs using a slash.
		/// </summary>
		/// 
		/// <param name="value">The value.</param>
		public string TransformOutbound(object value)
		{
			if (value == null)
			{
				return null;
			}

			var parameter = string.Empty;

			var regex = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) | (?<=[^A-Z])(?=[A-Z]) | (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
			var regexTokens = regex.Split(value.ToString());

			foreach (var regexToken in regexTokens)
			{
				if (regexToken.EndsWith("y") || regexToken.EndsWith("Y"))
				{
					parameter += $"{regexToken.Substring(0, regexToken.Length - 1)}ies/";
				}
				else
				{
					parameter += $"{regexToken}s/";
				}
			}

			return parameter.ToLower();
		}
		#endregion
	}
}