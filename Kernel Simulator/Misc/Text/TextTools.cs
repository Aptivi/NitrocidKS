using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using KS.Languages;
using KS.Misc.Writers.DebugWriters;

namespace KS.Misc.Text
{
	public static class TextTools
	{
		private readonly static string regexMatchEnclosedStrings = @"(""(.+?)(?<![^\\]\\)"")|('(.+?)(?<![^\\]\\)')|(`(.+?)(?<![^\\]\\)`)|(?:[^\\\s]|\\.)+|\S+";

		/// <summary>
        /// Splits the string enclosed in double quotes delimited by spaces using regular expression formula
        /// </summary>
        /// <paramname="target">Target string</param>
		public static string[] SplitEncloseDoubleQuotes(this string target)
		{
			if (target is null)
				throw new Exception(Translate.DoTranslation("The target may not be null"));

			return Regex.Matches(target, regexMatchEnclosedStrings).Cast<Match>().Select(m => m.Value.ReleaseDoubleQuotes()).ToArray();
		}

		/// <summary>
        /// Splits the string enclosed in double quotes delimited by spaces using regular expression formula without releasing double quotes
        /// </summary>
        /// <paramname="target">Target string</param>
		public static string[] SplitEncloseDoubleQuotesNoRelease(this string target)
		{
			if (target is null)
				throw new Exception(Translate.DoTranslation("The target may not be null"));

			return Regex.Matches(target, regexMatchEnclosedStrings).Cast<Match>().Select(m => m.Value).ToArray();
		}

		/// <summary>
        /// Releases a string from double quotations
        /// </summary>
        /// <paramname="target">Target string</param>
        /// <returns>A string that doesn't contain double quotation marks at the start and at the end of the string</returns>
		public static string ReleaseDoubleQuotes(this string target)
		{
			if (target is null)
				throw new Exception(Translate.DoTranslation("The target may not be null"));

			string ReleasedString = target;
			if (target.StartsWith("\"") && target.EndsWith("\"") && !Equals(target, "\"") || target.StartsWith("'") && target.EndsWith("'") && !Equals(target, "'") || target.StartsWith("`") && target.EndsWith("`") && !Equals(target, "`"))
			{
				ReleasedString = ReleasedString.Remove(0, 1);
				ReleasedString = ReleasedString.Remove(ReleasedString.Length - 1);
			}
			return ReleasedString;
		}

		/// <summary>
        /// Gets the enclosed double quotes type from the string
        /// </summary>
        /// <paramname="target">Target string to query</param>
        /// <returns><seecref="EnclosedDoubleQuotesType"/> containing information about the current string enclosure</returns>
		public static EnclosedDoubleQuotesType GetEnclosedDoubleQuotesType(this string target)
		{
			if (target is null)
				throw new Exception(Translate.DoTranslation("The target may not be null"));

			var type = EnclosedDoubleQuotesType.None;
			if (target.StartsWith("\"") && target.EndsWith("\"") && !Equals(target, "\""))
			{
				type = EnclosedDoubleQuotesType.DoubleQuotes;
			}
			else if (target.StartsWith("'") && target.EndsWith("'") && !Equals(target, "'"))
			{
				type = EnclosedDoubleQuotesType.SingleQuotes;
			}
			else if (target.StartsWith("`") && target.EndsWith("`") && !Equals(target, "`"))
			{
				type = EnclosedDoubleQuotesType.Backticks;
			}
			return type;
		}

		/// <summary>
        /// Truncates the string if the string is larger than the threshold, otherwise, returns an unchanged string
        /// </summary>
        /// <paramname="target">Source string to truncate</param>
        /// <paramname="threshold">Max number of string characters</param>
        /// <returns>Truncated string</returns>
		public static string Truncate(this string target, int threshold)
		{
			if (target is null)
				throw new Exception(Translate.DoTranslation("The target may not be null"));

			// Try to truncate string. If the string length is bigger than the threshold, it'll be truncated to the length of
			// the threshold, putting three dots next to it. We don't use ellipsis marks here because we're dealing with the
			// terminal, and some terminals and some monospace fonts may not support that character, so we mimick it by putting
			// the three dots.
			if (target.Length > threshold)
			{
				return target.Substring(0, threshold - 1) + "...";
			}
			else
			{
				return target;
			}
		}

		/// <summary>
        /// Makes a string array with new line as delimiter
        /// </summary>
        /// <paramname="target">Target string</param>
        /// <returns>List of words that are separated by the new lines</returns>
		public static string[] SplitNewLines(this string target)
		{
			if (target is null)
				throw new Exception(Translate.DoTranslation("The target may not be null"));

			return target.Replace(Convert.ToChar(13).ToString(), "").Split(Convert.ToChar(10));
		}

		/// <summary>
        /// Checks to see if the string starts with any of the values
        /// </summary>
        /// <paramname="target">Target string</param>
        /// <paramname="values">Values</param>
        /// <returns>True if the string starts with any of the values specified in the array. Otherwise, false.</returns>
		public static bool StartsWithAnyOf(this string target, string[] values)
		{
			if (target is null)
				throw new Exception(Translate.DoTranslation("The target may not be null"));

			bool started = false;
			foreach (var value in values)
			{
				if (target.StartsWith(value))
					started = true;
			}
			return started;
		}

