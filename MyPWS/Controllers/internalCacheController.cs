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
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.Contracts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Hosting;
using System.Threading;

namespace MyPWS.API.Controllers
{
    public class internalCacheController : ControllerBase
    {
        protected readonly PwsStoreContext _context;
        private IMemoryCache _memCache;
        private IServiceScopeFactory _serviceFactory;

        protected internalCacheController(IMemoryCache memoryCache, PwsStoreContext context, IServiceScopeFactory serviceFactory)
        {
            _memCache = memoryCache;
            _context = context;
            _serviceFactory = serviceFactory;
        }


        protected async Task storeCachedWeatherData(int IdPws, IEnumerable<Weather> lstWeather)
        {                        
            var insWeather = lstWeather.GroupBy(w => w.Dateutc.DayOfYear).GetAverage(windgustMax: true).ToList();
            insWeather.ForEach(w => w.IdPws = IdPws);
            using (var scope = _serviceFactory.CreateScope())
            {
                PwsStoreContext context = scope.ServiceProvider.GetRequiredService<PwsStoreContext>();
                await context.Weather.AddRangeAsync(insWeather);
                await context.SaveChangesAsync();
            }
        }



        /// <summary>
        /// chache station DB key and last upload request
        /// </summary>        
        /// <returns></returns>
        protected async Task<CacheWeather> getCachePws(string id, string pwd)
        {
            string key = CacheKeys.PWS + id;
            if (!_memCache.TryGetValue(key, out CacheWeather cacheWeather))
            {
                //not in cache                
                //TODO on change password cache must by reseted
                int? IdPws = await _context.Pws.Where(pws => pws.Id == id && pws.Pwd == pwd).Select(pws => pws.IdPws).FirstOrDefaultAsync();
                if (IdPws.HasValue && IdPws > 0)
                {
                    _memCache.Set(key, cacheWeather = new CacheWeather() { IdPws = IdPws.Value, lastWeatherSet = new List<Weather>() },
                        new MemoryCacheEntryOptions().RegisterPostEvictionCallback(async (object key, object value, EvictionReason reason, object state) =>
                        {
                            CacheWeather cacheWeather = (CacheWeather)value;                            
                            await Task.Run(async () => { await storeCachedWeatherData(cacheWeather.IdPws, cacheWeather.lastWeatherSet); });
                        }, this).SetAbsoluteExpiration(Constants.weatherPostCacheTimeout));
                }
            }
            return cacheWeather;
        }

        protected DateTime GetDateTimeStamp()
        {
            throw new NotImplementedException();
             //todo zober z loggera
        }

    }
}
