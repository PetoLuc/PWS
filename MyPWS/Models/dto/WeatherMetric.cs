using Google.Protobuf.WellKnownTypes;
using System;

namespace MyPWS.API.Models.dto
{
	public class WeatherMetric :  WeatherBase//, IEquatable<WeatherMetric>
	{
		
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
		

		//public bool Equals(WeatherMetric other)
		//{
		//	if (other == null) return false;
		//	if (this.Baromhpa != other.Baromhpa ||
		//		this.Dailyrainmm != other.Dailyrainmm ||
		//		this.Dewptc != other.Dewptc ||
		//		this.Humidity != other.Humidity ||
		//		this.Indoorhumidity != other.Indoorhumidity ||
		//		this.Indoortempc != other.Indoortempc ||
		//		this.Rainmm!= other.Rainmm ||
		//		this.Tempc != other.Tempc ||
		//		this.Uv != other.Uv ||
		//		this.Winddir != other.Winddir ||
		//		this.Windgustdir != other.Windgustdir ||
		//		this.Windgustkmh != other.Windgustkmh ||
		//		this.Windspeedkmh != other.Windspeedkmh
		//		)
		//	{
		//		return false;
		//	}
		//	return true;
		//}
	}
}
