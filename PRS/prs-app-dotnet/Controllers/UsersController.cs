using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prs_app_dotnet.Models;

namespace prs_app_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        /*
         *  HTTP GET -->
         */

        // GET: api/Users/username/password | LOGIN USER
        [HttpGet("{username}/{password}")]
        public async Task<ActionResult<User>> LoginUser(string username, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Username.ToLower().Equals(username.ToLower())
                                                                    && x.Password.Equals(password));
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        // GET: api/Users | GET ALL USERS
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5 | GET USER BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        /*
         *  HTTP PUT -->
         */

        // PUT: api/Users/5 | UPDATE USER
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //Put: api/Users | UPDATE USER WITHOUT ID IN HTML SEARCH
        [HttpPut]
        public async Task<IActionResult> PutUser(User user)
        {

            _context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        /*
         *  HTTP POST -->
         */

        // POST: api/Users | ADD USER
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // Post: api/users/login | LOGIN USER USING POST && JSON BODY
        // Unlike java, user must have null class attributes in json body.
        [HttpPost("{login}")]
        public async Task<ActionResult<User>> LoginUser(User user)
        {
            var login = await _context.Users
                                      .FirstOrDefaultAsync //Allows null
                                      (u => u.Username == user.Username && u.Password == user.Password);

            if (login == null)
            {
                return NotFound();
            }
            return login;

        }

        /*
         *  HTTP DELETE -->
         */

        // DELETE: api/Users/5 | DELETE USER
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
