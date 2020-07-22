using MyPWS.API.Models;
using MyPWS.API.Models.dto;
using MyPWS.Models.pwsstore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPWS.API.Cache
{
	/// <summary>
	/// For API post method caching last request and PWS entity
	/// </summary>
	public class CacheWeather
	{

		public int IdPws { get; set; }
		public ConcurrentQueue<Weather> lastWeatherSet { get; set; }
	}
}
