using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace WebApplicationEF.Middlewares
{
  public class HealthMiddleware
  {
    private RequestDelegate Next { get; }
    public HealthMiddleware(RequestDelegate nextMiddleware)
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

  public class AuthenticationMiddleware
  {
    private RequestDelegate Next { get; }
    public AuthenticationMiddleware(RequestDelegate nextMiddleware)
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
