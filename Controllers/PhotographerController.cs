using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Capstone.Api.Models;
using Capstone.Api.Models.DTOs;
using Capstone.Api.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Web;
using System.Security.Claims;

namespace Capstone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotographerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public PhotographerController(
            ApplicationDbContext context, 
            UserManager<User> userManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var photographers = await _context.Photographers
                .Include(p => p.User)
                .Include(p => p.Packages)
                .Select(p => new PhotographerDTO
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Name = p.Name,
                    Bio = p.Bio,
                    PortfolioLink = p.PortfolioLink,
                    User = new UserDTO
                    {
                        Id = p.User.Id,
                        UserName = p.User.UserName,
                        Email = p.User.Email,
                        FirstName = p.User.FirstName,
                        LastName = p.User.LastName,
                        ImageLocation = p.User.ImageLocation,
                        IsVerified = p.User.IsVerified
                    }
                })
                .ToListAsync();

            return Ok(photographers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var photographer = await _context.Photographers
                .Include(p => p.User)
                .Include(p => p.Packages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (photographer == null) return NotFound();

            return Ok(photographer);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreatePhotographerDTO photographerDto)
        {
            try 
            {
                // Create the user account
                var user = new User
                {
                    UserName = photographerDto.Email,
                    Email = photographerDto.Email,
                    FirstName = photographerDto.Name.Split(' ')[0],
                    LastName = photographerDto.Name.Split(' ').Length > 1 ? photographerDto.Name.Split(' ')[1] : "",
                    Role = "Photographer",
                    IsVerified = false, // This will indicate if they've completed initial setup
                    CreateDateTime = DateTime.UtcNow
                };

                // Set default password as "Password123!"
                var result = await _userManager.CreateAsync(user, "Password123!");
                if (!result.Succeeded)
                {
                    return BadRequest(new { message = "Failed to create user", errors = result.Errors });
                }

                // Assign the Photographer role
                await _userManager.AddToRoleAsync(user, "Photographer");

                // Double check that the user is still not verified
                // Create initial photographer record
                var photographer = new Photographer
                {
                    UserId = user.Id,
                    Name = photographerDto.Name,
                    Bio = photographerDto.Bio,
                    PortfolioLink = photographerDto.PortfolioLink
                };

                _context.Photographers.Add(photographer);
                await _context.SaveChangesAsync();

                // Return the user info and photographer ID
                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = photographer.Id },
                    new { 
                        id = photographer.Id,
                        email = user.Email,
                        message = "Photographer created successfully. They can log in with email and default password: Password123!"
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating photographer", error = ex.Message });
            }
        }

        [HttpPost("profile")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateProfile([FromBody] CreatePhotographerProfileDTO profileDto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(profileDto.UserId);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Check if photographer profile already exists
                var existingPhotographer = await _context.Photographers
                    .FirstOrDefaultAsync(p => p.UserId == profileDto.UserId);

                if (existingPhotographer != null)
                {
                    // Update existing profile
                    existingPhotographer.Bio = profileDto.Bio;
                    existingPhotographer.PortfolioLink = profileDto.PortfolioLink;
                    existingPhotographer.ProfileImage = profileDto.ProfileImage;
                    existingPhotographer.ContactInfo = profileDto.ContactInfo;
                }
                else
                {
                    // Create new photographer profile
                    var photographer = new Photographer
                    {
                        UserId = profileDto.UserId,
                        Name = $"{user.FirstName} {user.LastName}",
                        Bio = profileDto.Bio,
                        PortfolioLink = profileDto.PortfolioLink,
                        ProfileImage = profileDto.ProfileImage,
                        ContactInfo = profileDto.ContactInfo
                    };

                    _context.Photographers.Add(photographer);
                }

                // Don't update verification status here - it will be updated when they change their password
                await _context.SaveChangesAsync();

                return Ok(new { message = "Photographer profile created successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating photographer profile", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePhotographerDTO photographerDto)
        {
            var photographer = await _context.Photographers
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (photographer == null) return NotFound();

            photographer.Name = photographerDto.Name;
            photographer.Bio = photographerDto.Bio;
            photographer.PortfolioLink = photographerDto.PortfolioLink;

            if (!string.IsNullOrEmpty(photographerDto.Password))
            {
                var hasher = new PasswordHasher<User>();
                photographer.User.PasswordHash = hasher.HashPassword(photographer.User, photographerDto.Password);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var photographer = await _context.Photographers
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (photographer == null) return NotFound();

                // Store the user before removing the photographer
                var user = photographer.User;

                // Remove the photographer first (due to foreign key constraints)
                _context.Photographers.Remove(photographer);
                await _context.SaveChangesAsync();

                // Then remove the user if it exists
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting photographer", error = ex.Message });
            }
        }
    }
} 