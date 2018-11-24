using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyJuncAPI.Models;
using MyJuncAPI.Services;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace MyJuncAPI
{
	public class Startup
	{
		public static ImmutableList<Event> EventsList = ImmutableList<Event>.Empty;

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1.0.0/swagger.json", "REST API");
			});

			app.UseCors(builder =>
				   builder.AllowAnyHeader()
				   .AllowAnyMethod()
				   .AllowAnyOrigin());

			app.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();
			LoadEventsData();
			services.AddTransient<QueryService>();
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1.0.0", new Info { Title = "REST API", Version = "v1.0.0" });
			});
			services.AddCors();
		}

		private void LoadEventsData()
		{
			string jsonEvents = File.ReadAllText(@"DataFiles/open-api-events.json");
			EventsList = JsonConvert.DeserializeObject<ImmutableList<Event>>(jsonEvents);
		}
	}
}
