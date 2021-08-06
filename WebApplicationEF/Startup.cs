using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplicationEF
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

      services.AddControllers();

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApplicationEF", Version = "v1" });
      });
      services.AddDbContext<WebEFDbContext>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseHttpsRedirection();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplicationEF v1"));
      }

      app.Use((httpContext, t) =>
      {
        if (httpContext.Request.Path.StartsWithSegments("/use-health"))
        {
          httpContext.Response.StatusCode = 200;
          return httpContext.Response.WriteAsync("OK");
        }
        return t();
      });

      app.UseMiddleware<MyMiddleware>();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }

  public class MyMiddleware
  {
    private RequestDelegate Next { get; }
    public MyMiddleware(RequestDelegate nextMiddleware)
    {
      Next = nextMiddleware;
    }
    public async Task Invoke(HttpContext httpContext)
    {
      if (httpContext.Request.Path.StartsWithSegments("/health"))
      {
        httpContext.Response.StatusCode = 200;
        await httpContext.Response.WriteAsync("OK");
      }
      await Next(httpContext);
    }

  }
}
