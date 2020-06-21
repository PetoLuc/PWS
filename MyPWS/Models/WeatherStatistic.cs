using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPWS.Models
{
	public struct WeatherStatistic
	{
		/// <summary>
		/// % outdoor humidity 0-100%
		/// </summary>
		public long? Humidity { get; set; }

		/// <summary>
		/// index
		/// </summary>
		public decimal? Uv { get; set; }

		/// <summary>
		/// 0-360 instantaneous wind direction]
		/// </summary>
		public long? Winddir { get; set; }

		/// <summary>
		/// 0-360 using software specific time period
		/// </summary>
		public long? Windgustdir { get; set; }

		/// <summary>
		/// % indoor humidity 0-100
		/// </summary>
		public long? Indoorhumidity { get; set; }

		/// <summary>
		/// - barometric pressure hectopascals
		/// </summary>
		public decimal? Baromhpa { get; set; }

		/// <summary>
		/// rain milimeters so far today in local time
		/// </summary>
		public decimal? Dailyrainmm { get; set; }

		/// <summary>
		/// rain milimeters over the past hour -- the accumulated rainfall in the past 60 min
		/// </summary>
		public decimal? Rainmm { get; set; }

		/// <summary>
		/// F outdoor dewpoint C
		/// </summary>
		public decimal? Dewptc { get; set; }


		/// <summary>
		/// C outdoor temperature
		/// </summary>
		public decimal? Tempc { get; set; }


		/// <summary>
		/// kmh instantaneous wind speed
		/// </summary>
		public decimal? Windspeedkmh { get; set; }

		/// <summary>
		/// kmh current wind gust
		/// </summary>
		public decimal? Windgustkmh { get; set; }



		/// <summary>
		///  indoor temperature C
		/// </summary>
		public decimal? Indoortempc { get; set; }

	}
}
