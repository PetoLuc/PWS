﻿using Microsoft.AspNetCore.Mvc;
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
using System.Collections.Concurrent;

namespace MyPWS.API.Controllers
{
    public class CacheWeatherLogic
    {
        protected readonly pwsstoreContext _context;
        private IMemoryCache _memCache;
        private IServiceScopeFactory _serviceFactory;

        public CacheWeatherLogic(IMemoryCache memoryCache, pwsstoreContext context, IServiceScopeFactory serviceFactory)
        {
            _memCache = memoryCache;
            _context = context;
            _serviceFactory = serviceFactory;
        }

        protected async Task storeCachedWeatherData(int IdPws, IEnumerable<Weather> lstWeather)
        {                        
            var insWeather = lstWeather.GroupBy(w => w.Dateutc.DayOfYear).GetAverage(windgustMax: true);            
            using (var scope = _serviceFactory.CreateScope())
            {
                pwsstoreContext context = scope.ServiceProvider.GetRequiredService<pwsstoreContext>();
                await context.Weather.AddRangeAsync(insWeather);
                await context.SaveChangesAsync();
            }            
           
        }



        /// <summary>
        /// chache station DB key and last upload request
        /// </summary>        
        /// <returns></returns>
        public async Task<Cache.CacheWeather> GetPWSAsync(string id, string pwd)
        {
            string key = CacheKeys.PWS + id;
			Cache.CacheWeather cacheWeather = null;
            if (!_memCache.TryGetValue(key, out cacheWeather))
            {
                //not in cache                
                //TODO on change password cache must by reseted
                int? IdPws = await _context.Pws.Where(pws => pws.Id == id && pws.Pwd == pwd).Select(pws => pws.IdPws).FirstOrDefaultAsync();
                if (IdPws.HasValue && IdPws > 0)
                {
                    _memCache.Set(key, cacheWeather = new CacheWeather() { IdPws = IdPws.Value, lastWeatherSet = new ConcurrentQueue<Weather>()},
                        new MemoryCacheEntryOptions().RegisterPostEvictionCallback(async (object key, object value, EvictionReason reason, object state) =>
                        {
                            CacheWeather cacheWeather = (CacheWeather)value;                            
                            await Task.Run(async () => { await storeCachedWeatherData(cacheWeather.IdPws, cacheWeather.lastWeatherSet); });
                            cacheWeather.lastWeatherSet.Clear();
                        }, this).SetAbsoluteExpiration(Constants.weatherPostCacheTimeout));
                }
            }
            return cacheWeather;
        }
	}
}
