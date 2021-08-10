using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Threading.Tasks;
using WebApplicationEF.Contexts;
using WebApplicationEF.Middlewares;

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

      //  Adds a middleware delegate defined in-line to the application's request pipeline.
      app.Use((httpContext, next) =>
      {
        if (httpContext.Request.Path.StartsWithSegments("/diagnostics"))
        {
          httpContext.Response.StatusCode = 200;
          return httpContext.Response.WriteAsync("{\"message\": \"Ok\"}");
        }
        return next();
      });

      app.UseMiddleware<HealthMiddleware>();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
