using contactgroupAPIefMySQL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace contactgroupAPIefMySQL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly contactgroupContext _context;
        public ContactsController(contactgroupContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<List<Contact>>> GetContacts()
        {
            return Ok(await _context.Contacts.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> Get(int id)
        {
            var dbContact = await _context.Contacts.FindAsync(id);
            if (dbContact == null)
                return BadRequest("Contact not found.");
            return Ok(dbContact);
        }
        
        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetContactsid(string username)
        {
            var dbContact = new Contact();
            try
            {
                dbContact = await _context.Contacts.SingleAsync(x => x.Username == username);
            }
            catch (System.Exception)
            {
                return BadRequest("Contact not found.");
            }
            return Ok(dbContact);
        }

        [HttpPost]
        public async Task<ActionResult<List<Contact>>> AddContact(Contact cont)
        {
            if (cont.Isadmin == null) { cont.Isadmin = 0; }   //Lets keep isadmin 0 for not and 1 for admin. no nulls
            Console.WriteLine("Isadmin="+cont.Isadmin);
            cont.Password = MyPublicClass.EncryptPassword(cont.Password);   //Make pwd crypted : Change to SHA3 512 if time
            _context.Contacts.Add(cont);
            await _context.SaveChangesAsync();

            return Ok(cont.Idcontacts);             //Check that return last inserted id
        }

        [HttpPut]
        public async Task<ActionResult<Contact>> UpdateContact(Contact request)
        {
            var dbContact = await _context.Contacts.FindAsync(request.Idcontacts);
            if (dbContact == null)
                return BadRequest("Contact not found.");

            dbContact.Idcontacts = request.Idcontacts;  
            dbContact.Username = request.Username;
            dbContact.Password = MyPublicClass.EncryptPassword(request.Password);
            dbContact.Nickname = request.Nickname;
            dbContact.Email = request.Email;
            dbContact.Phone = request.Phone;
            dbContact.Isadmin = request.Isadmin;

            if (dbContact.Isadmin == null) { dbContact.Isadmin = 0; }

            await _context.SaveChangesAsync();

            return Ok(dbContact);   //Needs to return updated 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Contact>> Delete(int id)
        {
            var dbContact = await _context.Contacts.FindAsync(id);
            if (dbContact == null)
                return BadRequest("Contact not found.");

            _context.Contacts.Remove(dbContact);
            await _context.SaveChangesAsync();

            return Ok(dbContact);
        }

    }
}
