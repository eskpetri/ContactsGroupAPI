using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using contactgroupAPIIefMySQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Security.Principal;

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
            //string jwt = string.Empty;
            Console.WriteLine("body username = "+ body.Username);
            Console.WriteLine("body password = "+body.Password);
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
            Console.WriteLine("db username = "+ dbContact.Username);
            Console.WriteLine("db password = "+dbContact.Password);
          /*  Console.WriteLine("VerifyPWD = "+MyPublicClass.VerifyPassword(body.Password, dbContact.Password));
            Console.WriteLine("VerifyPWDTestT = "+MyPublicClass.VerifyPassword("pwd", dbContact.Password));
            Console.WriteLine("VerifyPWDTestF = "+MyPublicClass.VerifyPassword("väärä", dbContact.Password));*/
            if (! MyPublicClass.VerifyPassword(body.Password, dbContact.Password))
            {
                // authentication failed
                return new OkObjectResult(false);
            }
            else
            {
                // authentication successful

                return new OkObjectResult(true);
            }
        }
        
        // POST api/login/register
        [HttpPost("register")]
        public async Task<IActionResult> SetRegisterNewUser([FromBody]Contact body)
        {
            //string jwt = string.Empty;
            if (body.Isadmin == null) { body.Isadmin = 0; }   //Lets keep isadmin 0 for not and 1 for admin. no nulls
            body.Password = MyPublicClass.EncryptPassword(body.Password);  //This should be done using method() now in several places
            _context.Contacts.Add(body);
            await _context.SaveChangesAsync();

            return Ok(body.Idcontacts);             //Check that return last inserted id
        }

        [HttpPost("jwt")]
        public async Task<ActionResult<string>> Get([FromBody]Contact body)
        {
            string jwt = string.Empty;
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
                return new OkObjectResult(jwt);
            }
            else
            {
                // authentication successful
                jwt = CreateToken(dbContact);           //Create JWT token here and put it in httponly cookie?!? Roles are admin and user
                Console.WriteLine("jwt="+jwt);
                setTokenCookie(jwt);
                //JWT to httponly Cookie. here
                return Ok(jwt);             //Check that return last inserted id
                //jwt = CreateToken(dbContact);//Create JWT token here and put it in httponly cookie?!? Roles are admin and user
            }
  
        }
        [AllowAnonymous]
        [HttpPost("renew-token")]
        public IActionResult RenewToken()
        {
            string? refreshToken = Request.Cookies["renewToken"];
            //Tähän validointi, että uusitaan oikean käyttäjän tokeni
            setTokenCookie(refreshToken);
            return Ok(refreshToken);
        }
        [AllowAnonymous]
        [HttpGet("get-token-from-cookie")]
        public ActionResult<string> CookieToToken()
        {
            string? refreshToken = Request.Cookies["renewToken"];
            //Boolean ret = ValidateCurrentToken(refreshToken);
            //Console.WriteLine("Bool="+ret);
            //Validate token here before pushing it back!!!!!!!!!!!!!!!!!!!! or invalid token will have credentials at frontend
            //Or invalid created token will have access in frontend
            if (refreshToken==null)
                return BadRequest("No cookie");

            if (ValidateToken(refreshToken))    
                return Ok(refreshToken);
            else
                return BadRequest("Invalid Token");
        }
        //[HttpPost("logout"), Authorize(Roles = "User,Admin")]
        [HttpPost("logout")]  //Needs to be authenticated to logout
        public ActionResult<string> Logout()
        {
            var token ="";
            //Response.Cookies.Delete("renewToken");  //Does not work with Httponly cookies in frontend
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(-1)
            };
            Response.Cookies.Append("renewToken", token, cookieOptions);
            //Response.Cookies.Append
            return Ok("Cookie deleted");
        }

    /*    public bool VerifyToken(string token)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(System.Environment.GetEnvironmentVariable("SecredKey")));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }*/
        private string CreateToken(Contact user)       // Check out created token content at https://jwt.io/ <-- CopyPaste token string there
        {
            string Role = string.Empty;
            if (user.Isadmin == 1) {
                Role="Admin";
            }
            else {
                Role="User";  //Includes groupAdmin handled in FrontEnd
            }

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Idcontacts.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, Role)

            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(System.Environment.GetEnvironmentVariable("SecredKey")));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(15),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
          // Console.WriteLine("IsValidated as true= "+ValidateToken(jwt));
          // Console.WriteLine("IsValidated as false= "+ValidateToken(" "+jwt));
            return jwt;
        }
    private static bool ValidateToken(string authToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();
        try
        {
            SecurityToken validatedToken;
            IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
            }
        catch (System.Exception ex)
        {
            Console.WriteLine("Validoinnissa virhe = "+ex.Message);
            return false;
        }

        return true;
    }

    private static TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters()
        {
            ValidateLifetime = false, // Because there is no expiration in the generated token
            ValidateAudience = false, // Because there is no audiance in the generated token
            ValidateIssuer = false,   // Because there is no issuer in the generated token
            ValidIssuer = "Sample",
            ValidAudience = "Sample",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(System.Environment.GetEnvironmentVariable("SecredKey"))) // The same key as the one that generate the token
        };
    }

        private void setTokenCookie(string token)
        {
            // append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddDays(15)
            };
            Response.Cookies.Append("renewToken", token, cookieOptions);
        }
/*
public bool ValidateCurrentToken(string token)
{
	var mySecret = System.Environment.GetEnvironmentVariable("SecredKey");
	var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

	var myIssuer = "http://localhost";
	var myAudience = "http://localhost";

	var tokenHandler = new JwtSecurityTokenHandler();
	try
	{
		tokenHandler.ValidateToken(token, new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidIssuer = myIssuer,
			ValidAudience = myAudience,
			IssuerSigningKey = mySecurityKey
		}, out SecurityToken validatedToken);
	}
	catch
	{
		return false;
	}
	return true;
}*/
    }
}