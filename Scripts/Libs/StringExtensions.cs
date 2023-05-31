﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Libs
{
	public static class StringExtensions
	{
		/// <summary>
		/// Converts string to enum object
		/// </summary>
		/// <typeparam name="T">Type of enum</typeparam>
		/// <param name="value">String value to convert</param>
		/// <returns>Returns enum object</returns>
		public static T ToEnum<T>(this string value)
			where T : struct
		{
			return (T)System.Enum.Parse(typeof(T), value, true);
		}


		/// <summary>
		/// Determines whether the specified string contains all the specified values.
		/// </summary>
		/// <param name="value">The string to search.</param>
		/// <param name="values">The values to search for.</param>
		/// <returns>true if all values are found in the string; otherwise, false.</returns>
		public static bool ContainsAll(this string value, params string[] values)
		{
			foreach (string one in values)
			{
				if (!value.Contains(one))
				{
					return false;
				}
			}
			return true;
		}


		/// <summary>
		/// Determines whether the specified string contains any of the specified values.
		/// </summary>
		/// <param name="value">The string to search.</param>
		/// <param name="values">The values to search for.</param>
		/// <returns>true if any value are found in the string; otherwise, false.</returns>
		public static bool ContainsAny(this string value, params string[] values)
		{
			foreach (string one in values)
			{
				if (value.Contains(one))
				{
					return true;
				}
			}
			return false;
		}



		/// <summary>
		///     Formats a large number to 5 digits (2 decimal places) with a suffix. (e.g. 123,456,789,123 (123 Billion) ->
		///     123.45B)
		/// </summary>
		/// <param name="numberToFormat"> The number to format. </param>
		/// <param name="decimalPlaces"> The number of decimal places to include - <i> defaults to <c> 2 </c> </i> </param>
		/// <returns> A <see cref="string" />. </returns>
		public static string FormatNumber(this long numberToFormat, int decimalPlaces = 2)
		{
			// Get the default string representation of the numberToFormat.
			string numberString = numberToFormat.ToString();

			foreach (NumberSuffix suffix in Enum.GetValues<NumberSuffix>())
			{
				// Assign the amount of digits to base 10.
				double currentValue = 1 * Math.Pow(10, (int)suffix * 3);

				// Get the suffix value.
				string? suffixValue = Enum.GetName(typeof(NumberSuffix), (int)suffix);

				// If the suffix is the placeholder, set it to an empty string.
				if ((int)suffix == 0) { suffixValue = string.Empty; }

				// Set the return value to a rounded value with the suffix.
				if (numberToFormat >= currentValue)
				{
					numberString = $"{Math.Round(numberToFormat / currentValue, decimalPlaces, MidpointRounding.ToEven)}{suffixValue}";
				}
			}

			return numberString;
		}

		/// <summary> Suffixes for numbers based on how many digits they have left of the decimal point. </summary>
		/// <remarks> The order of the suffixes matters! </remarks>
		private enum NumberSuffix
		{
			/// <summary> A placeholder if the value is under 1 thousand </summary>
			P,
			/// <summary> Thousand </summary>
			K,
			/// <summary> Million </summary>
			M,
			/// <summary> Billion </summary>
			B,
			/// <summary> Trillion </summary>
			T,
			/// <summary> Quadrillion </summary>
			Q
		}
	}
}