using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using contactgroupAPIIefMySQL;


namespace contactgroupAPIIefMySQL.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
private readonly contactgroupContext _context;
        public LoginController(contactgroupContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Contact body)
        {
            Console.WriteLine(body.Username);
            Console.WriteLine(body.Password);
            var dbContact = new Contact();
            try
            {
                dbContact = await _context.Contacts.SingleAsync(x => x.Username == body.Username);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Login failed no username"+ex.Message);
                return BadRequest("Login username not found.");
            }

            if (! MyPublicClass.VerifyPassword(body.Password, dbContact.Password))
            {
                // authentication failed
                return new OkObjectResult(false);
            }
            else
            {
                // authentication successful

                //Create JWT token here and put it in httponly cookie?!? Roles are admin and user

                return new OkObjectResult(true);
            }
        }
        
        // POST api/login/register
        [HttpPost("register")]
        public async Task<IActionResult> SetRegisterNewUser([FromBody]Contact body)
        {
            if (body.Isadmin == null) { body.Isadmin = 0; }   //Lets keep isadmin 0 for not and 1 for admin. no nulls
            body.Password = MyPublicClass.EncryptPassword(body.Password);  //This should be done using method() now in several places
            _context.Contacts.Add(body);
            await _context.SaveChangesAsync();

                       //Create JWT token here and put it in httponly cookie?!? Roles are admin and user

            return Ok(body.Idcontacts);             //Check that return last inserted id
        }
    }
}