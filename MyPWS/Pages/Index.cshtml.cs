using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyPWS.API.Controllers;
using MyPWS.API.Models.dto;
using MyPWS.Models.pwsstore;

namespace RazorWebApplication.Pages
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly IMemoryCache _memoryCache;
		private readonly pwsstoreContext _context;
		private readonly IServiceScopeFactory _serviceScopeFactory;		

		public Dictionary<Pws, WeatherMetric> Weather { get; private set; }

		public IndexModel(ILogger<IndexModel> logger, IMemoryCache memoryCache, pwsstoreContext context, IServiceScopeFactory serviceFactory)
		{
			_logger = logger;
			_memoryCache = memoryCache;
			_context = context;
			_serviceScopeFactory = serviceFactory;
			
		}

		public async void OnGetAsync()
		{
			Weather = new Dictionary<Pws, WeatherMetric>();
			var pwss = _context.Pws.ToList();
			using (var weatherController = new weatherController(_memoryCache, _context, _serviceScopeFactory))
			{
				foreach (var pws in pwss)
				{
					var res = await weatherController.GetWeather(pws.Id, pws.Pwd);
					Weather.Add(pws, res.Value);
				}
			}
		}
	}
}
