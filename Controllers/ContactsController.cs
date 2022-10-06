using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace contactgroupAPI.Controllers
{
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        public Database Db { get; }

        public ContactsController(Database db)
        {
            Db = db;
        }

        // GET api/Contacts
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            await Db.Connection.OpenAsync();
            var query = new Contacts(Db);
            var result = await query.GetAllAsync();
            return new OkObjectResult(result);
        }

        // GET api/Contacts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new Contacts(Db);
            var result = await query.FindOneAsync(id);
            if (result is null)
                return new NotFoundResult();
            return new OkObjectResult(result);

        }
        // GET api/Contacts/5
        [HttpGet("username/{username}")]
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

        // POST api/Contacts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Contacts body)
        {
            await Db.Connection.OpenAsync();
            body.password = BCrypt.Net.BCrypt.HashPassword(body.password);
            body.Db = Db;
            int result = await body.InsertAsync();
            Console.WriteLine("inserted id=" + result);
            if (result == 0)
            {
                return new ConflictObjectResult(0);
            }
            return new OkObjectResult(result);
        }

        // PUT api/Contacts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOne(int id, [FromBody] Contacts body)
        {
            Console.WriteLine("update called");
            await Db.Connection.OpenAsync();
            Console.WriteLine("Update body: "+body);
            body.password = BCrypt.Net.BCrypt.HashPassword(body.password);
            body.idcontacts=id;
            body = new Contacts(Db, body);      //Roughly right not so nice coding but works
            int updateTest = 0;    //assume that no rows are affected
            try
            {
                updateTest = await body.UpdateAsync();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Virhe: "+ex.Message);
                return new NotFoundResult();
            }                  
            if (updateTest == 0)
            {
                return new BadRequestResult();
            }
            else
            {
                return new OkObjectResult(updateTest);
            }
        }

        // DELETE api/Contacts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOne(int id)
        {
            await Db.Connection.OpenAsync();
            var query = new Contacts(Db);
            query.idcontacts=id;
            int rowsa = 0;
            try
            {
                rowsa = await query.DeleteAsync();
                Console.WriteLine("DeleteOnePoistettu arvo "+rowsa);
            }
            catch (System.Exception)
            {
                return new NotFoundResult();
            }
            if (rowsa==0){return new NotFoundResult();}
            else {return new OkObjectResult(query);}
        }
    }
}