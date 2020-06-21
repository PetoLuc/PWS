using MyPWS.API.Models;
using MyPWS.API.Models.dto;
using MyPWS.Models.pwsstore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPWS.API.Cache
{
	/// <summary>
	/// For API post method caching last request and PWS entity
	/// </summary>
	public class CachePwsWeather
	{

		public int IdPws { get; set; }
		public List<Weather> lastWeatherSet { get; set; }
	}
}
