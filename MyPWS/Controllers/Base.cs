using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using MyPWS.API.Cache;
using MyPWS.Models.pwsstore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using MyPWS.Models.extensions;
using MyPWS.API.Models.dto;
using MyPWS.API.Models.extensions;

namespace MyPWS.API.Controllers
{
    public class Base : ControllerBase
    {
        protected readonly pwsstoreContext _context;
        private IMemoryCache _memCache;        
        

        public Base(IMemoryCache memoryCache, pwsstoreContext context)
        {
            _memCache = memoryCache;
            _context = context;            
        }

        private void cacheEvicted(object key, object value, EvictionReason reason, object state)
        {
            if (value == null)
                return;
            CachePwsWeather cacheWeather = (CachePwsWeather)value;
            List<Weather> lstWeather = cacheWeather.lastWeatherSet;

            if (lstWeather ==null || lstWeather.Count == 0)
                return;
         
            foreach (var weatherByHour in lstWeather.GroupBy(w => w.Dateutc.Hour))
            {
                Weather insWeather = new Weather()
                {
                    //rount safe
                    Baromhpa = lstWeather.Average(w => w.Baromhpa),
                    Dailyrainmm = lstWeather.Max(w => w.Dailyrainmm),
                    Dateutc = lstWeather.Max(w => w.Dateutc),
                    Dewptc = lstWeather.Average(w => w.Dewptc),
                    Humidity = (short?)lstWeather.Average(w => w.Humidity),
                    Indoorhumidity = (short?)lstWeather.Average(w => w.Indoorhumidity),
                    Indoortempc = lstWeather.Average(w => w.Indoortempc),
                    Rainmm = lstWeather.Max(w => w.Rainmm),
                    Tempc = lstWeather.Average(w => w.Tempc),
                    Uv = lstWeather.Average(w => w.Uv),
                    Windgustkmh = lstWeather.Max(w => w.Windgustkmh),                    
                };
                (insWeather.Windspeedkmh, insWeather.Winddir) = lstWeather.vectorAvg(w => w.Windspeedkmh, w => w.Winddir);                
            }                                
            //insert 
            //_context.Weather.Add(addWeathe6r);
            //await _context.SaveChangesAsync();
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
                    /*RegisterPostEvictionCallback(
            (key, value, reason, substate) =>
            {
                //tu spocitanie priemeru / maxima a ulozenie

                var _result = $"'{key}':'{value}' was evicted because: {reason}";


            })*/

                    _memCache.Set(key, weather = new CachePwsWeather() { IdPws = IdPws.Value, lastWeatherSet = new List<Weather>() }, new MemoryCacheEntryOptions().RegisterPostEvictionCallback(cacheEvicted).SetAbsoluteExpiration(TimeSpan.FromSeconds(10)));                 }
            }
            return weather;
        }
    }
}
