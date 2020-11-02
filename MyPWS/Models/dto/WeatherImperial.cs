using System;


namespace MyPWS.API.Models.dto
{
	/// <summary>
	/// weatherstation data, imperial values
	/// based on https://support.weather.com/s/article/PWS-Upload-Protocol?language=en_US	
	/// </summary>
	public partial class WeatherImperial : WeatherBase
	{				
		/// <summary>
		/// - barometric pressure inches
		/// </summary>
		public decimal? Baromin { get; set; }

		/// <summary>
		/// rain inches so far today in local time
		/// </summary>
		public decimal? Dailyrainin { get; set; }

		/// <summary>
		/// rain inches over the past hour -- the accumulated rainfall in the past 60 min
		/// </summary>
		public decimal? Rainin { get; set; }

		/// <summary>
		/// F outdoor dewpoint F
		/// </summary>
		public decimal? Dewptf { get; set; }
		

		/// <summary>
		/// F outdoor temperature
		/// </summary>
		public decimal? Tempf { get; set; }
		

		/// <summary>
		/// mph instantaneous wind speed
		/// </summary>
		public decimal? Windspeedmph { get; set; }

		/// <summary>
		/// mph current wind gust, using software specific time period
		/// </summary>
		public decimal? Windgustmph { get; set; }

		

		/// <summary>
		/// F indoor temperature F
		/// </summary>
		public decimal? Indoortempf { get; set; }
				
		
	}
}