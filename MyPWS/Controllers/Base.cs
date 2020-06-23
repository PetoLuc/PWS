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
    public class Base : ControllerBase
    {
        protected readonly pwsstoreContext _context;
        private IMemoryCache _memCache;
        private IServiceScopeFactory _serviceFactory;

        public Base(IMemoryCache memoryCache, pwsstoreContext context, IServiceScopeFactory serviceFactory)
        {
            _memCache = memoryCache;
            _context = context;
            _serviceFactory = serviceFactory;
        }

        private async void cacheEvicted(object key, object value, EvictionReason reason, object state)
        {            
            CacheWeather cacheWeather = (CacheWeather)value;            
            Weather[] arrWeather = new Weather[cacheWeather.lastWeatherSet.Count];
             cacheWeather.lastWeatherSet.CopyTo(arrWeather); //skopirujeme do pola
            await Task.Factory.StartNew(async() =>{ await average(cacheWeather, arrWeather); });
        }

        protected async Task average(CacheWeather cacheWeather, Weather[] lstWeather)
        {
            if (lstWeather == null || lstWeather.Length == 0)
                return;
            var insWeather = lstWeather.GroupBy(w => w.Dateutc.DayOfYear).GetAverage(windgustMax: true).ToList();
            insWeather.ForEach(w => w.IdPws = cacheWeather.IdPws);               
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
		protected async Task<CacheWeather> getPWS(string id, string pwd)
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
                        new MemoryCacheEntryOptions().RegisterPostEvictionCallback(cacheEvicted).SetAbsoluteExpiration(TimeSpan.FromSeconds(Constants.PWSTimeout)));
                }
            }
            return cacheWeather;
        }
    }
}
