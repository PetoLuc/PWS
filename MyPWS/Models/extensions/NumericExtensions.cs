using System;
using System.Collections.Generic;

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

		//public static bool IsWithin(this short? value, Constants.range range)
		//{
		//	if (value == null) return true;

		//	return range.MinValue <= value.Value && value.Value <= range.MaxValue;
		//}
	}
}
