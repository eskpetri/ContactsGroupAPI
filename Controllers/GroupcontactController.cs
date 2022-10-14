using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using contactgroupAPIefMySQL.Models;
using Microsoft.AspNetCore.Authorization;

namespace contactgroupAPIefMySQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupcontactController : ControllerBase
    {
        private readonly contactgroupContext _context;
        public GroupcontactController(contactgroupContext context)
        {
            _context = context;
        }

        [HttpGet, Authorize(Roles = "User,Admin")]

        public async Task<ActionResult<List<Groupcontact>>> GetGroups()
        {
            return Ok(await _context.Groupcontacts.ToListAsync());
        }

        [HttpGet("{id}"), Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<Groupcontact>> Get(int id)
        {
            var dbGroupcontacts = await _context.Groupcontacts.FindAsync(id);
            if (dbGroupcontacts == null)
                return BadRequest("Groupcontact not found.");
            return Ok(dbGroupcontacts);
        }

        [HttpPost, Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<List<Groupcontact>>> AddGroup(Groupcontact cont)
        {
            if (cont.Isadmin == null) { cont.Isadmin = 0; }   //Lets keep isadmin 0 for not and 1 for admin. no nulls
            _context.Groupcontacts.Add(cont);
            await _context.SaveChangesAsync();

            return Ok(cont.Idgroupcontacts);             //Check that return last inserted id
        }

        [HttpPut, Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<Groupcontact>> UpdateGroupcontact(Groupcontact request)
        {
            var dbGroupcontact = await _context.Groupcontacts.FindAsync(request.Idgroupcontacts);
            if (dbGroupcontact == null)
                return BadRequest("Groupcontact not found.");
            
            dbGroupcontact.Idcontacts = request.Idcontacts;
            dbGroupcontact.Idgroups = request.Idgroups;
            dbGroupcontact.Isadmin = request.Isadmin;

            await _context.SaveChangesAsync();

            return Ok(dbGroupcontact);
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<Groupcontact>> Delete(int id)
        {
            var dbGroupcontact = await _context.Groupcontacts.FindAsync(id);
            if (dbGroupcontact == null)
                return BadRequest("Groupcontact not found.");

            _context.Groupcontacts.Remove(dbGroupcontact);
            await _context.SaveChangesAsync();

            return Ok(dbGroupcontact);
        }
        [HttpPost("cg/"), Authorize(Roles = "User,Admin")]    //Axion ei tykkää deletestä ja JSONista samaan aikaan. Ihme säätöä
        public async Task<ActionResult<Groupcontact>> Deletecg(Groupcontact cont)
        {
            List<Groupcontact> lgc = await _context.Groupcontacts.ToListAsync();
            var dbGroupcontact = new Groupcontact();
                    
            foreach (var Groupc in lgc)
            {
                if (Groupc.Idcontacts == cont.Idcontacts && Groupc.Idgroups == cont.Idgroups) {
                    dbGroupcontact = Groupc;
                }
            }
            Console.WriteLine("cgid="+dbGroupcontact.Idgroupcontacts);

            if (dbGroupcontact.Idgroupcontacts == 0)
                return BadRequest("Groupcontact not found.");

            _context.Groupcontacts.Remove(dbGroupcontact);
            await _context.SaveChangesAsync();

            return Ok(dbGroupcontact);
        }

    }
}
