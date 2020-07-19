using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace MyPWS.Models.extensions
{
	public static class NumericExtensions
	{
		public static double ConvertToRadians(this short angle)
		{
			return (Math.PI / 180) * angle;
		}

		public static double ConvertToDegress(this double angle)
		{
			return (180 / Math.PI) * angle;
		}


		public static bool IsWithin(this decimal? value, string valueName,  Constants.range range, ref List<string> rangeError)
		{
			if (value == null) return true;

			bool inRange = range.MinValue <= value.Value && value.Value <= range.MaxValue;
			if (!inRange)
			{
				rangeError?.Add(string.Format(Constants.OutOfRangePattern, valueName, range));
			}		
			return inRange;
		}

		/// <summary>
		/// convert decimal number to string with 2 decima places precision
		/// </summary>
		/// <param name="value"></param>
		/// <param name="unit"></param>
		/// <returns></returns>
		public static string ToTwoDecimalPrecision(this decimal? value, string unit)
		{
			return value.HasValue ? $"{Math.Round(value.Value, 2).ToString("0.##")} {unit}"  : string.Empty;
		}

		//public static bool IsWithin(this short? value, Constants.range range)
		//{
		//	if (value == null) return true;

		//	return range.MinValue <= value.Value && value.Value <= range.MaxValue;
		//}
	}
}
