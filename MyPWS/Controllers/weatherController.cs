using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyPWS.API.Cache;
using MyPWS.API.Models.dto;
using MyPWS.API.Models.extensions;
using MyPWS.Models.pwsstore;
using System.Linq;
using System.Threading.Tasks;

namespace MyPWS.API.Controllers
{
	[Produces("application/json")]
	[Route("pws")]    
    [ApiController]
    public class weatherController : Base
    {
		public weatherController(IMemoryCache memoryCache, pwsstoreContext context) : base(memoryCache, context)
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

			CachePwsWeather cachedWeather = await cacheFindPWS(id, pwd);
			//no PWS foud by pwsData.PWSId
			if (cachedWeather == null)
			{
				return NotFound(Constants.NoPWS);
			}
			int IdPws = cachedWeather.IdPws;			
			Weather weather = await _context.Weather.Where(p => p.IdPws == IdPws).OrderByDescending(p => p.Id).FirstOrDefaultAsync<Weather>();
			if (weather == null)
			{
				return NotFound(Constants.NoPWSData);
			}
			return weather.ToWeatherMetric();
			
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
        public async Task<IActionResult> PostPwsUpoad(string id, string pwd, [FromBody] WeatherImperial weatherImperial)
        {            

            //get pws and they last requet from cache - chache timeout in Constants.PWSTimeout, after this period is chache refreshed, and same request can by writed 
            CachePwsWeather cachedWeather = await cacheFindPWS(id, pwd);
            //no PWS foud by pwsData.PWSId
            if (cachedWeather == null)
            {
                return Unauthorized(Constants.NoPWS);
            }
            if (!weatherImperial.Equals(cachedWeather.lastPwsWeather))
            {
                
                Weather addWeather = weatherImperial.ToWeather();
                //set found / cached id 

				//when wind dust speed is 0 then, not store, wind wane direction is from last wind direction 
                addWeather.IdPws = cachedWeather.IdPws;
				if ((addWeather.Windgustkmh ?? 0) == 0)
				{
					addWeather.Windgustdir = null;
				}

				//when wind  speed is 0 then, not store, wind wane direction is from last wind direction 
				if ((addWeather.Windspeedkmh ?? 0) == 0)
				{
					addWeather.Winddir = null;
			}		

                //insert 
                _context.Weather.Add(addWeather);
                await _context.SaveChangesAsync();
				cachedWeather.lastPwsWeather = weatherImperial;


			}
            return Ok();            
        }
       
	}
}


