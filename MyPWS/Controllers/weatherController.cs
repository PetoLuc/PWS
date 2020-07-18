﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using MyPWS.API.Cache;
using MyPWS.API.Models.dto;
using MyPWS.API.Models.extensions;
using MyPWS.Models.pwsstore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyPWS.API.Controllers
{
	[Produces("application/json")]
	[Route("pws")]    
    [ApiController]
    public class weatherController : internalCacheController
    {
		public weatherController(IMemoryCache memoryCache, pwsstoreContext context, IServiceScopeFactory serviceFactory) : base(memoryCache, context, serviceFactory)
		{
		}

		/*https://stackoverflow.com/questions/36280947/how-to-pass-multiple-parameters-to-a-get-method-in-asp-net-core*/

		/// <summary>
		/// returns last stored pws data
		/// </summary>
		/// <remarks>
		/// Sample request:/api/pws/weather/yourStationID/yourStationPassword
		///
		/// </remarks>
		/// <param name="id"></param>
		/// <param name="pwd"></param>
		/// <returns></returns>
		/// <response code = "404">PWS not found by ID and PASSWORD, or found any data to response</response>
		/// <response code = "200">last stored PWS data</response>
		[HttpGet("{id}/{pwd}/[controller]")]
		public async Task<ActionResult<WeatherMetric>> GetWeather(string id, string pwd)
		{
			CacheWeather cachedWeather = await getCachePws(id, pwd);
			//no PWS foud by pwsData.PWSId
			if (cachedWeather == null)
			{
				return NotFound(Constants.NoPWS);
			}
			Weather lastWeather = cachedWeather.lastWeatherSet.LastOrDefault();
			if (lastWeather == null)
			{
				lastWeather = await _context.Weather.Where(p => p.IdPws == cachedWeather.IdPws).OrderByDescending(p => p.Id).FirstOrDefaultAsync<Weather>();
			}
			if (lastWeather == null)
			{
				return NotFound(Constants.NoPWSData);
			}
			return lastWeather.ToWeatherMetric();
		}


		//https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio	
		/// <summary>
		/// add weather record for pws
		/// </summary>
		/// /// <remarks>
		/// Sample request:
		///
		///     POST
		///     { 		
		///             "Dateutc": "now",
		///             "Baromin": 1,
		///             "Dailyrainin": 1,
		///             "Rainin": 2,
		///             "Dewptf": 3,
		///             "Humidity": 10,
		///             "Tempf": 1.2558,
		///             "Uv": 10,    
		///             "Winddir": 25,
		///             "Windspeedmph": 1,
		///             "Windgustmph": 10,		
		///             "Windgustdir": 10,
		///             "Indoortempf": 10,
		///             "Indoorhumidity": 100    
		///         }
		///
		/// </remarks>
		/// <param name="weatherImperial">measured weather data</param>
		/// <returns>code 200, if data stored sucesfully </returns>
		/// <response code = "401">PWS not found by ID and PASSWORD</response>
		/// <response code = "200">data sucesfully stored</response>
		/// <response code="400">If the item is null</response>   
		[HttpPost("{id}/{pwd}/[controller]")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> PostWeather(string id, string pwd, [FromBody] WeatherImperial weatherImperial)
        {
			DateTime now = DateTime.UtcNow;
            //get pws and they last requet from cache - chache timeout in Constants.PWSTimeout, after this period is chache refreshed, and same request can by writed 
            CacheWeather cachedWeather = await getCachePws(id, pwd);			

            //no PWS foud by pwsData.PWSId
            if (cachedWeather == null)
            {
                return Unauthorized(Constants.NoPWS);
            }
			Weather lastWeather =  cachedWeather.lastWeatherSet.LastOrDefault();			
			if (lastWeather != null )
			{
				TimeSpan tmLastUpdate = now - lastWeather.Dateutc;
				//the shortest update interval is owerflow
				if (tmLastUpdate < Constants.ShortestUploadInterval)
				{
					Thread.Sleep(Constants.ShortestUploadInterval - tmLastUpdate);
					//await Task.Delay(Constants.ShortestUploadInterval - tmLastUpdate);
				}				
			}
			string checkRangeError = string.Empty;
			if (!weatherImperial.CheckRange(ref checkRangeError))
			{
				return ValidationProblem(checkRangeError);
			}
			Weather weather = weatherImperial.ToWeather();
			weather.IdPws =  cachedWeather.IdPws;
			cachedWeather.lastWeatherSet.Add(weather);			
            return Ok();            
        }       
	}
}
