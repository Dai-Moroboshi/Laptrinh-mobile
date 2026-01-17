using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_api_p5.Data;
using web_api_p5.Models;

namespace web_api_p5.Controllers
{
    [Route("api/tables")]
    [ApiController]
    public class TablesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TablesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/tables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TableEntity>>> GetTables([FromQuery] bool? available_only)
        {
            var query = _context.Tables.AsQueryable();

            if (available_only == true)
            {
                query = query.Where(t => t.IsAvailable);
            }

            return await query.ToListAsync();
        }

        // GET: api/tables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TableEntity>> GetTable(int id)
        {
            var table = await _context.Tables.FindAsync(id);

            if (table == null)
            {
                return NotFound();
            }

            return table;
        }

        // POST: api/tables
        [HttpPost]
        // [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TableEntity>> PostTable(TableEntity table)
        {
            _context.Tables.Add(table);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTable", new { id = table.Id }, table);
        }

        // PUT: api/tables/5
        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutTable(int id, TableEntity table)
        {
            if (id != table.Id)
            {
                return BadRequest();
            }

            _context.Entry(table).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TableExists(id))
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

        // DELETE: api/tables/5
        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var table = await _context.Tables.FindAsync(id);
            if (table == null)
            {
                return NotFound();
            }

            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TableExists(int id)
        {
            return _context.Tables.Any(e => e.Id == id);
        }
    }
}
