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
    public class RequestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RequestsController(AppDbContext context)
        {
            _context = context;
        }

        /*
         *  HTTP GET -->
         */

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        /*
         *  HTTP PUT -->
         */

        // Put: api/Users/Approve/2 | APPROVE STATUS
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveRequest(int id, Request request)
        {
            request.Status = "Approved";
            return await PutRequest(id, request);
        }

        // Put: api/Users/Reject/2 | REJECT STATUS
        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectRequest(int id, Request request)
        {
            request.Status = "Rejected";
            return await PutRequest(id, request);
        }

        //Put: api/Users/Review | REVIEW STATUS
        [HttpPut("review")] // <- references url && {} <- references parameter
        public async Task<IActionResult> SubmitReview(Request request)
        {
            _context.Entry(request).State = EntityState.Modified;

            request.SubmittedDate = DateTime.Now;
            request.Status = (request.Total <= 50) ? "Approve" : "Review";

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //Put: api/Requests <- ID NOT REQUIRED IN SEARCH BAR | UPDATE
        [HttpPut]
        public async Task<IActionResult> PutUser(Request request)
        {

            _context.Entry(request).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/Requests/5 <- ID REQUIRED IN SEARCH BAR | UPDATE
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        /*
         *  HTTP POST -->
         */

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.Id }, request);
        }

        /*
         *  HTTP DELETE -->
         */

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return _context.Requests.Any(e => e.Id == id);
        }
    }
}
