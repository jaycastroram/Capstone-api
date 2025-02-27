using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Capstone.Api.Models;
using Capstone.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Capstone.Api.Data;

namespace Capstone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public UserController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
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
    }
} 