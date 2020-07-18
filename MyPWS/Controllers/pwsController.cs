using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyPWS.API.Cache;
using MyPWS.API.Models.dto;
using MyPWS.Models.pwsstore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyPWS.API.Controllers
{
	/// <summary>
	/// 
	/// </summary>
	[Route("[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class pwsController : internalCacheController
    {
        Microsoft.Extensions.Configuration.IConfiguration _configuration;
        public pwsController(IMemoryCache memoryCache, pwsstoreContext context, IServiceScopeFactory serviceFactory, Microsoft.Extensions.Configuration.IConfiguration configuration) : base(memoryCache, context, serviceFactory)
        {
            _configuration = configuration;
        }

        // GET: api/Pws
        /// <summary>
        /// return registred pws list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PWSList>>> GetPws()
        {
            var pwsList = await (from p in _context.Pws select new PWSList { Id = p.Id, Alt = p.Alt, Desc = p.Desc, Lat = p.Lat, Lon = p.Lon, Name = p.Name }).ToListAsync();
            if (pwsList == null || pwsList.Count == 0)
            {
                return NotFound(Constants.NoPWS);
            }
            return pwsList;
        }

        /// <summary>
        /// return datail of pws
        /// </summary>
        /// <param name="id">station id</param>
        /// <param name="pwd">station password</param>
        /// <returns></returns>
        // GET: api/Pws/yourstationid/yourstationpassword
        [HttpGet("{id}/{pwd}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PWSDetail>> GetPws(string id, string pwd)
        {
             CacheWeather cachePwsWeather =  await getCachePws(id, pwd);
            if (cachePwsWeather == null)
            {
                return NotFound(Constants.NoPWS);
            }
            PWSDetail pws = await (from p in _context.Pws where p.IdPws == cachePwsWeather.IdPws select new PWSDetail { Alt = p.Alt, Desc = p.Desc, Id = p.Id, Lat = p.Lat, Lon = p.Lon, Name = p.Name, Pwd = p.Pwd }).FirstOrDefaultAsync();

            if (pws == null)
            {
                return NotFound(Constants.NoPWS);
            }
            pws.WeatherRecordsCount = await _context.Weather.LongCountAsync(p => p.IdPws == cachePwsWeather.IdPws);
            pws.LastUpdateDateTime  = await (from w in _context.Weather where w.IdPws == cachePwsWeather.IdPws orderby w.Id descending select w.Dateutc).FirstOrDefaultAsync();// _context.Weather.Where(p => p.IdPws == cachePwsWeather.IdPws).OrderByDescending(p => p.Id).FirstOrDefaultAsync<Weather>();
            return pws;
        }

        /// <summary>
        /// add new pws
        /// </summary>
        /// <param name="pws"></param>
        /// <param name="secret"></param>
        /// <returns></returns>        
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]        
        public async Task<ActionResult<PWSDetail>> PostPws(PWSDetail pws, string secret)
        {

            if (secret != _configuration.GetValue<string>("PSWAddSecret"))
            {
                return Unauthorized();
            }
            var exists = await _context.Pws.FirstOrDefaultAsync(p => p.Id == pws.Id);
            if (exists != null)
            {
                return NotFound("can't add pws");
            }

            _context.Pws.Add(new Pws() {Alt = pws.Alt, Desc = pws.Desc, Id = pws.Id, Lat = pws.Lat, Lon  = pws.Lon, Name = pws.Name,  Pwd = pws.Pwd});
            await _context.SaveChangesAsync();

            return Created($"Pws/{pws.Id}/{pws.Pwd}", pws);
        }      
    }
}
