
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using MyPWS.API.Controllers;
using MyPWS.Models.pwsstore;
using System;
using System.Collections.Generic;
using System.Linq;
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

				weatherController wc = new weatherController(sProvider.GetService<IMemoryCache>(), sProvider.GetService<pwsstoreContext>());
			    var xxx = wc.GetWeather("a", "b");
		}


	
	}
}
