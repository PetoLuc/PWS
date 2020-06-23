
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using MyPWS.API.Controllers;
using MyPWS.Models.pwsstore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Xunit;

namespace MyPWSTest
{
	public class UnitTest1
	{
		//https://docs.microsoft.com/en-us/aspnet/web-api/overview/testing-and-debugging/mocking-entity-framework-when-unit-testing-aspnet-web-api-2
		//https://docs.microsoft.com/en-us/ef/ef6/fundamentals/testing/mocking		
		//https://github.com/moq/moq4
		[Fact]
		public void Test1()
		{
			//Arrange
			ServiceCollection sc = new ServiceCollection();
			sc.AddDbContext<pwsstoreContext>(opt => opt.UseInMemoryDatabase("pwsStore"), ServiceLifetime.Singleton, ServiceLifetime.Singleton);
			var _cache = sc.AddMemoryCache();
			var sProvider =  sc.BuildServiceProvider();

			var _context = sProvider.GetService<pwsstoreContext>();
			Pws testPws = new Pws { Alt = 520, Desc = "in memory PWS test", Id = "pwsTest", IdPws = 1, Pwd = "pwd" };
			var rand = new Random();
			//for (int i = 0; i < 10000000; i++)
			//{
				
			//}

			
			_context.Pws.Add(new Pws { Alt = 520, Desc = "in memory PWS test", Id = "pwsTest", IdPws = 1, Pwd = "pwd" });
			_context.SaveChanges();

			//var service = new BlogService(mockContext.Object);
			//service.AddBlog("ADO.NET Blog", "http://blogs.msdn.com/adonet");

			//mockSetPWS.Verify(m => m.Add(It.IsAny<Blog>()), Times.Once());
			//mockContext.Verify(m => m.SaveChanges(), Times.Once());


			//var mockContext = new Mock<pwsstoreContext>();
			//mockContext.Setup(x => x.Pws.ToListAsync(x=).;
			//var res =  mockContext.Object.Pws.
			////mockConte                                                                                                                                                                                                                                                                                                                                                             xt.Setup(x => x.Set<Weather>());
			//mockContext.Object.Pws = mockPws.Object;
			//mockContext.Object.Weather = mockWeather.Object;
			//mockContext.Object.Pws.Add(new Pws { Id = "test station", IdPws = 1, Pwd = "pwd" } );
			//mockContext.Setup(c => c.Pws).Returns(new DbSet<Pws> { new Pws { Id = "test station", IdPws = 1, Pwd = "pwd" } });


			//mock.Setup(x => x.Set<Pws>()).Returns( } )

			//ID = IDETVA4 & PASSWORD = shvzwfGf & action = updateraww & realtime = 1 & rtfreq = 5 & dateutc = now & baromin = 30.00 & tempf = 56.1 & humidity = 64 & windspeedmph = 1.5 & windgustmph = 1.5 & winddir = 290 & dewptf = 44.0 & rainin = 0 & dailyrainin = 0.06 & UV = 2.1 & indoortempf = 71.2 & indoorhumidity = 62
			weatherController wc = new weatherController(sProvider.GetService<IMemoryCache>(), sProvider.GetService<pwsstoreContext>());
			for (int i = 0 ;  i < 100000000; i++ )
				{
				var xxx = wc.PostWeather(
					testPws.Id,
					testPws.Pwd,
					new MyPWS.API.Models.dto.WeatherImperial
					{
						Baromin = rand.Next(0, 1500),
						Dailyrainin = rand.Next(0, 999),
						Dateutc = DateTime.UtcNow.AddMinutes(i*10).ToString(),
						Dewptf = rand.Next(-100, 100),
						Humidity = (short)rand.Next(0, 100),
						Indoorhumidity = (short)rand.Next(0, 100),
						Indoortempf = rand.Next(-100, 100),
						Rainin = rand.Next(0, 999),
						Tempf = rand.Next(-100, 100),
						Uv = rand.Next(0, 30),
						Winddir = 50,
						Windgustdir = null,
						Windgustmph = 100 + i,
						Windspeedmph = 50 + i
					});
				//Thread.Sleep(10);
				if(i % 10000 == 1)
				System.Diagnostics.Debug.WriteLine(_context.Weather.Count());
			}
		}	
	}
}
