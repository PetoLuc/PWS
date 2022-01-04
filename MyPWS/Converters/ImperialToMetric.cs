using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyPWS.API.Converters
{
	/// <summary>
	/// convert imperial system to metric system
	/// </summary>
	public static class ImperialToMetric
	{
        /// <summary>
        /// convert inches of mercury to hectopascals conversion 
        //Pressure(hPa) = Pressure(inHg) × 33.86389
        /// </summary>
        /// <param name="mectInch"></param>
        /// <returns></returns>        
        public static decimal? MercInchToHpa(decimal? mectInch)
        {
            if (!mectInch.HasValue) return null;

            return   mectInch.Value * (decimal)33.86389;
        }

        /// <summary>
        /// convert inches to milimeters 
        /// 1 inches to mm = 25.4 mm
        /// </summary>
        /// <param name="inches"></param>
        /// <returns></returns>
        public static decimal? InchesToMilimeters(decimal? inches)
        {
            if (!inches.HasValue) return null;
            return inches.Value * (decimal)25.4;
        }

        /// <summary>
        /// convert farenheit to celsius
        /// </summary>
        /// <param name="farenheit"></param>
        /// <returns></returns>
        public static decimal? FarenheitToCelsius(decimal? farenheit)
        {
            if (!farenheit.HasValue) return null;
            return (farenheit.Value - 32) * 5 / 9;
        }

        /// <summary>
        /// convert miles per hour to kilometers to hour
        /// </summary>
        /// <param name="mph"></param>
        /// <returns></returns>
        public static decimal? MphToKmh(decimal? mph)
        {
            if (!mph.HasValue) return null;
            return mph.Value * (decimal)1.60934;
        }
    }
}
