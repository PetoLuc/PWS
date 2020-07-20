using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyPWS.API.Models.dto
{
	public class WeatherBase
	{			
		/// <summary>
		/// % outdoor humidity 0-100%
		/// </summary>
		public short? Humidity { get; set; }

		/// <summary>
		/// index
		/// </summary>
		public decimal? Uv { get; set; }

		/// <summary>
		/// 0-360 instantaneous wind direction]
		/// </summary>
		public short? Winddir { get; set; }

		/// <summary>
		/// 0-360 using software specific time period
		/// </summary>
		public short? Windgustdir { get; set; }

		/// <summary>
		/// % indoor humidity 0-100
		/// </summary>
		public short? Indoorhumidity { get; set; }

	}
}
