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
    public class LineItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LineItemsController(AppDbContext context)
        {
            _context = context;
        }

        /*
         *  HTTP GET -->
         */

        // GET: api/LineItems | GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LineItem>>> GetLineItems()
        {
            return await _context.LineItems.ToListAsync();
        }

        // GET: api/LineItems/lines-for-pr/4 | GET BY REQUEST ID
        [HttpGet("lines-for-pr/{id}")]
        public async Task<ActionResult<IEnumerable<LineItem>>> GetByRequestId(int id)
        {
            var lineItem = await _context.LineItems.Where(li => li.RequestId == id).ToListAsync();

            if (lineItem == null)
            {
                return null; ;
            }

            return lineItem;
        }

        // GET: api/LineItems/5 | GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<LineItem>> GetLineItem(int id)
        {
            var lineItem = await _context.LineItems.FindAsync(id);

            if (lineItem == null)
            {
                return NotFound();
            }

            return lineItem;
        }

        /*
         *  HTTP PUT -->
         */

        //Put: api/LineItems | UPDATE LINEITEM WITHOUT ID IN HTML SEARCH
        [HttpPut]
        public async Task<IActionResult> PutLineItem(LineItem lineItem)
        {

            _context.Entry(lineItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PUT: api/LineItems/5 | UPDATE && CALCULATE REQUEST TOTAL
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLineItem(int id, LineItem lineItem)
        {
            if (id != lineItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(lineItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LineItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            await RecalculateTotal(id);

            return NoContent();
        }

        /*
         *  HTTP POST -->
         */

        // POST: api/LineItems | ADD LINEITEM && CALCULATE REQUEST TOTAL
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LineItem>> PostLineItem(LineItem lineItem)
        {
            _context.LineItems.Add(lineItem);
            await _context.SaveChangesAsync();

            await RecalculateTotal(lineItem.RequestId); // Update the Request Total

            return CreatedAtAction("GetLineItem", new { id = lineItem.Id }, lineItem);
        }

        /*
         *  HTTP DELETE -->
         */

        // DELETE: api/LineItems/5 | DELETE LINEITEM
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLineItem(int id)
        {
            var lineItem = await _context.LineItems.FindAsync(id);
            if (lineItem == null)
            {
                return NotFound();
            }

            _context.LineItems.Remove(lineItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LineItemExists(int id)
        {
            return _context.LineItems.Any(e => e.Id == id);
        }

        /*
         *  BONUS FUNCTIONS -->
         */

        // Recalculation for the total in Class Requests
        public async Task RecalculateTotal(int requestId)
        {
            var request = await _context.Requests.FindAsync(requestId);

            request.Total = (from l in _context.LineItems
                             join p in _context.Products
                             on l.ProductId equals p.Id
                             where l.RequestId == requestId
                             select new { Total = l.Quantity * p.Price }).Sum(i => i.Total);

            var rowsChanged = await _context.SaveChangesAsync();

            if (rowsChanged != 1)
                throw new Exception("Fatal Error: Did not calculate.");
        }
    }
}
