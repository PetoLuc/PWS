using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyPWS.API.Cache;
using MyPWS.Models.pwsstore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyPWS.API.Controllers
{
	public class Base: ControllerBase
    {
		protected readonly pwsstoreContext _context;
		private IMemoryCache _memCache;

		public Base(IMemoryCache memoryCache, pwsstoreContext context)
		{
			_memCache = memoryCache;
			_context = context;
		}

        /// <summary>
        /// chache station DB key and last upload request
        /// </summary>        
        /// <returns></returns>
        protected async Task<CachePwsWeather> cacheFindPWS(string id, string pwd)
        {
            string key = CacheKeys.PWS + id;

            if (!_memCache.TryGetValue(key, out CachePwsWeather weather))
            {
                //not in cache                
                //TODO on change password cache must by reseted
                int? IdPws = await _context.Pws.Where(pws => pws.Id == id && pws.Pwd == pwd).Select(pws => pws.IdPws).FirstOrDefaultAsync();
                if (IdPws.HasValue && IdPws > 0)
                {
                    //pws exists in db store, add to cache
                    _memCache.Set(key, weather = new CachePwsWeather() { IdPws = IdPws.Value, lastPwsWeather = null }, TimeSpan.FromSeconds(Constants.PWSTimeout));
                }
            }
            return weather;
        }
    }
}
