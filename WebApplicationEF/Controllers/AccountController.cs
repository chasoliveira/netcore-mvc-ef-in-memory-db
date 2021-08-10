using Microsoft.AspNetCore.Mvc;
using WebApplicationEF.Contexts;
using WebApplicationEF.Models;
using WebApplicationEF.ViewModels;

namespace WebApplicationEF.Controllers
{
  [ApiController]
  [Route("[controller]")]

  public class AccountController : ControllerBase
  {

    private readonly WebEFDbContext dbContext;

    public AccountController(WebEFDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    [HttpPost]
    public IActionResult Post([FromBody] UserViewModel user)
    {
      var userCreated = dbContext.Users.Add(new User { UserName = user.UserName, Password = user.Password });
      dbContext.SaveChanges();

      return Ok(userCreated.Entity);
    }
  }
}
