using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capstone.Api.Models;
using Capstone.Api.Models.DTOs;
using Capstone.Api.Data;
using System.Security.Claims;

namespace Capstone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BookingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserBookings()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userRole == "Photographer")
            {
                var photographer = await _context.Photographers
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                if (photographer == null) return NotFound("Photographer not found");

                var photographerBookings = await _context.Bookings
                    .Include(b => b.User)
                    .Include(b => b.Package)
                    .Include(b => b.Inquiry)
                    .Where(b => b.Package.PhotographerId == photographer.Id)
                    .ToListAsync();

                return Ok(photographerBookings);
            }

            var userBookings = await _context.Bookings
                .Include(b => b.Package)
                    .ThenInclude(p => p.Photographer)
                .Include(b => b.Inquiry)
                .Where(b => b.UserId == userId)
                .ToListAsync();

            return Ok(userBookings);
        }

        [HttpGet("pending-inquiries")]
        public async Task<IActionResult> GetPendingInquiries()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var pendingInquiries = await _context.Inquiries
                .Include(i => i.Photographer)
                .Include(i => i.Package)
                .Where(i => i.UserId == userId && i.Status == "open")
                .ToListAsync();

            return Ok(pendingInquiries);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin,Photographer")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Package)
                .Include(b => b.Photographer)
                .ToListAsync();

            return Ok(bookings);
        }

        [HttpPut("booking/{id}/status")]
        [Authorize(Roles = "Admin,Photographer")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] StatusUpdateDTO statusUpdate)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            booking.Status = statusUpdate.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("inquiry/{id}/status")]
        [Authorize(Roles = "Admin,Photographer")]
        public async Task<IActionResult> UpdateInquiryStatus(int id, [FromBody] StatusUpdateDTO statusUpdate)
        {
            var inquiry = await _context.Inquiries.FindAsync(id);
            if (inquiry == null) return NotFound();

            inquiry.Status = statusUpdate.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 