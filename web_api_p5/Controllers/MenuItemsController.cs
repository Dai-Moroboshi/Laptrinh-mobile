using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_api_p5.Data;
using web_api_p5.DTOs;
using web_api_p5.Models;

namespace web_api_p5.Controllers
{
    [Route("api/menu-items")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MenuItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/menu-items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItems(
            [FromQuery] string? search,
            [FromQuery] string? category,
            [FromQuery] bool? vegetarian_only,
            [FromQuery] bool? spicy_only,
            [FromQuery] bool? available_only,
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            var query = _context.MenuItems.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(m => m.Name.Contains(search) || m.Description.Contains(search));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(m => m.Category == category);
            }

            if (vegetarian_only == true)
            {
                query = query.Where(m => m.IsVegetarian);
            }
            
            if (spicy_only == true)
            {
                query = query.Where(m => m.IsSpicy);
            }
            
            if (available_only == true)
            {
                query = query.Where(m => m.IsAvailable);
            }
            
            var items = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();
                
            return items;
        }

        // GET: api/menu-items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItem(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);

            if (menuItem == null)
            {
                return NotFound();
            }

            return menuItem;
        }
        
        // GET: api/menu-items/search (already handled in GetMenuItems but keeping specific endpoint if needed)
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> SearchMenuItems([FromQuery] string q)
        {
             return await _context.MenuItems
                .Where(m => m.Name.Contains(q) || m.Description.Contains(q))
                .ToListAsync();
        }

        // POST: api/menu-items
        [HttpPost]
        // [Authorize(Roles = "Admin")] // Commented out for easier testing, uncomment in production
        public async Task<ActionResult<MenuItem>> PostMenuItem(MenuItemCreateRequest request)
        {
            var menuItem = new MenuItem
            {
                Name = request.Name,
                Description = request.Description ?? "",
                Category = request.Category,
                Price = request.Price,
                ImageUrl = request.ImageUrl ?? "",
                PreparationTime = request.PreparationTime,
                IsVegetarian = request.IsVegetarian,
                IsSpicy = request.IsSpicy,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMenuItem", new { id = menuItem.Id }, menuItem);
        }

        // PUT: api/menu-items/5
        [HttpPut("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutMenuItem(int id, MenuItemUpdateRequest request)
        {
             var menuItem = await _context.MenuItems.FindAsync(id);
             if (menuItem == null)
             {
                 return NotFound();
             }
             
             menuItem.Name = request.Name;
             menuItem.Description = request.Description ?? menuItem.Description;
             menuItem.Category = request.Category;
             menuItem.Price = request.Price;
             menuItem.ImageUrl = request.ImageUrl ?? menuItem.ImageUrl;
             menuItem.PreparationTime = request.PreparationTime;
             menuItem.IsVegetarian = request.IsVegetarian;
             menuItem.IsSpicy = request.IsSpicy;
             menuItem.IsAvailable = request.IsAvailable;
             menuItem.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/menu-items/5
        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return NotFound();
            }
            
            // Check dependency: reservation_items
            bool hasOrders = await _context.ReservationItems.AnyAsync(ri => ri.MenuItemId == id);
            if (hasOrders)
            {
                 // Check if any active reservation uses this? Simplification: Just allow delete if no orders, or soft delete.
                 // Requirements say: "Kiểm tra item có trong reservation_items không (reservation chưa completed)"
                 var activeOrders = await _context.ReservationItems
                    .Include(ri => ri.Reservation)
                    .AnyAsync(ri => ri.MenuItemId == id && ri.Reservation.Status != "completed" && ri.Reservation.Status != "cancelled");
                    
                 if (activeOrders)
                 {
                     return BadRequest(new { message = "Cannot delete item associated with active reservations" });
                 }
            }

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
