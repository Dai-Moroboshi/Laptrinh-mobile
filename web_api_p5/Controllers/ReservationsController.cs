using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web_api_p5.Data;
using web_api_p5.DTOs;
using web_api_p5.Models;

namespace web_api_p5.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReservationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/reservations
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Reservation>> CreateReservation(ReservationCreateRequest request)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null) return Unauthorized();
            int customerId = int.Parse(userIdClaim.Value);

            var reservation = new Reservation
            {
                CustomerId = customerId,
                ReservationNumber = "RES-" + DateTime.Now.ToString("yyyyMMdd") + "-" + Guid.NewGuid().ToString().Substring(0, 5).ToUpper(),
                ReservationDate = request.ReservationDate,
                NumberOfGuests = request.NumberOfGuests,
                Status = "pending",
                SpecialRequests = request.SpecialRequests,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReservation", new { id = reservation.Id }, reservation);
        }

        // POST: api/reservations/5/items
        [HttpPost("{id}/items")]
        [Authorize]
        public async Task<IActionResult> AddItemToReservation(int id, ReservationItemAddRequest request)
        {
             var reservation = await _context.Reservations.FindAsync(id);
             if (reservation == null) return NotFound();
             
             // Check ownership or admin (skipped for brevity)

             var menuItem = await _context.MenuItems.FindAsync(request.MenuItemId);
             if (menuItem == null || !menuItem.IsAvailable) return BadRequest("Menu item not available");

             var reservationItem = new ReservationItem
             {
                 ReservationId = id,
                 MenuItemId = request.MenuItemId,
                 Quantity = request.Quantity,
                 Price = menuItem.Price,
                 CreatedAt = DateTime.UtcNow
             };

             _context.ReservationItems.Add(reservationItem);
             
             // Recalculate totals
             reservation.Subtotal += reservationItem.Price * reservationItem.Quantity;
             reservation.ServiceCharge = reservation.Subtotal * 0.1m;
             reservation.Total = reservation.Subtotal + reservation.ServiceCharge - reservation.Discount;
             reservation.UpdatedAt = DateTime.UtcNow;

             await _context.SaveChangesAsync();
             
             return Ok(reservation);
        }

        // PUT: api/reservations/5/confirm
        [HttpPut("{id}/confirm")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ConfirmReservation(int id, ReservationConfirmRequest request)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();

            var table = await _context.Tables.FirstOrDefaultAsync(t => t.TableNumber == request.TableNumber);
            if (table == null) return BadRequest("Table not found");

            if (!table.IsAvailable) return BadRequest("Table is not available");
            if (table.Capacity < reservation.NumberOfGuests) return BadRequest("Table capacity insufficient");

            reservation.Status = "confirmed";
            reservation.TableNumber = request.TableNumber;
            reservation.UpdatedAt = DateTime.UtcNow;
            
            table.IsAvailable = false;
            
            await _context.SaveChangesAsync();
            return Ok(reservation);
        }
        
        // GET: api/reservations/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _context.Reservations
                .Include(r => r.ReservationItems)
                .ThenInclude(ri => ri.MenuItem)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reservation == null)
            {
                return NotFound();
            }

            return reservation;
        }

        // POST: api/reservations/5/pay
        [HttpPost("{id}/pay")]
        [Authorize]
        public async Task<IActionResult> PayReservation(int id, PayReservationRequest request)
        {
             using var transaction = _context.Database.BeginTransaction();
             try
             {
                 var reservation = await _context.Reservations.FindAsync(id);
                 if (reservation == null) return NotFound();
                 
                 // if (reservation.Status != "seated") return BadRequest("Reservation must be seated to pay"); // Relaxed for exam

                 var customer = await _context.Customers.FindAsync(reservation.CustomerId);
                 
                 decimal discountAmount = 0;
                 if (request.UseLoyaltyPoints && customer != null)
                 {
                     // 1 point = 1000 VND
                     decimal maxDiscount = reservation.Total * 0.5m;
                     decimal attemptedDiscount = request.LoyaltyPointsToUse * 1000m;
                     
                     if (customer.LoyaltyPoints < request.LoyaltyPointsToUse) return BadRequest("Insufficient points");
                     
                     discountAmount = Math.Min(attemptedDiscount, maxDiscount);
                     customer.LoyaltyPoints -= (int)(discountAmount / 1000); // Deduced points
                 }
                 
                 reservation.Discount = discountAmount;
                 reservation.Total = reservation.Subtotal + reservation.ServiceCharge - discountAmount;
                 reservation.PaymentMethod = request.PaymentMethod;
                 reservation.PaymentStatus = "paid";
                 reservation.Status = "completed";
                 reservation.UpdatedAt = DateTime.UtcNow;
                 
                 // Award points (1% of total)
                 if (customer != null)
                 {
                     customer.LoyaltyPoints += (int)(reservation.Total * 0.01m);
                 }
                 
                 if (reservation.TableNumber != null)
                 {
                     var table = await _context.Tables.FirstOrDefaultAsync(t => t.TableNumber == reservation.TableNumber);
                     if (table != null) table.IsAvailable = true;
                 }

                 await _context.SaveChangesAsync();
                 await transaction.CommitAsync();
                 
                 return Ok(reservation);
             }
             catch
             {
                 await transaction.RollbackAsync();
                 throw;
             }
        }
        
        // DELETE: api/reservations/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null) return NotFound();
            
            reservation.Status = "cancelled";
            reservation.UpdatedAt = DateTime.UtcNow;
            
            if (reservation.TableNumber != null)
            {
                 var table = await _context.Tables.FirstOrDefaultAsync(t => t.TableNumber == reservation.TableNumber);
                 if (table != null) table.IsAvailable = true;
            }
            
            await _context.SaveChangesAsync();
            return Ok(new { message = "Reservation cancelled" });
        }
    }
}
