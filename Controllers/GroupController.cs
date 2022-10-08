using contactgroupAPIefMySQL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace contactgroupAPIefMySQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly contactgroupContext _context;
        public GroupController(contactgroupContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<List<Cgroup>>> GetGroups()
        {
            return Ok(await _context.Cgroups.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cgroup>> Get(int id)
        {
            var dbCgroups = await _context.Cgroups.FindAsync(id);
            if (dbCgroups == null)
                return BadRequest("Groups not found.");
            return Ok(dbCgroups);
        }

        [HttpPost]
        public async Task<ActionResult<List<Cgroup>>> AddGroup(Cgroup cont)
        {
            _context.Cgroups.Add(cont);
            await _context.SaveChangesAsync();

            return Ok(cont.Idgroups);             //Check that return last inserted id
        }

        [HttpPut]
        public async Task<ActionResult<Cgroup>> UpdateGroup(Cgroup request)
        {
            var dbCgroup = await _context.Cgroups.FindAsync(request.Idgroups);
            if (dbCgroup == null)
                return BadRequest("Group not found.");
            dbCgroup.Groupname = request.Groupname;
            dbCgroup.Description = request.Description;
            await _context.SaveChangesAsync();

            return Ok(dbCgroup);  
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Cgroup>> Delete(int id)
        {
            var dbCgroup = await _context.Cgroups.FindAsync(id);
            if (dbCgroup == null)
                return BadRequest("Group not found.");

            _context.Cgroups.Remove(dbCgroup);
            await _context.SaveChangesAsync();

            return Ok(dbCgroup);
        }

    }
}
