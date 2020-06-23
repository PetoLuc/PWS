using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MyPWS.Models.pwsstore;
using System;
using System.IO;

namespace MyPws
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			//"server=c072um.forpsi.com;port=3306;user=f134044;password=bfTfSgsp;database=f134044"
			services.AddDbContext<pwsstoreContext>(options => options.UseMySQL(Configuration.GetConnectionString("PWSStore")),ServiceLifetime.Scoped);						
			services.AddControllers();
			services.AddMemoryCache();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "MyPWS API",
					Description = "Personal weather station data store API as private alternative to WU, ....",
					//TermsOfService = new Uri("https://example.com/terms"),
					Contact = new OpenApiContact
					{
						Name = "Peter Lucansky",
						Email = "plucansky@gmail.com",
						Url = new Uri("https://github.com/PetoLuc/MyPWS"),
					},
					//License = new OpenApiLicense
					//{
					//	//Name = "Use under LICX",
					//	//Url = new Uri("https://example.com/license"),
					//}				
				});
				//var filePath = Path.Combine(System.AppContext.BaseDirectory, "MyPWS.xml");
				//c.IncludeXmlComments(filePath);
			});			
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{				
			app.UseSwagger();
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyPWS API V1");
			});

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
			
		}
	}
}
