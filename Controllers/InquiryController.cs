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
    public class InquiryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InquiryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInquiries()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userRole == "Photographer")
            {
                var photographer = await _context.Photographers
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                if (photographer == null) return NotFound("Photographer not found");

                var photographerInquiries = await _context.Inquiries
                    .Where(i => i.PhotographerId == photographer.Id)
                    .Include(i => i.User)
                    .Include(i => i.Package)
                    .ToListAsync();

                return Ok(photographerInquiries);
            }

            var userInquiries = await _context.Inquiries
                .Where(i => i.UserId == userId)
                .Include(i => i.Photographer)
                .Include(i => i.Package)
                .ToListAsync();

            return Ok(userInquiries);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InquiryDTO inquiryDto)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var inquiry = new Inquiry
            {
                UserId = userId,
                PhotographerId = inquiryDto.PhotographerId,
                PackageId = inquiryDto.PackageId,
                Message = inquiryDto.Message,
                Status = "open",
                CreatedAt = DateTime.UtcNow
            };

            _context.Inquiries.Add(inquiry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserInquiries), new { id = inquiry.Id }, inquiry);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, InquiryDTO inquiryDto)
        {
            var inquiry = await _context.Inquiries.FindAsync(id);
            if (inquiry == null) return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (inquiry.UserId != userId) return Forbid();

            inquiry.Message = inquiryDto.Message;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var inquiry = await _context.Inquiries.FindAsync(id);
            if (inquiry == null) return NotFound();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (inquiry.UserId != userId) return Forbid();

            _context.Inquiries.Remove(inquiry);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 