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
	}
}