		/// <summary>
        /// Checks to see if the string contains any of the target strings.
        /// </summary>
        /// <paramname="source">Source string</param>
        /// <paramname="targets">Target strings</param>
        /// <returns>True if one of them is found; otherwise, false.</returns>
		public static bool ContainsAnyOf(this string source, string[] targets)
		{
			if (source is null)
				throw new Exception(Translate.DoTranslation("The source may not be null"));

			foreach (var target in targets)
			{
				if (source.Contains(target))
					return true;
			}
			return false;
		}

		/// <summary>
        /// Replaces all the instances of strings with a string
        /// </summary>
        /// <paramname="target">Target string</param>
        /// <paramname="toBeReplaced">Strings to be replaced</param>
        /// <paramname="toReplace">String to replace with</param>
        /// <returns>Modified string</returns>
        /// <exceptioncref="ArgumentNullException"></exception>
		public static string ReplaceAll(this string target, string[] toBeReplaced, string toReplace)
		{
			if (target is null)
				throw new Exception(Translate.DoTranslation("The target may not be null"));
			if (toBeReplaced is null || toBeReplaced.Length == 0)
				throw new Exception(Translate.DoTranslation("Array of to be replaced strings may not be null"));

			foreach (var ReplaceTarget in toBeReplaced)
				target = target.Replace(ReplaceTarget, toReplace);
			return target;
		}

		/// <summary>
        /// Replaces all the instances of strings with a string assigned to each entry
        /// </summary>
        /// <paramname="target">Target string</param>
        /// <paramname="toBeReplaced">Strings to be replaced</param>
        /// <paramname="toReplace">Strings to replace with</param>
        /// <returns>Modified string</returns>
        /// <exceptioncref="ArgumentNullException"></exception>
        /// <exceptioncref="ArgumentException"></exception>
		public static string ReplaceAllRange(this string target, string[] toBeReplaced, string[] toReplace)
		{
			if (target is null)
				throw new Exception(Translate.DoTranslation("The target may not be null"));
			if (toBeReplaced is null || toBeReplaced.Length == 0)
				throw new Exception(Translate.DoTranslation("Array of to be replaced strings may not be null"));
			if (toReplace is null || toReplace.Length == 0)
				throw new Exception(Translate.DoTranslation("Array of to be replacement strings may not be null"));
			if (toBeReplaced.Length != toReplace.Length)
				throw new Exception(Translate.DoTranslation("Array length of which strings to be replaced doesn't equal the array length of which strings to replace."));

			int i = 0;
			int loopTo = toBeReplaced.Length - 1;

			while (i <= loopTo)
			{
				target = target.Replace(toBeReplaced[i], toReplace[i]);
				i += 1;
			}
			return target;
		}

		/// <summary>
        /// Replaces last occurrence of a text in source string with the replacement
        /// </summary>
        /// <paramname="source">A string which has the specified text to replace</param>
        /// <paramname="searchText">A string to be replaced</param>
        /// <paramname="replace">A string to replace</param>
        /// <returns>String that has its last occurrence of text replaced</returns>
		public static string ReplaceLastOccurrence(this string source, string searchText, string replace)
		{
			if (source is null)
				throw new Exception(Translate.DoTranslation("The source may not be null"));
			if (searchText is null)
				throw new Exception(Translate.DoTranslation("The search text may not be null"));

			int position = source.LastIndexOf(searchText);
			if (position == -1)
				return source;
			string result = source.Remove(position, searchText.Length).Insert(position, replace);
			return result;
		}

		/// <summary>
        /// Get all indexes of a value in string
        /// </summary>
        /// <paramname="target">Source string</param>
        /// <paramname="value">A value</param>
        /// <returns>Indexes of strings</returns>
		public static IEnumerable<int> AllIndexesOf(this string target, string value)
		{
			if (target is null)
				throw new Exception(Translate.DoTranslation("The target may not be null"));
			if (string.IsNullOrEmpty(value))
				throw new Exception(Translate.DoTranslation("Empty string value specified"));

			int index = 0;
			while (true)
			{
				index = target.IndexOf(value, index);
				if (index == -1)
					break;
				yield return index;
				index += value.Length;
			}
		}

		/// <summary>
        /// Replaces last occurrence of a text in source string with the replacement
        /// </summary>
        /// <paramname="source">A string which has the specified text to replace</param>
        /// <paramname="searchText">A string to be replaced</param>
        /// <paramname="replace">A string to replace</param>
        /// <returns>String that has its last occurrence of text replaced</returns>
		public static string Repeat(this string source, int times)
		{
			string result = "";
			for (int time = 1, loopTo = times; time <= loopTo; time++)
				result += source;
			return result;
		}

		/// <summary>
        /// Formats the string
        /// </summary>
        /// <paramname="Format">The string to format</param>
        /// <paramname="Vars">The variables used</param>
        /// <returns>A formatted string if successful, or the unformatted one if failed.</returns>
		public static string FormatString(this string Format, params object[] Vars)
		{
			if (Format is null)
				throw new Exception(Translate.DoTranslation("The target format may not be null"));

			string FormattedString = Format;
			try
			{
				if (Vars.Length > 0)
					FormattedString = string.Format(Format, Vars);
			}
			catch (Exception ex)
			{
				DebugWriter.Wdbg(DebugLevel.E, "Failed to format string: {0}", ex.Message);
				DebugWriter.WStkTrc(ex);
			}
			return FormattedString;
		}

		/// <summary>
        /// Is the string numeric?
        /// </summary>
        /// <paramname="Expression">The expression</param>
		public static bool IsStringNumeric(string Expression)
		{
			if (Expression is null)
				throw new Exception(Translate.DoTranslation("The target expression may not be null"));

			double __ = (double)default;
			return double.TryParse(Expression, out __);
		}
	}
}