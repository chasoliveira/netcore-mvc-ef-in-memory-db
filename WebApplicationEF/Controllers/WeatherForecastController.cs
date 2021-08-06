using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationEF.Controllers
{

  [ApiController]
  [Route("[controller]")]

  public class WeatherForecastController : ControllerBase
  {
    private static readonly string[] Summaries = new[]
    {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly WebEFDbContext dbContext;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, WebEFDbContext dbContext)
    {
      _logger = logger;
      this.dbContext = dbContext;
    }

    [HttpPost]
    [Authorize]
    public IActionResult Post(int temperature)
    {
      var rng = new Random();
      var newTemperature = new WeatherForecast
      {
        Date = DateTime.Now,
        TemperatureC = temperature,
        Summary = Summaries[rng.Next(Summaries.Length)]
      };

      dbContext.WeatherForecasters.Add(newTemperature);
      dbContext.SaveChanges();

      return Ok();
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
      return dbContext.WeatherForecasters.ToList();
    }

    [HttpGet("{id}")]
    public WeatherForecast Get(int id)
    {
      return dbContext.WeatherForecasters.FirstOrDefault(e => e.WetherKey == id);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult Delete(int id)
    {
      var item = dbContext.WeatherForecasters.FirstOrDefault(e => e.WetherKey == id);
      dbContext.WeatherForecasters.Remove(item);
      dbContext.SaveChanges();
      return NoContent();
    }
  }

  public class UserModel
  {
    public string UserName { get; set; }
    public string Password { get; set; }
  }
}
