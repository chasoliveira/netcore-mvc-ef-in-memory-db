using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationEF
{
  public class WebEFDbContext : DbContext
  {
    public DbSet<WeatherForecast> WeatherForecasters { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseInMemoryDatabase("web-ef-db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.Entity<WeatherForecast>().HasKey(p => p.WetherKey);
    }

    public List<String> ToListChanges()
    {
      return this.ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).Select(e => e.State.ToString()).ToList();
    }

    public List<WeatherForecast> ListWF()
    {
      var list = from e in WeatherForecasters
                 join en in WeatherForecasters on e.Summary equals en.Summary
                 where e.TemperatureC > 10
                 select e;
      return list.ToList();

      //return WeatherForecast.Where(e => e.TemperatureC > 10).ToList();
    }
  }
}
