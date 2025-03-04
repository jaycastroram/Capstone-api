using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Capstone.Api.Models;
using Capstone.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Capstone.Api.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Capstone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(
            UserManager<User> userManager, 
            ApplicationDbContext context,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDTO registration)
        {
            User user = new User
            {
                UserName = registration.UserName,
                Email = registration.Email,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                ImageLocation = registration.ImageLocation,
                Role = "User",
                CreateDateTime = DateTime.UtcNow
            };

            IdentityResult result = await _userManager.CreateAsync(user, registration.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                return Ok(new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    IsVerified = user.IsVerified,
                    ImageLocation = user.ImageLocation,
                    CreateDateTime = user.CreateDateTime
                });
            }

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation("Getting all users. User: {User}", User?.Identity?.Name);

                var users = await _context.Users
                    .Include(u => u.Photographer)
                    .Select(u => new
                    {
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                        u.Role,
                        u.IsVerified,
                        u.ImageLocation,
                        Photographer = u.Photographer != null ? new
                        {
                            u.Photographer.Id,
                            u.Photographer.Bio,
                            u.Photographer.PortfolioLink
                        } : null
                    })
                    .ToListAsync();

                _logger.LogInformation("Found {Count} users", users.Count);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _context.Users
                .Include(u => u.Photographer)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDTO model)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.IsVerified = model.IsVerified;

            if (user.Role == "Photographer" && user.Photographer != null)
            {
                user.Photographer.Bio = model.Bio;
                user.Photographer.PortfolioLink = model.PortfolioLink;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // If the user is a photographer, delete the photographer record first
            if (user.Role == "Photographer" && user.Photographer != null)
            {
                _context.Photographers.Remove(user.Photographer);
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    }
} 