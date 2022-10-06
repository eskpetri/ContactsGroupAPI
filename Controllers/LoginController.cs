using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace contactgroupAPI.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        public Database Db { get; }
        public LoginController(Database db)
        {
            Db = db;
        }
        // POST api/login
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Contacts body)
        {
            Console.WriteLine(body.username);
            Console.WriteLine(body.password);
            Console.WriteLine(body.idcontacts);
            await Db.Connection.OpenAsync();
            var query = new Login(Db);
            var result = await query.GetPassword(body.username);
   
            if (result is null || ! BCrypt.Net.BCrypt.Verify(body.password, result))
            {
                // authentication failed
                return new OkObjectResult(false);
            }
            else
            {
                // authentication successful

                //Create JWT token here and put it in httponly cookie?!?

                return new OkObjectResult(true);
            }
            
        }
        
        // POST api/login/register
        [HttpPost("register")]
        public async Task<IActionResult> GetContactsid(string username)
        {
            await Db.Connection.OpenAsync();
            var query = new Contacts(Db);
            Console.WriteLine("username = "+username);
            var result = await query.GetContactsidAsync(username);
            if (result == 0)
                return new NotFoundResult();
            return new OkObjectResult(result);

        }
    }
}