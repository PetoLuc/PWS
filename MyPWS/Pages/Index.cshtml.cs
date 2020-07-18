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
		private static weatherController weatherController;

		public WeatherMetric Weather { get; private set; }

		public IndexModel(ILogger<IndexModel> logger, IMemoryCache memoryCache, pwsstoreContext context, IServiceScopeFactory serviceFactory)
		{
			_logger = logger;
			_memoryCache = memoryCache;
			_context = context;
			_serviceScopeFactory = serviceFactory;
			weatherController = new weatherController(_memoryCache, _context, _serviceScopeFactory);
		}

		public async void OnGetAsync()
		{
			var res = await weatherController.GetWeather("IDETVA4", "nemam");
			Weather = res.Value;
		}
	}
}
